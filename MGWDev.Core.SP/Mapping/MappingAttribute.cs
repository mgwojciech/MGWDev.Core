using MGWDev.Core.SP.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Mapping
{
    public class MappingAttribute : Attribute
    {
        public string ColumnName { get; protected set; }
        public IColumnMapper Mapper { get; protected set; }
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

        public static MappingAttribute GetMappingAttribute(ICustomAttributeProvider obj)
        {
            MappingAttribute result = obj.GetCustomAttributes(true).FirstOrDefault(attr => attr is MappingAttribute) as MappingAttribute;
            if (result != null)
                return result;
            throw new PropertyNotMappedException(obj);
        }
    }
}
