using PagarMe;
using PagarMe.Model;
using System;
using System.Linq;

namespace Playground
{
    class Program
    {
        public static void Main(string[] args)
        {
            PagarMeService.DefaultApiKey = "ak_test_RBORKsHflgcrO7gISMyhatMx8UyiJY";
            PagarMeService.DefaultEncryptionKey = "ek_test_Ajej5CakM8QXGnA2lWX3AarwLWqspL";

            Transfer[] transfer = PagarMeService.GetDefaultService().Transfers.FindAll(new Transfer()).ToArray();
            Console.Write(transfer.Count());
            Console.Read();

            Recipient recipient = PagarMeService.GetDefaultService().Recipients.Find("re_ciwxxlge502jfwp6exm59a1ir");

            var x = recipient.Balance.Operations;

            var y = x.FindAll(new BalanceOperation()).First().MovementTransfer;

            Console.Write(y.Amount);

            try
            {
                BankAccount b = new BankAccount
                {
                    Agencia = "0196",
                    AgenciaDv = "0",
                    Conta = "05392",
                    ContaDv = "0",
                    BankCode = "0341",
                    DocumentNumber = "05737104141",
                    LegalName = "JONATHAN LIMA"
                };
                b.Save();

                Recipient r1 = PagarMeService.GetDefaultService().Recipients.Find("re_ci76hxnym00b8dw16y3hdxb21");
                Recipient r2 = PagarMeService.GetDefaultService().Recipients.Find("re_ci7nheu0m0006n016o5sglg9t");
                Recipient r3 = new Recipient
                {
                    BankAccount = b,
                    TransferEnabled = true,
                    TransferInterval = TransferInterval.Weekly,
                    TransferDay = 1,
                    AnticipatableVolumePercentage = 80,
                    AutomaticAnticipationEnabled = true
                };
                r3.Save();

                Subscription s = new Subscription()
                {
                    CardCvv = "651",
                    CardExpirationDate = "0921",
                    CardHolderName = "JONATHAN LIMA",
                    CardNumber = "4242424242424242",
                    Customer = PagarMeService.GetDefaultService().Customers.Find("77785"),
                    Plan = PagarMeService.GetDefaultService().Plans.Find("38187")
                };

                s.Save();

                Transaction t = new Transaction
                {
                    SplitRules = new[]
                {
                    new SplitRule {
                        RecipientId = r1.Id,
                        Percentage = 10,
                        ChargeProcessingFee = true,
                        Liable = true
                    },
                    new SplitRule {
                        RecipientId = r2.Id,
                        Percentage = 40,
                        ChargeProcessingFee = false,
                        Liable = false
                    },
                    new SplitRule {
                        RecipientId = r3.Id,
                        Percentage = 50,
                        ChargeProcessingFee = false,
                        Liable = false
                    }
                },

                    PaymentMethod = PaymentMethod.Boleto,
                    Amount = 10000
                };
                t.Save();
            }
            catch (PagarMeException ex)
            {
                foreach (var erro in ex.Error.Errors)
                    Console.WriteLine(String.Format("Error: {0}", erro.Message));
            }
        }
    }
}
