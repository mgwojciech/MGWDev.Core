using MGWDev.Core.Repositories;
using MGWDev.Core.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.Core.Repo
{
    [TestClass]
    public class InMemoryRepositoryTests
    {
        [TestMethod]
        public void InMemoryRepository_Test_OrderDefaultBy()
        {
            InMemoryRepository<MockEntity, int> repo = new InMemoryRepository<MockEntity, int>(new List<MockEntity>()
            {
                new MockEntity()
                {
                    Id = 1,
                    Title = "Item 1"
                },
                new MockEntity()
                {
                    Id = 2,
                    Title = "Item 2"
                }
            }, id => me => me.Id == id);

            var result = repo.Query(me => me.Id > 0);

            Assert.AreEqual(2, result.LastOrDefault().Id);
        }
        [TestMethod]
        public void InMemoryRepository_Test_OrderBy()
        {
            IEntityRepository<MockEntity, int> repo = new InMemoryRepository<MockEntity, int>(new List<MockEntity>()
            {
                new MockEntity()
                {
                    Id = 1,
                    Title = "Item 1"
                },
                new MockEntity()
                {
                    Id = 2,
                    Title = "Item 2"
                }
            }, id => me => me.Id == id);

            repo.OrderAscending = false;
            var result = repo.Query(me => me.Id > 0);

            Assert.AreEqual(2, result.FirstOrDefault().Id);

        }
        [TestMethod]
        public void InMemoryRepository_Test_OrderBy_Title()
        {
            IEntityRepository<MockEntity, int> repo = new InMemoryRepository<MockEntity, int>(new List<MockEntity>()
            {
                new MockEntity()
                {
                    Id = 1,
                    Title = "Item 1"
                },
                new MockEntity()
                {
                    Id = 2,
                    Title = "Item 2"
                }
            }, id => me => me.Id == id);

            repo.OrderAscending = false;
            repo.OrderByField = "Title";
            var result = repo.Query(me => me.Id > 0);

            Assert.AreEqual(2, result.FirstOrDefault().Id);

        }
    }
}
