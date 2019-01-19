using MGWDev.Core.Model;
using MGWDev.Core.SP.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.Mocks
{
    [ListMapping("Test List")]
    public class MockSPEntity : IEntityWithIdAndTitle
    {
        [Mapping("Id", "Counter")]
        public int Id { get; set; }
        [Mapping("Title", "Text")]
        public string Title { get; set; }
        [Mapping("Created", "DateTime")]
        public DateTime CreatedDate { get; set; }
        [LookupMapping("TestLookup", typeof(MockSPSPLookupTarget))]
        public MockSPSPLookupTarget TestLookup { get; set; }
    }
}
