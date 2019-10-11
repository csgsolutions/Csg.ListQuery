using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Abstractions
{
    public class ListQuerySort
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the column is sorted descending.
        /// </summary>
        public virtual bool SortDescending { get; set; }        
    }
}
