using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Exceptions
{
    public class PropertyNotMappedException : Exception
    {
        public PropertyNotMappedException(MemberInfo memberInfo):base(String.Format("Member {0} is not mapped in model", memberInfo.Name))
        {

        }
        public PropertyNotMappedException(ICustomAttributeProvider provider) : base(String.Format("Member {0} is not mapped in model", provider))
        {

        }
    }
}
