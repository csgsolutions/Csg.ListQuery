using Csg.ListQuery.Server;
using System;
using System.Collections.Generic;
using System.Text;
using Csg.ListQuery.Server;

namespace Csg.ListQuery.AspNetCore
{
    public interface IListQueryValidator
    {
        ListRequestValidationResult Validate(
            IListRequest request,
            IDictionary<string, ListItemPropertyInfo> selectableProperties,
            IDictionary<string, ListItemPropertyInfo> filerableProperties,
            IDictionary<string, ListItemPropertyInfo> sortableProperties
        );
    }
}
