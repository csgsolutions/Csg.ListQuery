using System;
using System.Linq;
using System.Text;
using Csg.ListQuery.Abstractions;
using Csg.ListQuery.AspNetCore.Abstractions;
using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore
{
    public static class ListRequestExtensions
    {
        /// <summary>
        /// Transforms a list request into a repository query
        /// </summary>
        /// <typeparam name="TValidationModel">A type that defines the selections, filters, and sortable fields allowed.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Csg.ListQuery.AspNetCore.ListRequestValidationResult Validate<TValidationModel>(this IListRequestModel request)
        {
            var properties = PropertyHelper.GetProperties(typeof(TValidationModel));
            return request.Validate(properties, properties, properties);
        }

        /// <summary>
        /// Transforms a list request into a repository query
        /// </summary>
        /// <typeparam name="TFieldValidationModel">A type that defines the selections and sortable fields allowed.</typeparam>
        /// <typeparam name="TFilterValidationModel">A type that defines the filters allowed.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Csg.ListQuery.AspNetCore.ListRequestValidationResult Validate<TFieldValidationModel, TFilterValidationModel>(this IListRequestModel request)
        {
            var fieldProperties = PropertyHelper.GetProperties(typeof(TFieldValidationModel));
            var filterPropreties = PropertyHelper.GetProperties(typeof(TFilterValidationModel));
            return request.Validate(fieldProperties, filterPropreties, fieldProperties);
        }

        /// <summary>
        /// Transforms a list request into a repository query
        /// </summary>
        /// <typeparam name="TSelectableProperties">A type that defines the selections and sortable fields allowed.</typeparam>
        /// <typeparam name="TFilterableProperties">A type that defines the filters allowed.</typeparam>
        /// <typeparam name="TSortableProperties">A type that defines the filters allowed.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Csg.ListQuery.AspNetCore.ListRequestValidationResult Validate<TSelectableProperties, TFilterableProperties, TSortableProperties>(this IListRequestModel request)
        {
            var selectProperties = PropertyHelper.GetProperties(typeof(TSelectableProperties));
            var filterProperties = PropertyHelper.GetProperties(typeof(TFilterableProperties));
            var orderProperties = PropertyHelper.GetProperties(typeof(TSortableProperties));
            return request.Validate(selectProperties, filterProperties, orderProperties);
        }

        /// <summary>
        /// Adds a validation error message for the given field name.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="fieldName"></param>
        /// <param name="errorMessage"></param>
        public static void Add(this ICollection<ListRequestValidationError> errors, string fieldName, string errorMessage)
        {
            errors.Add(new ListRequestValidationError()
            {
                Field = fieldName,
                Error = errorMessage
            });
        }
    }
}
