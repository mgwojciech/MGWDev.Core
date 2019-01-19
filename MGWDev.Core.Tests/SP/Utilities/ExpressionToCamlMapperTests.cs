using MGWDev.Core.SP.Exceptions;
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
        [ExpectedException(typeof(PropertyNotMappedException))]
        public void ExpressionToCamlMapper_Test_SimpleWhere_NotMappedException()
        {
            string expected = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">Test12312312</Value></Eq></Where>";
            string actual = ExpressionToCamlMapper<MockEntity>.MapExpressionToCaml<MockEntity>(me => me.Title == "Test");

            Assert.AreEqual(expected, actual);
        }
    }
}
