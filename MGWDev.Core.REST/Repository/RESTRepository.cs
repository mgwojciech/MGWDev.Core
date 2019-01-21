using MGWDev.Core.Model;
using MGWDev.Core.Repositories;
using MGWDev.Core.REST.Model;
using MGWDev.Core.REST.Utilities;
using MGWDev.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.REST.Repository
{
    public class RESTRepository<T, U> : IEntityRepository<T, U> where T : class, IEntityWithId<U>
    {
        public IUrlBuilder<T> UrlBuilder { get; set; } = new RESTUrlBuilder<T>();
        IHttpHelper HttpHelper { get; set; }
        public string Url { get; set; }

        public RESTRepository(string url, IHttpHelper httpHelper)
        {
            Url = url;
            HttpHelper = httpHelper;
        }
        public void Add(T entity)
        {
            HttpHelper.CallAPI<object,T>(Url, "POST", entity);
        }

        public void Commit()
        {
        }

        public void Delete(T entity)
        {
            string url = UrlBuilder.BuildIdQuery(Url, entity.Id);
            HttpHelper.CallAPI<object, T>(url, "DELETE", entity);
        }

        public T GetById(U Id)
        {
            string url = UrlBuilder.BuildIdQuery(Url, Id);
            return HttpHelper.CallAPI<T, T>(url, "GET");
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> query, int top = 100, int skip = 0)
        {
            string url = BuildGetUrl(query, top, skip);
            return HttpHelper.CallAPI<RESTResponse<T>, T>(url, "GET").value;
        }

        protected virtual string BuildGetUrl(Expression<Func<T, bool>> query, int top, int skip)
        {
            string url = String.Format("{0}?$filter={1}&$top={2}&$select={3}", Url, UrlBuilder.BuildFilterClause(query), UrlBuilder.BuildTop(top), UrlBuilder.BuildSelect());
            if (skip > 0)
                url += "&$skip=" + UrlBuilder.BuildSkip(skip);
            string expand = UrlBuilder.BuildExpand();
            if (!String.IsNullOrEmpty(expand))
                url += "&$expand=" + expand;
            return url;
        }

        public void Update(T entity)
        {
            string url = UrlBuilder.BuildIdQuery(Url, entity.Id);
            HttpHelper.CallAPI<T, T>(url, "PATCH", entity);
        }
    }
}
