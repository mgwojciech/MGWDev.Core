using MGWDev.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.Mocks
{
    public class MockEntity : IEntityWithId
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
