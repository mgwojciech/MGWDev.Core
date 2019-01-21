using MGWDev.Core.Mapping;
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
    public class MappingAttribute : BasicMappingAttribute
    {
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
        public MappingAttribute(string columnName, string typeAsText, Type mapperType):base(columnName)
        {
            ColumnName = columnName;
            TypeAsText = typeAsText;
            Mapper = Activator.CreateInstance(mapperType) as IColumnMapper;
        }
    }
}
