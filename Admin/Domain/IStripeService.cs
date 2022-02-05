using System.Collections.Generic;
using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Users.Domain.DomainObjects;
using Stripe;

namespace plannerBackEnd.Admin.Domain
{
    public interface IStripeService
    {
        Customer CreateCustomer(string email);
        Subscription CreateSubscription(UserBillingSubscriptionRequest request);
        Subscription CancelSubscription(UserBillingCancelSubscriptionRequest request);
        Product GetProduct(string productKey);
        List<LimitedPrice> GetListPrices();
    }
}