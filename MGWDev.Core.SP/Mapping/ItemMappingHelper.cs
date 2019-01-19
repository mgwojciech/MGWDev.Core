using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.SP.Mapping
{
    public class ItemMappingHelper
    {
        public static T MapFromItem<T>(ListItem item) where T : new()
        {
            T result = new T();
            var mappingProperties = result.GetType().GetProperties().Where(mp => mp.GetSetMethod() != null);
            foreach(var mappedProperty in mappingProperties)
            {
                MappingAttribute mappingAttribute = mappedProperty.GetCustomAttributes(true).FirstOrDefault(attr => attr is MappingAttribute) as MappingAttribute;
                if(mappingAttribute != null)
                {
                    try
                    {
                        object mappedValue = mappingAttribute.Mapper.MapColumn(mappingAttribute.ColumnName, item);
                        mappedProperty.SetValue(result, mappedValue);
                    }
                    catch
                    {
                        //TODO: handle exception
                    }
                }
            }
            return result;
        }

        public static void MapToItem<T>(T entity, ListItem item)
        {
            var mappingProperties = entity.GetType().GetProperties().Where(mp => mp.GetSetMethod() != null);
            foreach (var mappedProperty in mappingProperties)
            {
                MappingAttribute mappingAttribute = mappedProperty.GetCustomAttributes(true).FirstOrDefault(attr => attr is MappingAttribute) as MappingAttribute;
                if (mappingAttribute != null)
                {
                    try
                    {
                        object mappedValue = mappedProperty.GetValue(entity);
                        item[mappingAttribute.ColumnName] = mappedValue;
                    }
                    catch
                    {
                        //TODO: handle exception
                    }
                }
            }
        }

        public static int GetId<T>(T entity)
        {
            var properties = entity.GetType().GetProperties().Where(pi => pi.GetSetMethod() != null);
            foreach (var property in properties)
            {
                MappingAttribute attr = property.GetCustomAttributes(true).FirstOrDefault(at => at is MappingAttribute) as MappingAttribute;
                if (attr != null)
                {
                    try
                    {
                        if (attr.ColumnName.Equals("ID", StringComparison.InvariantCultureIgnoreCase))
                            return (int)property.GetValue(entity);
                    }
                    catch
                    {

                    }
                }
            }
            return 0;
        }
    }
}
