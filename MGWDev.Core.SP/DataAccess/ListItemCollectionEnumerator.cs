using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.DataAccess
{
    public class ListItemCollectionEnumerator : IEnumerator<ListItemCollection>
    {
        public int RowLimit { get; private set; }
        public List TargetList { get; private set; }
        public string WhereSection { get; private set; }
        public ListItemCollection Current { get; private set; }
        protected ListItemCollectionPosition Position { get; private set; }
        private bool endReached;
        public ListItemCollectionEnumerator(List targetList, string whereSection, int rowLimit)
        {
            TargetList = targetList;
            WhereSection = whereSection;
            RowLimit = rowLimit;
            endReached = false;
        }
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public void Dispose()
        {
        }
        public bool MoveNext()
        {
            if (endReached)
                return false;
            bool result = true;
            GetNextCollection();
            if (Current.Count == 0)
                result = false;
            return result;
        }
        protected void GetNextCollection()
        {
            CamlQuery query = PrepareQueryForCurrent();
            Current = TargetList.GetItems(query);
            TargetList.Context.Load(Current);
            TargetList.Context.ExecuteQuery();
            Position = Current.ListItemCollectionPosition;
            if (Position == null)
                endReached = true;
        }
        private CamlQuery PrepareQueryForCurrent()
        {
            CamlQuery query = new CamlQuery();
            query.ViewXml = String.Format("<View><Query>{0}</Query><RowLimit>{1}</RowLimit></View>", WhereSection, RowLimit);
            if (Position != null)
                query.ListItemCollectionPosition = Position;
            return query;
        }
        public void Reset()
        {
            endReached = false;
            Current = null;
            Position = null;
        }
    }
}
