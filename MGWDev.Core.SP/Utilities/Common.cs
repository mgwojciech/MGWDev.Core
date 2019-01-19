using MGWDev.Core.SP.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Utilities
{
    public class Common
    {
        public static SecureString ToSecureString(string value)
        {
            SecureString result = new SecureString();
            foreach(char c in value)
            {
                result.AppendChar(c);
            }

            return result;
        }
        public static string GetViewFields<T>()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<ViewFields>");
            var mappingProperties = typeof(T).GetProperties().Where(mp => mp.GetSetMethod() != null);
            foreach (var mappedProperty in mappingProperties)
            {
                MappingAttribute mappingAttribute = mappedProperty.GetCustomAttributes(true).FirstOrDefault(attr => attr is MappingAttribute) as MappingAttribute;
                if (mappingAttribute != null)
                {
                    builder.Append(String.Format("<FieldRef Name=\"{0}\" />", mappingAttribute.ColumnName));
                }
            }
            builder.Append("</ViewFields>");
            return builder.ToString();
        }
    }
}
