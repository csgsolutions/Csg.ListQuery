using System;
using System.Linq;
using System.Text;
using Csg.ListQuery;
using System.Collections.Generic;
using Csg.ListQuery.Server;

namespace Csg.ListQuery.AspNetCore
{
    /// <summary>
    /// Provides extension methods for list validator.
    /// </summary>
    public static class ListQueryValidatorExtensions
    {
        /// <summary>
        /// Transforms a list request into a repository query
        /// </summary>
        /// <typeparam name="TValidationModel">A type that defines the selections, filters, and sortable fields allowed.</typeparam>
        /// <param name="validator"></param>
        /// <returns></returns>
        public static ListRequestValidationResult Validate<TValidationModel>(this IListRequestValidator validator, IListRequest request)
        {
            var properties = PropertyHelper.GetProperties(typeof(TValidationModel));

            return validator.Validate(request, properties, properties, properties);
        }

        /// <summary>
        /// Transforms a list request into a repository query
        /// </summary>
        /// <typeparam name="TFieldValidationModel">A type that defines the selections and sortable fields allowed.</typeparam>
        /// <typeparam name="TFilterValidationModel">A type that defines the filters allowed.</typeparam>
        /// <param name="validator"></param>
        /// <returns></returns>
        public static ListRequestValidationResult Validate<TFieldValidationModel, TFilterValidationModel>(this IListRequestValidator validator, IListRequest request)
        {
            var fieldProperties = PropertyHelper.GetProperties(typeof(TFieldValidationModel));
            var filterPropreties = PropertyHelper.GetProperties(typeof(TFilterValidationModel));
            return validator.Validate(request, fieldProperties, filterPropreties, fieldProperties);
        }

        /// <summary>
        /// Transforms a list request into a repository query
        /// </summary>
        /// <typeparam name="TSelectableProperties">A type that defines the selections and sortable fields allowed.</typeparam>
        /// <typeparam name="TFilterableProperties">A type that defines the filters allowed.</typeparam>
        /// <typeparam name="TSortableProperties">A type that defines the filters allowed.</typeparam>
        /// <param name="validator"></param>
        /// <returns></returns>
        public static ListRequestValidationResult Validate<TSelectableProperties, TFilterableProperties, TSortableProperties>(this IListRequestValidator validator, IListRequest request)
        {
            var selectProperties = PropertyHelper.GetProperties(typeof(TSelectableProperties));
            var filterProperties = PropertyHelper.GetProperties(typeof(TFilterableProperties));
            var orderProperties = PropertyHelper.GetProperties(typeof(TSortableProperties));
            return validator.Validate(request, selectProperties, filterProperties, orderProperties);
        }
    }
}
