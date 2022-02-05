using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Domain
{
    public interface IUserBillingService
    {
        UserBilling Get(int requestedId);
        UserBilling GetByUserId(int requestedId);
        UserBilling Create(UserBilling userBilling);
        UserBilling Update(UserBilling userBilling);
        UserBilling CreateSubscription(UserBillingSubscriptionRequest userBillingSubscriptionRequest);
        UserBilling CancelSubscription(UserBillingCancelSubscriptionRequest userBillingCancelSubscriptionRequest);
    }
}