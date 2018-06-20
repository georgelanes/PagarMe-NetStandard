using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PagarMe.Test
{
    [TestClass]
    public class CreditCardTests : PagarMeTestFixture
    {
        [TestMethod]
        public void CreateHash()
        {
            Assert.IsTrue(GetCardHash().Length > 0);
        }
    }
}
