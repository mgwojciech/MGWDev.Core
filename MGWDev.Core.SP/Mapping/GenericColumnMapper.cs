using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace MGWDev.Core.SP.Mapping
{
    public class GenericColumnMapper : IColumnMapper
    {
        public object MapColumn(string columnName, ListItem item)
        {
            return item[columnName];
        }
    }
}
