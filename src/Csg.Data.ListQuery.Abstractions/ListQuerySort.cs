using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.Data.ListQuery.Abstractions
{
    public class ListQuerySort
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the column is sorted descending.
        /// </summary>
        public bool SortDescending { get; set; }        
    }
}
