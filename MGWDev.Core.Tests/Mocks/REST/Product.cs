using MGWDev.Core.Mapping;
using MGWDev.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.Mocks.REST
{
    public class Product : IEntityWithId
    {
        [BasicMappingAttribute("ID")]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
