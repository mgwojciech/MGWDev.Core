using MGWDev.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Repositories
{
    public class JSONFileRepository<T, U> : InMemoryRepository<T, U> where T : class
    {
        string Path { get; set; }
        public JSONFileRepository(string filePath, Func<U, Func<T, bool>> identityQuery) :
            base(JsonConvert.DeserializeObject<List<T>>(FileHelper.ReadFile(filePath)), identityQuery)
        {
            Path = filePath;

        }
        public override void Commit()
        {
            FileHelper.OverwriteFile(Path, JsonConvert.SerializeObject(Entities));
        }
    }
}
