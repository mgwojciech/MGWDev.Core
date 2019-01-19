using System;
using MGWDev.Core.Receiver;
using MGWDev.Core.Repositories;
using MGWDev.Core.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MGWDev.Core.Tests.Core.Repo
{
    [TestClass]
    public class EventReceiverRepositoryTests
    {
        [TestMethod]
        public void EventReceiverRepository_Test_ItemAdding()
        {
            EventDrivenReceiver<MockEntity> receiver = new EventDrivenReceiver<MockEntity>(EventType.EntityAdding);
            receiver.Handler += OnItemAdding;
            InMemoryRepository<MockEntity, int> repo = new InMemoryRepository<MockEntity, int>(new System.Collections.Generic.List<MockEntity>(), id => (ent => ent.Id == id));
            EventReceiverRepository<MockEntity, int> eventRepo = new EventReceiverRepository<MockEntity, int>(repo);
            eventRepo.Receivers.Add(receiver);
            eventRepo.Add(new MockEntity()
            {
                Id = 1,
                Title = "Test"
            });
        }

        private void OnItemAdding(object sender, MockEntity e)
        {
            Assert.AreEqual(1, e.Id);
        }
    }
}
