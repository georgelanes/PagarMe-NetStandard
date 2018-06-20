using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PagarMe.Test
{
    [TestClass]
    public class TransactionTestsV3 : PagarMeTestFixtureV3
    {


        [TestMethod]
        public void CreateV3CardTransaction()
        {
            var transaction = CreateTestV3CardTransaction();

            transaction.Save();

            Assert.IsTrue(transaction.Status == TransactionStatus.Paid);
        }


        [TestMethod]
        public void CreateV3BoletoTransaction()
        {
            var transaction = CreateTestV3BoletoTransaction();

            transaction.Save();

            Assert.IsTrue(transaction.Status == TransactionStatus.WaitingPayment);
        }
    }
}
