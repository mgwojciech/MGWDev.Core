# MGWDev.Core

This project helps You with ListItem to object mapping and provides You repository layer for entities.

To start with it create Your model

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
ListMapping attriute tells the repository what is the title of SharePoint list You want to map entities from.
Mapping attribute tells the repository what You are mapping (using field internal name). Second parameter is typeAsString. 
This will properly map type in queries.

Then You can use SPRepository in following way:

    using (ClientContext context = new ClientContext("https://domain.sharepoint.com"))
    {
        //set credentials if needed.
        IEntityRepository<MockTestSPEntity> repo = new SPClientRepository<MockTestSPEntity>(context);
        List<MockTestSPEntity> result = repo.Query(test => test.Id > 0).ToList();
    }

Check out test project for more samples.
