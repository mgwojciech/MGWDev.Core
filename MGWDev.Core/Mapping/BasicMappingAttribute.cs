using MGWDev.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Mapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BasicMappingAttribute : Attribute
    {
        /// <summary>
        /// Name of mapped column
        /// </summary>
        public string ColumnName { get; protected set; }
        public BasicMappingAttribute(string columnName)
        {
            ColumnName = columnName;
        }
        /// <summary>
        /// Gets the mapping attribute for provided object
        /// </summary>
        /// <param name="obj">Property with mapping attribute</param>
        /// <returns></returns>
        public static BasicMappingAttribute GetMappingAttribute(ICustomAttributeProvider obj)
        {
            BasicMappingAttribute result = obj.GetCustomAttributes(true).FirstOrDefault(attr => attr is BasicMappingAttribute) as BasicMappingAttribute;
            if (result != null)
                return result;
            throw new PropertyNotMappedException(obj);
        }
        public static string GetMappingColumnName(MemberInfo propInfo)
        {
            BasicMappingAttribute result = propInfo.GetCustomAttributes(true).FirstOrDefault(attr => attr is BasicMappingAttribute) as BasicMappingAttribute;
            if (result != null)
                return result.ColumnName;
            return propInfo.Name;
        }
    }
}
