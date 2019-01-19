using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Utilities
{
    public class DataRecordMapper
    {
        public static T MapRecord<T>(IDataRecord record) where T : class , new()
        {
            T result = new T();
            var properties = result.GetType().GetProperties().Where(mp => mp.GetSetMethod() != null);
            foreach (var property in properties)
            {
                    try
                    {
                        property.SetValue(result, record[property.Name]);
                    }
                    catch
                    {
                        //TODO: handle exception
                    }
                }
            return result;
        }
    }
}
