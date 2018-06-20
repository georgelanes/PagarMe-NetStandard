﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PagarMe.Test
{
    [TestClass]
    public class PayableTest
    {
        [TestMethod]
        public void FindPayableTest()
        {
            Transaction transaction = PagarMeTestFixture.CreateTestBoletoTransaction();
            transaction.Save();
            transaction.Status = TransactionStatus.Paid;
            transaction.Save();

            Payable payable = transaction.Payables.FindAll(new Payable()).First();
            Payable payableReturned = PagarMeService.GetDefaultService().Payables.Find(payable.Id);

            Assert.IsTrue(payable.Id.Equals(payableReturned.Id));
            Assert.IsTrue(payable.Status.Equals(payableReturned.Status));
            Assert.IsTrue(payable.TransactionId.Equals(payableReturned.TransactionId));
        }

        [TestMethod]
        public void FindAllPayablesTest()
        {
            Transaction transaction = PagarMeTestFixture.CreateTestCardTransactionWithInstallments();
            transaction.Save();


            Payable[] payables = transaction.Payables.FindAll(new Payable()).ToArray();

            foreach (var pay in payables)
            {
                Assert.IsTrue(pay.TransactionId.Equals(int.Parse(transaction.Id)));
            }
        }
    }
}
