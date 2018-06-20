using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace PagarMe.Test
{
    [TestClass]
    public class BankAccountTests : PagarMeTestFixture
    {
        [TestMethod]
        public void CreateCorrenteBankAccount()
        {
            var bank = CreateTestBankAccount();
            bank.Type = AccountType.Corrente;

            bank.Save();

            Assert.IsNotNull(bank.Id);
            Assert.IsTrue(bank.Type == AccountType.Corrente);
        }

        [TestMethod]
        public void CreatePoupancaBankAccount()
        {
            var bank = CreateTestBankAccount();
            bank.Type = AccountType.Poupanca;

            bank.Save();

            Assert.IsNotNull(bank.Id);
            Assert.IsTrue(bank.Type == AccountType.Poupanca);
        }

        [TestMethod]
        public void CreateCorrenteConjuntaBankAccount()
        {
            var bank = CreateTestBankAccount();
            bank.Type = AccountType.CorrenteConjunta;

            bank.Save();

            Assert.IsNotNull(bank.Id);
            Assert.IsTrue(bank.Type == AccountType.CorrenteConjunta);
        }

        [TestMethod]
        public void CreatePoupancaConjuntaBankAccount()
        {
            var bank = CreateTestBankAccount();
            bank.Type = AccountType.PoupancaConjunta;

            bank.Save();

            Assert.IsNotNull(bank.Id);
            Assert.IsTrue(bank.Type == AccountType.PoupancaConjunta);
        }
    }
}
