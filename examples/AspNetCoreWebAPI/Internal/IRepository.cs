using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csg.ListQuery;

namespace AspNetCoreWebAPI.Internal
{
    public interface IRepository
    {
        Task<ListQueryResult<WeatherForecast>> GetWidgetsAsync(ListQueryDefinition queryDef);
    }
}
