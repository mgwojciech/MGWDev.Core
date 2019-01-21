using MGWDev.Core.REST.Repository;
using MGWDev.Core.Tests.Mocks.REST;
using MGWDev.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.REST
{
    [TestClass]
    public class RESTRepositoryTests
    {
        HttpClient client;
        string serviceUrl = "https://services.odata.org/V3/OData/OData.svc/Products";
        [TestInitialize]
        public void Initialize()
        {
            client = new HttpClient();
        }
        [TestMethod]
        public void RESTRepositoryTests_Test_SimpleGET()
        {
            RESTRepository<Product, int> repo = new RESTRepository<Product, int>(serviceUrl, new HttpClientHttpHelper(client));
            var result = repo.Query(p => p.Id >= 0, 10);

            Assert.AreEqual(10, result.Count());
        }
        [TestCleanup]
        public void CleanUp()
        {
            client.Dispose();
        }
    }
}
