using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PagarMe.Test
{
    [TestClass]
    public class TransactionTests : PagarMeTestFixture
    {
        [TestMethod]
        public void Charge()
        {
            var transaction = CreateTestTransaction();

            transaction.Save();

            Assert.IsTrue(transaction.Status == TransactionStatus.Paid);
        }

        [TestMethod]
        public void ChargeWithAsync()
        {
            var transaction = CreateTestTransaction();
            transaction.Async = true;
            transaction.Save();

            Assert.IsTrue(transaction.Status == TransactionStatus.Processing);
        }

        [TestMethod]
        public void ChargeWithCustomerBornAtNull()
        {
            var transaction = CreateTestTransaction();
            transaction.Customer = new Customer()
            {
                Name = "Aardvark Silva",
                Email = "aardvark.silva@pagar.me",
                BornAt = null,
                DocumentNumber = "00000000000"
            };

            transaction.Save();

            Assert.IsNull(transaction.Customer.BornAt);
        }

        [TestMethod]
        public void ChargeWithCaptureMethodEMV()
        {
            Type t = typeof(Transaction);
            dynamic dTransaction = Activator.CreateInstance(t, null);
            dTransaction.Amount = 10000;

            CardHash card = new CardHash()
            {
                CardNumber = "4242424242424242",
                CardHolderName = "Aardvark Silva",
                CardExpirationDate = "0140",
                CardCvv = "123"
            };

            dTransaction.CardHash = card.Generate();
            dTransaction.acquirers_configuration_id = "ac_cj7nrwwjb057txv6et3k5fd8c";
            dTransaction.capture_method = "emv";
            dTransaction.card_track_2 = "4242424242424242%3D51046070000091611111";
            dTransaction.card_emv_data = "9A031708119C01009F02060000000001009F10200FA501A030F8000000000000000000000F0000000000000000000000000000009F1A0200769F1E0830303030303030309F2608DF91B6A4D449C9819F3303E0F0E89F360202889F370411859D5F9F2701809F34034203005F2A0209868202580095056280046000";
            dTransaction.Save();

            Assert.IsNotNull(dTransaction.CardEmvResponse);

        }

        [TestMethod]
        public void Authorize()
        {
            var transaction = CreateTestTransaction();

            transaction.ShouldCapture = false;
            transaction.Save();

            Assert.IsTrue(transaction.Status == TransactionStatus.Authorized);

            transaction.Capture();

            Assert.IsTrue(transaction.Status == TransactionStatus.Paid);
        }

        [TestMethod]
        public void Refund()
        {
            var transaction = CreateTestTransaction();

            transaction.Save();
            transaction.Refund();

            Assert.IsTrue(transaction.Status == TransactionStatus.Refunded);
        }
        [TestMethod]
        public async Task RefundWithSynchronousRequest()
        {
            var transaction = CreateTestCardTransactionWithInstallments();
            transaction.PostbackUrl = "https://api.aardvark.me/1/handlepostback";
            transaction.Async = false;
#if HAS_ASYNC
            await transaction.SaveAsync();
#else
            await Task.Run(() =>
            {
                transaction.Save();
            });
#endif
            transaction.Refund(asyncRefund: false);

            Assert.IsTrue(transaction.Status == TransactionStatus.Refunded);
        }

        [TestMethod]
        public async Task RefundWithAsynchronousRequest()
        {
            var transaction = CreateTestCardTransactionWithInstallments();
            transaction.PostbackUrl = "https://api.aardvark.me/1/handlepostback";
            transaction.Async = false;
#if HAS_ASYNC
            await transaction.SaveAsync();
#else
            await Task.Run(() =>
            {
                transaction.Save();
            });
#endif

            transaction.Refund();

            Assert.IsTrue(transaction.Status == TransactionStatus.Paid);
        }

        [TestMethod]
        public void RefundWithBoleto()
        {
            var transaction = CreateTestBoletoTransaction();
            transaction.Save();
            transaction.Status = TransactionStatus.Paid;
            transaction.Save();
            var bankAccount = CreateTestBankAccount();
            transaction.Refund(bankAccount);
            Assert.IsTrue(transaction.Status == TransactionStatus.PendingRefund);
        }

        [TestMethod]
        public void PartialRefund()
        {
            var transaction = CreateTestTransaction();

            transaction.Save();
            int amountToBeRefunded = 100;
            transaction.Refund(amountToBeRefunded);

            Assert.IsTrue(transaction.Status == TransactionStatus.Paid);
            Assert.IsTrue(transaction.RefundedAmount == amountToBeRefunded);
        }

        [TestMethod]
        public void SendMetadata()
        {
            var transaction = CreateTestTransaction();

            transaction.Metadata["test"] = "uhuul";

            transaction.Save();

            Assert.IsTrue(transaction.Metadata["test"].ToString() == "uhuul");
        }

        [TestMethod]
        public void FindPayablesTest()
        {
            Transaction transaction = CreateTestBoletoTransaction();
            transaction.Save();
            transaction.Status = TransactionStatus.Paid;
            transaction.Save();

            Payable payable = transaction.Payables.FindAll(new Payable()).First();
            Assert.IsTrue(payable.Amount.Equals(transaction.Amount));
            Assert.IsTrue(payable.Status.Equals(PayableStatus.Paid));
        }

        [TestMethod]
        public void TransactionWithSplitRule()
        {
            var transaction = CreateTestTransaction();
            Recipient r1 = CreateRecipient();
            Recipient r2 = CreateRecipient();
            r1.Save();
            r2.Save();
            transaction.SplitRules = new SplitRule[] {
                 new SplitRule() {
                        Liable = true,
                        Percentage = 10,
                        ChargeProcessingFee = true,
                        RecipientId = r1.Id
                },
                new SplitRule(){
                        RecipientId = r2.Id,
                        ChargeProcessingFee = true,
                        Liable = true,
                        Percentage = 90
                    }
            };
            transaction.Save();
            Assert.IsTrue(transaction.Status == TransactionStatus.Paid);
            Assert.AreEqual(transaction.Amount, 1099);
            Assert.AreEqual(transaction.SplitRules[1].RecipientId, r1.Id);
            Assert.AreEqual(transaction.SplitRules[1].Percentage, 10);
            Assert.IsTrue(transaction.SplitRules[1].ChargeProcessingFee);
            Assert.IsTrue(transaction.SplitRules[1].Liable);
            Assert.AreEqual(transaction.SplitRules[0].RecipientId, r2.Id);
            Assert.AreEqual(transaction.SplitRules[0].Percentage, 90);
            Assert.IsTrue(transaction.SplitRules[0].ChargeProcessingFee);
            Assert.IsTrue(transaction.SplitRules[0].Liable);

        }
    }
}
