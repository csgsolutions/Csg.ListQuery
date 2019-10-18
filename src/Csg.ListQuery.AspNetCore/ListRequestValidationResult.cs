using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public class ListRequestValidationResult
    {
        public ListRequestValidationResult(ICollection<ListRequestValidationError> errors, Csg.ListQuery.Abstractions.ListQueryDefinition query)
        {
            this.Errors = errors;
            this.ListQuery = query;
        }

        public virtual ICollection<ListRequestValidationError> Errors { get; set; }

        public virtual bool IsValid { get => this.Errors.Count <= 0; }

        public virtual Csg.ListQuery.Abstractions.ListQueryDefinition ListQuery { get; }
    }
}
