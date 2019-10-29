using MGWDev.Core.Model;
using MGWDev.Core.Repositories;
using MGWDev.Core.SP.DataAccess;
using MGWDev.Core.SP.Mapping;
using MGWDev.Core.SP.Utilities;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Repositories
{
    public class SPClientRepository<T> : IEntityRepository<T> where T : class, IEntityWithId, new()
    {
        public ClientContext Context { get; private set; }
        public List List { get; private set; }
        public ExpressionToCamlMapper<T> CamlMapper { get; set; } = new ExpressionToCamlMapper<T>();
        protected ListItemCollectionPosition Position { get; private set; }
        private int PreviousSkip { get; set; }
        public string OrderByField { get; set; } = "ID";
        public bool OrderAscending { get; set; }
        public SPClientRepository(ClientContext context)
        {
            Context = context;
            List = context.Web.Lists.GetByTitle((typeof(T).GetCustomAttributes(true).FirstOrDefault(attr => attr is ListMappingAttribute) as ListMappingAttribute).ListTitle);
        }
        public SPClientRepository(ClientContext context, string listTitle)
        {
            Context = context;
            List = context.Web.Lists.GetByTitle(listTitle);
        }
        public SPClientRepository(ClientContext context, Guid listId)
        {
            Context = context;
            List = context.Web.Lists.GetById(listId);
        }

        public T GetById(int id)
        {
            ListItem item = List.GetItemById(id);
            Context.Load(item);
            Context.ExecuteQuery();

            return ItemMappingHelper.MapFromItem<T>(item);
        }
        public void Add(T entity)
        {
            ListItemCreationInformation ci = new ListItemCreationInformation();
            ListItem item = List.AddItem(ci);
            ItemMappingHelper.MapToItem(entity, item);

            item.Update();
            Context.Load(item);
            Context.ExecuteQuery();
        }

        public void Commit()
        {
            Context.ExecuteQuery();
        }

        public void Delete(T entity)
        {
            ListItem item = GetItem(entity);
            item.DeleteObject();
        }

        public virtual string ComposeQuery(string whereSection, int top)
        {
            return $"<View><Query>{whereSection}</Query><RowLimit>{top}</RowLimit>{Common.GetViewFields<T>()}<OrderBy><FieldRef Name=\"{OrderByField}\" Ascending=\"{OrderAscending.ToString().ToUpperInvariant()}\"/></OrderBy></View>";
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> query, int top = 100, int skip = 0)
        {
            List<T> results = new List<T>();

            string whereSection = CamlMapper.Translate(query.Body, query.Parameters.FirstOrDefault());

            CamlQuery caml = new CamlQuery();
            caml.ViewXml = ComposeQuery(whereSection, top);
            if (Position != null && skip != 0)
            {
                if (skip - PreviousSkip > top)
                    return results;
                if (PreviousSkip != skip - top)
                    throw new NotImplementedException("Unable to skip. Using SPClientRepository You can skip only one page!");

                PreviousSkip += top;
                caml.ListItemCollectionPosition = Position;
            }

            ListItemCollection collection = List.GetItems(caml);
            Context.Load(collection);
            Context.ExecuteQuery();
            foreach (ListItem item in collection)
            {
                results.Add(ItemMappingHelper.MapFromItem<T>(item));
            }
            Position = collection.ListItemCollectionPosition;
            return results;
        }

        public void Update(T entity)
        {
            ListItem item = GetItem(entity);
            ItemMappingHelper.MapToItem(entity, item);

            item.Update();
            Context.Load(item);
        }

        protected virtual ListItem GetItem(T entity)
        {
            int id = entity.Id;
            if (id == 0)
                throw new Exception("No mapping for column ID found!");

            ListItem result = List.GetItemById(id);
            Context.Load(result);
            Context.ExecuteQuery();

            return result;
        }
    }
}
