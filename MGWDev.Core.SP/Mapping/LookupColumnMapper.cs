using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGWDev.Core.Model;
using Microsoft.SharePoint.Client;

namespace MGWDev.Core.SP.Mapping
{
    public class LookupColumnMapper<T> : IColumnMapper where T : class, IEntityWithIdAndTitle, new()
    {
        public object MapColumn(string columnName, ListItem item)
        {
            FieldLookupValue lkpValue = item[columnName] as FieldLookupValue;
            T result = new T();
            result.Id = lkpValue.LookupId;
            result.Title = lkpValue.LookupValue;

            return result;
        }
    }
}
