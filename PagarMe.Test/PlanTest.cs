using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PagarMe.Test
{
    [TestClass]
    public class PlanTest
    {
        [TestMethod]
        public void TestCreatePlan()
        {
            Plan plan = PagarMeTestFixture.CreateTestPlan();
            plan.Save();
            Assert.IsNotNull(plan.Id);
            Assert.AreEqual(plan.TrialDays, 0);
            Assert.AreEqual(plan.Days, 30);
            Assert.AreEqual(plan.Amount, 1099);
            Assert.AreEqual(plan.Color, "#787878");
            Assert.AreEqual(plan.InvoiceReminder, 3);

            var paymentMethod = new PaymentMethod[] { PaymentMethod.CreditCard };

            CollectionAssert.AreEqual(plan.PaymentMethods, paymentMethod);
        }
    }
}
