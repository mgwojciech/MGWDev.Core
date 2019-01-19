using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.DataAccess
{
    public class ListItemCollectionEnumerable : IEnumerable<ListItemCollection>
    {
        IEnumerator<ListItemCollection> enumerator;
        public ListItemCollectionEnumerable(List targetList, string whereSection, int rowLimit = 100)
        {
            enumerator = new ListItemCollectionEnumerator(targetList, whereSection, rowLimit);
        }
        public IEnumerator<ListItemCollection> GetEnumerator()
        {
            return enumerator;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return enumerator;
        }
    }
}
