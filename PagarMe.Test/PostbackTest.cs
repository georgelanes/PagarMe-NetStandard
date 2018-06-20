using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagarMe.Enumeration;
using PagarMe.Model;

namespace PagarMe.Test
{
    [TestClass]
    public class PostbackTest
    {
        private static Transaction transaction = PagarMeTestFixture.BoletoTransactionPaidWithPostbackURL();

        [TestMethod]
        public void FindAllPostbacksTest()
        {
            var postbacks = transaction.Postbacks.FindAll(new Postback());

            foreach (var postback in postbacks)
            {
                Assert.IsTrue(postback.ModelId.Equals(transaction.Id));
            }
        }

        [TestMethod]
        public void FindPostbackTest()
        {
            Postback postback = transaction.Postbacks.FindAll(new Postback()).FirstOrDefault();
            Postback postbackReturned = transaction.Postbacks.Find(postback.Id);

            Assert.IsNotNull(postbackReturned.Id);
        }

        [TestMethod]
        public void RedeliverPostbackTest()
        {
            Postback postback = transaction.Postbacks.FindAll(new Postback()).FirstOrDefault();
            postback.Redeliver();

            Assert.IsTrue(postback.Status == PostbackStatus.PendingRetry);
        }
    }
}
