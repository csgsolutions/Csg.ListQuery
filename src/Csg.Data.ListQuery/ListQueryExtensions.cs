using System;
using System.Collections.Generic;
using System.Reflection;
using Csg.Data.ListQuery.Abstractions;

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
                listQuery.Validations.Add(property);
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

        public static void ApplyFilters(IListQuery listQuery, IDbQueryBuilder queryBuilder)
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

        public static IDbQueryBuilder Build(this IListQuery listQuery)
        {
            var query = listQuery.QueryBuilder.Fork();

            ApplySelections(listQuery, query);
            ApplyFilters(listQuery, query);
            ApplySort(listQuery, query);

            return query;
        }

        public async static System.Threading.Tasks.Task<ListQueryResult<T>> GetResultAsync<T>(this Csg.Data.ListQuery.IListQuery query)
        {
            var data = await query.Build().QueryAsync<T>();

            return new ListQueryResult<T>()
            {
                Data = data
            };
        }
    }

}
