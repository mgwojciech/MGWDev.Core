using MGWDev.Core.Exceptions;
using MGWDev.Core.SP.Utilities;
using MGWDev.Core.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.SP.Utilities
{
    [TestClass]
    public class ExpressionToCamlMapperTests
    {
        [TestMethod]
        public void ExpressionToCamlMapper_Test_SimpleWhere()
        {
            string expected = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">Test</Value></Eq></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.Title == "Test");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpressionToCamlMapper_Test_SimpleWhere_ComplexObject()
        {
            MockSPEntity test = new MockSPEntity()
            {
                Title = "Test"
            };
            string expected = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">Test</Value></Eq></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.Title == test.Title);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExpressionToCamlMapper_Test_SimpleWhere_Date()
        {
            DateTime date = new DateTime(2019, 06, 01, 12, 00, 00);
            string expected = "<Where><Eq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\">2019-06-01T12:00:00</Value></Eq></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.CreatedDate == date);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpressionToCamlMapper_Test_SimpleWhere_ComplexObject_Date()
        {
            MockSPEntity test = new MockSPEntity()
            {
                Title = "Test"
            };
            DateTime date = new DateTime(2019, 06, 01, 12, 00, 00);
            string expected = "<Where><Eq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\">2019-06-01T12:00:00</Value></Eq></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.CreatedDate == date);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpressionToCamlMapper_Test_LookupTitle()
        {
            string expected = "<Where><Eq><FieldRef Name=\"TestLookup\" /><Value Type=\"Lookup\">Test</Value></Eq></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.TestLookup.Title == "Test");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpressionToCamlMapper_Test_LookupId()
        {
            string expected = "<Where><Eq><FieldRef Name=\"TestLookup\" LookupId=\"TRUE\" /><Value Type=\"Lookup\">1</Value></Eq></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.TestLookup.Id == 1);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpressionToCamlMapper_Test_And()
        {
            string expected = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">Test</Value></Eq><Leq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\"><Today /></Value></Leq></And></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.Title == "Test" && me.CreatedDate <= DateTime.Now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpressionToCamlMapper_Test_DateTime()
        {
            string expected = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">Test</Value></Eq><Leq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\">2019-01-15T00:00:00</Value></Leq></And></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.Title == "Test" && me.CreatedDate <= new DateTime(2019,01,15));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpressionToCamlMapper_Test_FromComplexObject()
        {
            MockSPEntity test = new MockSPEntity(){
                CreatedDate = new DateTime(2019,01,15) 
            };
            string expected = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">Test</Value></Eq><Leq><FieldRef Name=\"Created\" /><Value Type=\"DateTime\">2019-01-15T00:00:00</Value></Leq></And></Where>";
            string actual = ExpressionToCamlMapper<MockSPEntity>.MapExpressionToCaml<MockSPEntity>(me => me.Title == "Test" && me.CreatedDate <= test.CreatedDate);

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [ExpectedException(typeof(PropertyNotMappedException))]
        public void ExpressionToCamlMapper_Test_SimpleWhere_NotMappedException()
        {
            string expected = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">Test12312312</Value></Eq></Where>";
            string actual = ExpressionToCamlMapper<MockEntity>.MapExpressionToCaml<MockEntity>(me => me.Title == "Test");

            Assert.AreEqual(expected, actual);
        }
    }
}
