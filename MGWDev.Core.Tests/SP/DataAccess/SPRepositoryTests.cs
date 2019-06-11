using MGWDev.Core.Repositories;
using MGWDev.Core.SP.Repositories;
using MGWDev.Core.SP.Utilities;
using MGWDev.Core.Tests.Mocks;
using MGWDev.Core.Utilities;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Tests.SP.DataAccess
{
    [TestClass]
    public class SPRepositoryTests
    {
        //[TestMethod]
        public void SPRepository_Test_GetAllData()
        {
            using (ClientContext context = new ClientContext(ConfigurationManager.AppSettings["SiteUrl"]))
            {
                SecureString password = Common.ToSecureString(ConfigurationManager.AppSettings["UserPassword"]);
                context.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings["UserLogin"], password);
                IEntityRepository<MockTestSPEntity> repo = new SPClientRepository<MockTestSPEntity>(context);

                DateTime date = DateTime.Now;

                List<MockTestSPEntity> result = repo.Query(test => test.ModifiedDate > date).ToList();

                Assert.AreEqual(10, result.Count);
            }
        }

        //[TestMethod]
        public void SPRepository_Test_Get_ByLookupId()
        {
            using (ClientContext context = new ClientContext(ConfigurationManager.AppSettings["SiteUrl"]))
            {
                SecureString password = Common.ToSecureString(ConfigurationManager.AppSettings["UserPassword"]);
                context.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings["UserLogin"], password);
                SPClientRepository<MockTestSPEntity> repo = new SPClientRepository<MockTestSPEntity>(context);

                List<MockTestSPEntity> result = repo.Query(test => test.LookupTarget.Id == 1).ToList();

                Assert.AreEqual(2, result.Count);
            }
        }

        //[TestMethod]
        public void SPRepository_Test_Get_ByLookupValue()
        {
            using (ClientContext context = new ClientContext(ConfigurationManager.AppSettings["SiteUrl"]))
            {
                SecureString password = Common.ToSecureString(ConfigurationManager.AppSettings["UserPassword"]);
                context.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings["UserLogin"], password);
                SPClientRepository<MockTestSPEntity> repo = new SPClientRepository<MockTestSPEntity>(context);

                List<MockTestSPEntity> result = repo.Query(test => test.LookupTarget.Title == "Lookup 2").ToList();

                Assert.AreEqual(1, result.Count);
            }
        }

        //[TestMethod]
        public void SPRepository_Test_EntityBuffer()
        {
            using (ClientContext context = new ClientContext(ConfigurationManager.AppSettings["SiteUrl"]))
            {
                SecureString password = Common.ToSecureString(ConfigurationManager.AppSettings["UserPassword"]);
                context.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings["UserLogin"], password);
                SPClientRepository<MockTestSPEntity> repo = new SPClientRepository<MockTestSPEntity>(context);
                EntityBuffer<MockTestSPEntity> buffer = new EntityBuffer<MockTestSPEntity>(repo, t=>t.Id > 0);
                int counter = 0;
                buffer.Enumerate(delegate (MockTestSPEntity entity)
                {
                    counter++;
                }, 3);

                Assert.AreEqual(10, counter);
            }
        }
    }
}
