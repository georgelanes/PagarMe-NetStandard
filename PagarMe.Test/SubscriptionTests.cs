using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PagarMe.Test
{
    [TestClass]
    public class SubscriptionTests : PagarMeTestFixture
    {

        [TestMethod]
        public void CreateWithPlan()
        {
            var plan = CreateTestPlan();

            plan.Save();

            Assert.AreNotEqual(plan.Id, 0);

            var subscription = new Subscription
            {
                CardHash = GetCardHash(),
                Customer = new Customer
                {
                    Email = "josedasilva@pagar.me"
                },
                Plan = plan
            };

            subscription.Save();

            Assert.AreEqual(subscription.Status, SubscriptionStatus.Paid);
        }

        [TestMethod]
        public void CreateWithPlanAndInvalidCardCvv()
        {
            var plan = CreateTestPlan();

            plan.Save();

            Assert.AreNotEqual(plan.Id, 0);

            var subscription = new Subscription
            {
                CardCvv = "651",
                CardExpirationDate = "0921",
                CardHolderName = "Jose da Silva",
                CardNumber = "4242424242424242",
                Customer = new Customer
                {
                    Email = "josedasilva@pagar.me"
                },
                Plan = plan
            };

            try
            {
                subscription.Save();
            }
            catch (PagarMeException ex)
            {
                Assert.IsNotNull(ex.Error.Errors.Where(e => e.Type == "action_forbidden").FirstOrDefault());
            }
        }

        [TestMethod]
        public void CancelSubscription()
        {
            var plan = CreateTestPlan();
            plan.Save();

            Assert.AreNotEqual(plan.Id, 0);

            var subscription = new Subscription
            {
                CardHash = GetCardHash(),
                Customer = new Customer
                {
                    Email = "josedasilva@pagar.me"
                },
                Plan = plan
            };

            subscription.Save();
            subscription.Cancel();

            Assert.AreEqual(subscription.Status, SubscriptionStatus.Canceled);
        }
    }
}
