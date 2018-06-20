using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PagarMe.Test
{
    [TestClass]
    public class TransactionEventTests : PagarMeTestFixture
    {
        [TestMethod]
        public void HasEvents()
        {
            var transaction = CreateTestTransaction();
            transaction.Save();
            var events = transaction.Events.FindAll(new Event());
            Assert.AreEqual(1, events.Count());
        }

        [TestMethod]
        public void ThrowsExceptionIfIdNull()
        {
            try
            {
                var transaction = CreateTestTransaction();
                var events = transaction.Events;
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void HasProperties()
        {
            var transaction = CreateTestTransaction();
            transaction.Save();

            var transactionEvent = transaction.Events.FindAll(new Event()).First();
            Assert.IsTrue(transactionEvent.DateCreated.HasValue);
            Assert.IsTrue(transactionEvent.Model.Length > 0);
            Assert.IsTrue(transactionEvent.ModelId.Length > 0);
            Assert.IsTrue(transactionEvent.Id.Length > 0);
            Assert.IsTrue(transactionEvent.Name.Length > 0);
            Assert.IsTrue(transactionEvent.Payload["current_status"].ToString().Length > 0);
            Assert.IsTrue(transactionEvent.Payload["old_status"].ToString().Length > 0);
            Assert.IsTrue(transactionEvent.Payload["desired_status"].ToString().Length > 0);
        }

        [TestMethod]
        public void RefuseReasonAcquirer()
        {
            var transaction = new Transaction();
            transaction.Amount = 10000;
            CardHash card = new CardHash()
            {
                CardNumber = "4242424242424242",
                CardHolderName = "Aardvark Silva",
                CardExpirationDate = "0140",
                CardCvv = "614"
            };
            transaction.CardHash = card.Generate();
            transaction.Customer = new Customer()
            {
                Name = "John Appleseed",
                Email = "jhon@pagar.me",
                DocumentNumber = "11111111111"
            };
            transaction.Save();
            Assert.AreEqual(transaction.RefuseReason, "acquirer");
        }

        [TestMethod]
        public void RefuseReasonNull()
        {
            var transaction = new Transaction();
            transaction.Amount = 10000;
            CardHash card = new CardHash()
            {
                CardNumber = "4242424242424242",
                CardHolderName = "Aardvark Silva",
                CardExpirationDate = "0140",
                CardCvv = "123"
            };
            transaction.CardHash = card.Generate();
            transaction.Customer = new Customer()
            {
                Name = "John Appleseed",
                Email = "jhon@pagar.me",
                DocumentNumber = "11111111111"
            };
            transaction.Save();
            Assert.IsNull(transaction.RefuseReason);
        }
    }
}
