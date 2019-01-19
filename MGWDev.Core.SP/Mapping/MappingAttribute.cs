using MGWDev.Core.SP.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Mapping
{
    /// <summary>
    /// Maps the entity property to sharepoint field
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingAttribute : Attribute
    {
        /// <summary>
        /// Internal name of the colum
        /// </summary>
        public string ColumnName { get; protected set; }
        /// <summary>
        /// Mapping strategy
        /// </summary>
        public IColumnMapper Mapper { get; protected set; }
        /// <summary>
        /// Field type as text. Used to construct correct caml queries
        /// </summary>
        public string TypeAsText { get; set; }
        public MappingAttribute(string columnName):
            this(columnName, "Text")
        {

        }
        public MappingAttribute(string columnName, string typeAsText):
            this(columnName, typeAsText, typeof(GenericColumnMapper))
        {
        }
        public MappingAttribute(string columnName, string typeAsText, Type mapperType)
        {
            ColumnName = columnName;
            TypeAsText = typeAsText;
            Mapper = Activator.CreateInstance(mapperType) as IColumnMapper;
        }
        /// <summary>
        /// Gets the mapping attribute for provided object
        /// </summary>
        /// <param name="obj">Property with mapping attribute</param>
        /// <returns></returns>
        public static MappingAttribute GetMappingAttribute(ICustomAttributeProvider obj)
        {
            MappingAttribute result = obj.GetCustomAttributes(true).FirstOrDefault(attr => attr is MappingAttribute) as MappingAttribute;
            if (result != null)
                return result;
            throw new PropertyNotMappedException(obj);
        }
    }
}
