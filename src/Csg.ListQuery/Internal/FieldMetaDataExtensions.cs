using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Internal
{
    public static class FieldMetaDataExtensions
    {
        /// <summary>
        /// Gets the fully qualified path name of a field using the data
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        //public static string GetFullName(this ListFieldMetadata metaData, bool useDataMemberNames = false)
        //{
        //    var nameBuilder = new StringBuilder();
        //    var meta = metaData;
        //    bool first = true;

        //    while (meta != null)
        //    {
        //        if (!first)
        //        {
        //            nameBuilder.Insert(0, ".");
        //        }

        //        nameBuilder.Insert(0, useDataMemberNames ? meta.DataMemberName ?? meta.Name : meta.Name);
                
        //        first = false;
        //        meta = meta.Parent;
        //    }

        //    return nameBuilder.ToString();
        //}
    }
}
