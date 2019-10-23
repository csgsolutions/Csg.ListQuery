using System;
using System.Collections.Generic;
using System.Text;
using Csg.ListQuery.AspNetCore;
using Csg.ListQuery.AspNetCore.ModelBinding;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddListQuery(this IServiceCollection services)
        {
            services.TryAddSingleton<IListQueryValidator, DefaultListQueryValidator>();
            services.TryAddSingleton<ListRequestFactory>();

            return services;
        }
    }
}
