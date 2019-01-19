using MGWDev.Core.SP.Mapping;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.DataAccess
{
    public class ListItemAccessor
    {
        public static List<T> GetListItems<T>(List list, string query, int rowLimit = 100) where T : new()
        {
            List<T> result = new List<T>();
            EnumerateListItems<T>(list, query, entity => result.Add(entity), rowLimit);
            return result;
        }
        public static void EnumerateListItems<T>(List list, string query, Action<T> action, int rowLimit = 100) where T : new()
        {
            int top = 0;
            ListItemCollectionEnumerable liEnumerable = new ListItemCollectionEnumerable(list, query, 100);
            foreach (ListItemCollection collection in liEnumerable)
            {
                foreach (ListItem item in collection)
                {
                    action(ItemMappingHelper.MapFromItem<T>(item));
                    top++;
                    if (top >= rowLimit)
                        return;
                }
            }
        }
    }
}
