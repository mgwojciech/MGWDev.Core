using MGWDev.Core.REST.Utilities;
using MGWDev.Core.Tests.Mocks;
using MGWDev.Core.Tests.Mocks.REST;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.REST
{
    [TestClass]
    public class RESTUrlBuilderTests
    {
        [TestMethod]
        public void RESTUrlBuilder_Test_GetSelect_Simple()
        {
            string expected = "Id,Title";
            RESTUrlBuilder<MockEntity> builder = new RESTUrlBuilder<MockEntity>();
            string actual = builder.BuildSelect();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RESTUrlBuilder_Test_GetSelect_Complex()
        {
            string expected = "Id,Title,Created,TestLookup/Id,TestLookup/Title";
            RESTUrlBuilder<MockSPEntity> builder = new RESTUrlBuilder<MockSPEntity>();
            string actual = builder.BuildSelect();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RESTUrlBuilder_Test_GetExpand_Simple()
        {
            string expected = "";
            RESTUrlBuilder<MockEntity> builder = new RESTUrlBuilder<MockEntity>();
            string actual = builder.BuildExpand();

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RESTUrlBuilder_Test_GetExpand_Complex()
        {
            string expected = "Categories";
            RESTUrlBuilder<Product> builder = new RESTUrlBuilder<Product>();
            string actual = builder.BuildExpand();

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RESTUrlBuilder_Test_GetSelect_Complex_WithList()
        {
            string expected = "ID,Name,Categories/ID,Categories/Name,Description,ReleaseDate";
            RESTUrlBuilder<Product> builder = new RESTUrlBuilder<Product>();
            string actual = builder.BuildSelect();

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RESTUrlBuilder_Test_GetId_Int()
        {
            string expected = "http://mockurl/odata/MockSPEntities(1)";
            RESTUrlBuilder<MockEntity> builder = new RESTUrlBuilder<MockEntity>();
            string actual = builder.BuildIdQuery("http://mockurl/odata/MockSPEntities", 1);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RESTUrlBuilder_Test_GetFilter_SimpleEquals_Text()
        {
            string expected = "Title eq 'Test'";
            RESTUrlBuilder<MockEntity> builder = new RESTUrlBuilder<MockEntity>();
            string actual = builder.BuildFilterClause(m=>m.Title == "Test");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RESTUrlBuilder_Test_GetFilter_SimpleEquals_Int()
        {
            string expected = "Id eq 1";
            RESTUrlBuilder<MockEntity> builder = new RESTUrlBuilder<MockEntity>();
            string actual = builder.BuildFilterClause(m => m.Id == 1);
            Assert.AreEqual(expected, actual);
        }
    }
}
