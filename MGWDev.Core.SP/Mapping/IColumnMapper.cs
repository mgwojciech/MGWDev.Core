using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Mapping
{
    /// <summary>
    /// Astraction of mapping field value to property
    /// </summary>
    public interface IColumnMapper
    {
        object MapColumn(string columnName, ListItem item);
    }
}
