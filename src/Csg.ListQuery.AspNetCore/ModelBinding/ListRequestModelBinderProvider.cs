using Csg.ListQuery.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Csg.ListQuery.AspNetCore.ModelBinding
{
    public class ListRequestModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // does the model type implement IListRequest?
            if (typeof(IListRequest).IsAssignableFrom(context.Metadata.ModelType))
            {
                return new ListRequestQueryStringModelBinder();
            }

            return null;
        }
    }

}

