using MGWDev.Core.Model;
using MGWDev.Core.SP.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.Mocks
{
    [ListMapping("TestList")]
    public class MockTestSPEntity : IEntityWithIdAndTitle
    {
        [Mapping("ID", "Counter")]
        public int Id { get; set; }
        [Mapping("Title","Text")]
        public string Title { get; set; }
        [LookupMapping("TestLookup", typeof(MockSPEntity))]
        public MockSPEntity LookupTarget { get; set; }
        [Mapping("Created", "DateTime")]
        public DateTime CreatedDate { get; set; }
        [LookupMapping("Author", typeof(MockSPSPLookupTarget))]
        public MockSPSPLookupTarget Author { get; set; }
    }
}
