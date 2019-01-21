using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.REST.Utilities
{
    public interface IUrlBuilder<T>
    {
        string BuildIdQuery<U>(string baseUrl, U id);
        string BuildFilterClause(Expression<Func<T, bool>> query);
        string BuildTop(int top);
        string BuildSkip(int skip);
        string BuildSelect();
        string BuildExpand();
    }
}
