using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PagarMe.Test
{
    [TestClass]
    public class TransferTest : PagarMeTestFixture
    {
       
        [TestMethod]
        public void CreateTestTransferWithDifferentBankAccount()
        {

            Assert.ThrowsException<PagarMeException>(() =>
            {
                BankAccount bank = CreateTestBankAccount();
                bank.Save();
                Recipient recipient = CreateRecipientWithAnotherBankAccount();
                recipient.Save();
                Transfer transfer = PagarMeTestFixture.CreateTestTransfer(bank.Id, recipient.Id);
                transfer.Save();
            });
        }

        [TestMethod]
        public void CreateTransferTest()
        {

            BankAccount bank = PagarMeTestFixture.CreateTestBankAccount();
            bank.Save();
            Recipient recipient = PagarMeTestFixture.CreateRecipient(bank);
            recipient.Save();

            Transaction transaction = PagarMeTestFixture.CreateBoletoSplitRuleTransaction(recipient);
            transaction.Save();
            transaction.Status = TransactionStatus.Paid;
            transaction.Save();

            Transfer transfer = PagarMeTestFixture.CreateTestTransfer(bank.Id, recipient.Id);
            transfer.Save();
            Assert.IsTrue(transfer.Status == TransferStatus.PendingTransfer);

        }

        [TestMethod]
        public void FindTransferTest()
        {
            BankAccount bank = PagarMeTestFixture.CreateTestBankAccount();
            bank.Save();
            Recipient recipient = PagarMeTestFixture.CreateRecipient(bank);
            recipient.Save();

            Transaction transaction = PagarMeTestFixture.CreateBoletoSplitRuleTransaction(recipient);
            transaction.Save();
            transaction.Status = TransactionStatus.Paid;
            transaction.Save();

            Transfer transfer = PagarMeTestFixture.CreateTestTransfer(bank.Id, recipient.Id);
            transfer.Save();

            Transfer transferReturned = PagarMeService.GetDefaultService().Transfers.Find(transfer.Id);

            Assert.IsTrue(transferReturned.Id.Equals(transfer.Id));
            Assert.IsTrue(transferReturned.Amount.Equals(transfer.Amount));
            Assert.IsTrue(transferReturned.DateCreated.Equals(transfer.DateCreated));
            Assert.IsTrue(transferReturned.Fee.Equals(transfer.Fee));
            Assert.IsTrue(transferReturned.Status.Equals(transfer.Status));
            Assert.IsTrue(transferReturned.Type.Equals(transfer.Type));
        }

        [TestMethod]
        public void FindAllTransferTest()
        {
            BankAccount bank = PagarMeTestFixture.CreateTestBankAccount();
            bank.Save();
            Recipient recipient = PagarMeTestFixture.CreateRecipient(bank);
            recipient.Save();

            Transaction transaction = PagarMeTestFixture.CreateBoletoSplitRuleTransaction(recipient);
            transaction.Save();
            transaction.Status = TransactionStatus.Paid;
            transaction.Save();

            Transfer transfer = PagarMeTestFixture.CreateTestTransfer(bank.Id, recipient.Id);
            transfer.Save();

            var transfers = PagarMeService.GetDefaultService().Transfers.FindAll(new Transfer());

            Assert.IsTrue(transfers.Count() >= 1);

        }

        [TestMethod]
        public void CancelTransfer()
        {
            BankAccount bank = PagarMeTestFixture.CreateTestBankAccount();
            bank.Save();
            Recipient recipient = PagarMeTestFixture.CreateRecipient(bank);
            recipient.Save();

            Transaction transaction = PagarMeTestFixture.CreateBoletoSplitRuleTransaction(recipient);
            transaction.Save();
            transaction.Status = TransactionStatus.Paid;
            transaction.Save();

            Transfer transfer = PagarMeTestFixture.CreateTestTransfer(bank.Id, recipient.Id);
            transfer.Save();

            transfer.CancelTransfer();

            Assert.IsTrue(transfer.Status == TransferStatus.Canceled);

        }
    }
}
