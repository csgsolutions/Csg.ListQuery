using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Csg.Data.ListQuery.Abstractions;
using Csg.Data.Sql;

namespace Csg.Data.ListQuery
{
    public static class ListQueryExtensions
    {
        public static IListQuery ValidateWith<TValidation>(this IListQuery listQuery) where TValidation : class, new()
        {
            return ValidateWith(listQuery, typeof(TValidation));
        }

        public static IListQuery ValidateWith(this IListQuery listQuery, Type validationType)
        {
            listQuery.ShouldValidate = true;

            var properties = Internal.ReflectionHelper.GetListPropertyInfo(validationType);

            foreach (var property in properties)
            {
                listQuery.Validations.Add(property.Key, property.Value);
            }

            return listQuery;
        }

        public static IListQuery ValidateWith(this IListQuery listQuery, IEnumerable<ListPropertyInfo> fields)
        {
            listQuery.ShouldValidate = true;

            foreach (var field in fields)
            {
                listQuery.Validations.Add(field.Name, field);
            }

            return listQuery;
        }

        public static IListQuery AddFilterHandler(this IListQuery listQuery, string name, ListQueryFilterHandler handler)
        {
            listQuery.Handlers.Add(name, handler);

            return listQuery;
        }

        public static IListQuery AddFilterHandlers<THandlers>(this IListQuery listQuery) where THandlers : class, new()
        {
            return AddFilterHandlers(listQuery, typeof(THandlers));
        }

        public static IListQuery AddFilterHandlers(this IListQuery listQuery, Type handlersType)
        {
            var methods = handlersType.GetMethods(BindingFlags.Static | BindingFlags.Public);

            foreach (var method in methods)
            {
                var handler = (ListQueryFilterHandler)ListQueryFilterHandler.CreateDelegate(typeof(ListQueryFilterHandler), method);
                //TODO: Cache these
                //ListQueryFilterHandler handler = (where, filter, config) =>
                //{
                //    method.Invoke(null, new object[] { where, filter, config });
                //};

                listQuery.Handlers.Add(method.Name, handler);
            }

            return listQuery;
        }
        
        public static IListQuery RemoveHandler(this IListQuery listQuery, string name)
        {
            listQuery.Handlers.Remove(name);
            return listQuery;
        }

        public static IListQuery NoValidation(this IListQuery listQuery)
        {
            listQuery.ShouldValidate = false;
            return listQuery;
        }

        //public static IDbQueryBuilder CreateCacheKey(this IDbQueryBuilder builder, out string cacheKey, string prefix = "QueryBuilder:")
        //{
        //    cacheKey = builder.CreateCacheKey(prefix: prefix);
        //    return builder;
        //}

        //public static string CreateCacheKey(this IDbQueryBuilder builder, string prefix = "QueryBuilder:")
        //{
        //    var hash = System.Security.Cryptography.SHA1.Create();
        //    var stmt = builder.Render();
        //    var buffer = System.Text.UTF8Encoding.UTF8.GetBytes(stmt.CommandText);

        //    hash.TransformBlock(buffer, 0, buffer.Length, null, 0);

        //    foreach (var par in stmt.Parameters)
        //    {
        //        buffer = System.Text.UTF8Encoding.UTF8.GetBytes(par.ParameterName);
        //        hash.TransformBlock(buffer, 0, buffer.Length, null, 0);

        //        buffer = System.Text.UTF8Encoding.UTF8.GetBytes(par.Value.ToString());
        //        hash.TransformBlock(buffer, 0, buffer.Length, null, 0);
        //    }

        //    hash.TransformFinalBlock(buffer, 0, 0);

        //    return string.Concat(prefix, Convert.ToBase64String(hash.Hash));
        //}

        public static void ApplyFilters(IListQuery listQuery, IDbQueryBuilder queryBuilder)
        {
            if (listQuery.QueryDefinition.Filters != null)
            {
                var where = new DbQueryWhereClause(queryBuilder.Root, Sql.SqlLogic.And);

                foreach (var filter in listQuery.QueryDefinition.Filters)
                {
                    var hasConfig = listQuery.Validations.TryGetValue(filter.Name, out ListPropertyInfo validationField);

                    if (listQuery.Handlers.TryGetValue(filter.Name, out ListQueryFilterHandler handler))
                    {
                        handler(where, filter, validationField);
                    }
                    else if (hasConfig || !listQuery.ShouldValidate)
                    {
                        where.AddFilter(filter.Name, filter.Operator ?? GenericOperator.Equal, filter.Value, validationField?.DataType ?? System.Data.DbType.String, validationField?.DataTypeSize);
                    }
                    else if (listQuery.ShouldValidate)
                    {
                        throw new Exception($"No handler is defined for the filter '{filter.Name}'.");
                    }
                }

                if (where.Filters.Count > 0)
                {
                    queryBuilder.AddFilter(where.Filters);
                }
            }
        }

        public static void ApplySelections(IListQuery listQuery, IDbQueryBuilder queryBuilder)
        {
            if (listQuery.QueryDefinition.Selections != null)
            {
                foreach (var column in listQuery.QueryDefinition.Selections)
                {
                    if (listQuery.Validations.TryGetValue(column, out ListPropertyInfo config))
                    {
                        queryBuilder.SelectColumns.Add(new Sql.SqlColumn(queryBuilder.Root, config.Name));
                    }
                    else if (listQuery.ShouldValidate)
                    {
                        throw new Exception($"The selection field '{column}' does not exist.");
                    }
                    else
                    {
                        queryBuilder.SelectColumns.Add(new Sql.SqlColumn(queryBuilder.Root, column));
                    }
                }
            }
        }

        public static void ApplySort(IListQuery listQuery, IDbQueryBuilder queryBuilder)
        {
            if (listQuery.QueryDefinition.Sort != null)
            {
                foreach (var column in listQuery.QueryDefinition.Sort)
                {
                    if (listQuery.Validations.TryGetValue(column.Name, out ListPropertyInfo config) && config.IsSortable == true)
                    {
                        queryBuilder.OrderBy.Add(new Sql.SqlOrderColumn()
                        {
                            ColumnName = config.Name,
                            SortDirection = column.SortDescending ? Sql.DbSortDirection.Descending : Sql.DbSortDirection.Ascending
                        });
                    }
                    else if (listQuery.ShouldValidate)
                    {
                        throw new Exception($"The sort field '{column.Name}' does not exist.");
                    }
                    else
                    {
                        queryBuilder.OrderBy.Add(new Sql.SqlOrderColumn()
                        {
                            ColumnName = column.Name,
                            SortDirection = column.SortDescending ? Sql.DbSortDirection.Descending : Sql.DbSortDirection.Ascending
                        });
                    }
                }
            }
        }

        public static void ApplyLimit(IListQuery listQuery, IDbQueryBuilder queryBuilder, bool getTotal = false)
        {
            if (listQuery.QueryDefinition.Offset > 0)
            {
                queryBuilder.PagingOptions = new Csg.Data.Sql.SqlPagingOptions()
                {
                    Limit = listQuery.QueryDefinition.Limit,
                    Offset = listQuery.QueryDefinition.Offset
                };
            }                
        }

        public static IDbQueryBuilder Build(this IListQuery listQuery, bool ignoreLimit = false)
        {
            var query = listQuery.QueryBuilder.Fork();

            ApplySelections(listQuery, query);
            ApplyFilters(listQuery, query);
            ApplySort(listQuery, query);

            if (!ignoreLimit)
            {
                ApplyLimit(listQuery, query);
            }

            return query;
        }

        //TODO: Move this into Csg.Data.Dapper
        public static Dapper.CommandDefinition CreateDapperCommand(this Sql.SqlStatement statement, System.Data.IDbTransaction transaction = null, int? commandTimeout = null, Dapper.CommandFlags commandFlags = Dapper.CommandFlags.Buffered)
        {
            var parameters = new Dapper.DynamicParameters();
            var cmd = new Dapper.CommandDefinition(statement.CommandText,
                commandType: System.Data.CommandType.Text,
                parameters: parameters,
                transaction: transaction,
                commandTimeout: commandTimeout,
                flags: commandFlags
            );

            foreach (var param in statement.Parameters)
            {
                parameters.Add(param.ParameterName, param.Value, param.DbType, System.Data.ParameterDirection.Input, param.Size > 0 ? (int?)param.Size : null);
            }

            return cmd;
        }

        public static IDbQueryBuilder GetCountQuery(IListQuery query)
        {
            var countQuery = query.Build();

            countQuery.PagingOptions = null;
            countQuery.SelectColumns.Clear();
            countQuery.SelectColumns.Add(new Sql.SqlRawColumn("COUNT(1)"));

            return countQuery;           
        }

        public async static System.Threading.Tasks.Task<ListQueryResult<T>> GetResultAsync<T>(this Csg.Data.ListQuery.IListQuery query)
        {
            int? totalCount = null;
            IEnumerable<T> data = null;

            if (query.QueryDefinition.GetTotal)
            {
                var countQuery = GetCountQuery(query);
                var cmd = new DbQueryBuilder[] { (DbQueryBuilder)countQuery, (DbQueryBuilder)query.Build() }.RenderBatch()
                    .CreateDapperCommand(query.QueryBuilder.Transaction, query.QueryBuilder.CommandTimeout);

                var batchReader = await Dapper.SqlMapper.QueryMultipleAsync(query.QueryBuilder.Connection, cmd);
                totalCount = await batchReader.ReadFirstAsync<int>();
                data = await batchReader.ReadAsync<T>();
            }
            else
            {
                data = await query.Build().QueryAsync<T>();
            }

            return new ListQueryResult<T>()
            {
                Data = data,
                TotalCount = totalCount
            };
        }
    }

}
