using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Utilities
{
    public interface IHttpHelper
    {
        T CallAPI<T, U>(string url, string method, U data = null) where U : class;
    }
}
