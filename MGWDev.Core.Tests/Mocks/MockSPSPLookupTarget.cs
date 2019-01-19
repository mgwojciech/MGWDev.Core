using MGWDev.Core.Model;
using MGWDev.Core.SP.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.Mocks
{
    public class MockSPSPLookupTarget : IEntityWithIdAndTitle
    {
        [Mapping("Id","Counter")]
        public int Id { get; set; }
        [Mapping("Title")]
        public string Title { get; set; }
    }
}
