using MGWDev.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Mapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LookupMappingAttribute : MappingAttribute
    {
        public LookupMappingAttribute(string columnName, Type mappingType) : base(columnName, "Lookup")
        {
            Type lookupMapperGenericType = typeof(LookupColumnMapper<>);
            Type mapper = lookupMapperGenericType.MakeGenericType(mappingType);
            ConstructorInfo ci = mapper.GetConstructor(new Type[] { });

            Mapper = (IColumnMapper)ci.Invoke(new object[] { });
        }
    }
}
