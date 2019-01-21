using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.REST.Model
{
    public class RESTResponse<T>
    {
        public List<T> value { get; set; }
    }
}
