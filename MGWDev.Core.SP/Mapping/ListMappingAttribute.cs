using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Mapping
{
    /// <summary>
    /// Is responsible for mapping the object to list with provided title.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ListMappingAttribute : Attribute
    {
        /// <summary>
        /// Title of list You want to map
        /// </summary>
        public string ListTitle { get; set; }
        public ListMappingAttribute(string listTitle)
        {
            ListTitle = listTitle;
        }
    }
}
