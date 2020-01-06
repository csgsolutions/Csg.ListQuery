using Csg.ListQuery.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    /// <summary>
    /// Represents a class that validates a list request and produces a <see cref="ListQueryDefinition"/>.
    /// </summary>
    public interface IListRequestValidator
    {
        /// <summary>
        /// Validates the given request and produces a <see cref="ListQueryDefinition"/> query.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="selectableProperties">A dictionary of the fields that are allowed to be selected.</param>
        /// <param name="filerableProperties">A dictionary of the fields that are allowed to be filtered.</param>
        /// <param name="sortableProperties">A dictionary of the fields that are allowed to be sorted.</param>
        /// <returns></returns>
        ListRequestValidationResult Validate(
            IListRequest request,
            IDictionary<string, ListItemPropertyInfo> selectableProperties,
            IDictionary<string, ListItemPropertyInfo> filerableProperties,
            IDictionary<string, ListItemPropertyInfo> sortableProperties
        );
    }
}
