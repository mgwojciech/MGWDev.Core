using MGWDev.Core.REST.Utilities;
using MGWDev.Core.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.REST
{
    [TestClass]
    public class ExpressionToRESTMapperTests
    {
        [TestMethod]
        public void ExpressionToRESTMapper_Test_SimpleEquals_Text()
        {
            string expected = "Title eq 'Test'";
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockEntity>.MapExpressionToRESTQuery<MockEntity>(me => me.Title == "Test"));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_SimpleEquals_Int()
        {
            string expected = "Id eq 1";
            ExpressionToRESTMapper<MockEntity> mapper = new ExpressionToRESTMapper<MockEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockEntity>.MapExpressionToRESTQuery<MockEntity>(me => me.Id == 1));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_SimpleLess_Int()
        {
            string expected = "Id lt 1";
            ExpressionToRESTMapper<MockEntity> mapper = new ExpressionToRESTMapper<MockEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockEntity>.MapExpressionToRESTQuery<MockEntity>(me => me.Id < 1));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_SimpleLessOrEqual_Int()
        {
            string expected = "Id le 1";
            ExpressionToRESTMapper<MockEntity> mapper = new ExpressionToRESTMapper<MockEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockEntity>.MapExpressionToRESTQuery<MockEntity>(me => me.Id <= 1));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_SimpleGreater_Int()
        {
            string expected = "Id gt 1";
            ExpressionToRESTMapper<MockEntity> mapper = new ExpressionToRESTMapper<MockEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockEntity>.MapExpressionToRESTQuery<MockEntity>(me => me.Id > 1));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_SimpleGreaterOrEqual_Int()
        {
            string expected = "Id ge 1";
            ExpressionToRESTMapper<MockEntity> mapper = new ExpressionToRESTMapper<MockEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockEntity>.MapExpressionToRESTQuery<MockEntity>(me => me.Id >= 1));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_AndEquals()
        {
            string expected = "Id eq 1 and Title eq 'Test'";
            ExpressionToRESTMapper<MockEntity> mapper = new ExpressionToRESTMapper<MockEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockEntity>.MapExpressionToRESTQuery<MockEntity>(me => me.Id == 1 && me.Title == "Test"));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_OrEquals()
        {
            string expected = "Id eq 1 or Title eq 'Test'";
            ExpressionToRESTMapper<MockEntity> mapper = new ExpressionToRESTMapper<MockEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockEntity>.MapExpressionToRESTQuery<MockEntity>(me => me.Id == 1 || me.Title == "Test"));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_Complex_Int()
        {
            string expected = "TestLookup/Id eq 1";
            ExpressionToRESTMapper<MockSPEntity> mapper = new ExpressionToRESTMapper<MockSPEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockSPEntity>.MapExpressionToRESTQuery<MockSPEntity>(me => me.TestLookup.Id == 1));
        }
        [TestMethod]
        public void ExpressionToRESTMapper_Test_Complex_Title()
        {
            string expected = "TestLookup/Title eq 'Test'";
            ExpressionToRESTMapper<MockSPEntity> mapper = new ExpressionToRESTMapper<MockSPEntity>();
            Assert.AreEqual(expected, ExpressionToRESTMapper<MockSPEntity>.MapExpressionToRESTQuery<MockSPEntity>(me => me.TestLookup.Title == "Test"));
        }
    }
}
