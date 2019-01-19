using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Mapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ListMappingAttribute : Attribute
    {
        public string ListTitle { get; set; }
        public ListMappingAttribute(string listTitle)
        {
            ListTitle = listTitle;
        }
    }
}
