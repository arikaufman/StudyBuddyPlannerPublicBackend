using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Users.DataAccess;
using plannerBackEnd.Users.Domain.DomainObjects;
using Stripe;

namespace plannerBackEnd.Users.Domain
{
    public class UserBillingService : IUserBillingService
    {
        private readonly IUserBillingDataAccessor userBillingDataAccessor;
        private readonly IStripeService stripeService;

        // -----------------------------------------------------------------------------

        public UserBillingService(
            IUserBillingDataAccessor userBillingDataAccessor,
            IStripeService stripeService
        )
        {
            this.userBillingDataAccessor = userBillingDataAccessor;
            this.stripeService = stripeService;
        }

        // -----------------------------------------------------------------------------

        public UserBilling Get(int requestedId)
        {
            return userBillingDataAccessor.Get(requestedId);
        }

        // -----------------------------------------------------------------------------

        public UserBilling GetByUserId(int requestedId)
        {
            return userBillingDataAccessor.GetByUserId(requestedId);
        }

        // -----------------------------------------------------------------------------

        public UserBilling Create(UserBilling userBilling)
        {

            return userBillingDataAccessor.Create(userBilling);
        }

        // -----------------------------------------------------------------------------

        public UserBilling Update(UserBilling userBilling)
        {

            return userBillingDataAccessor.Update(userBilling);
        }

        // -----------------------------------------------------------------------------

        public UserBilling CreateSubscription(UserBillingSubscriptionRequest userBillingSubscriptionRequest)
        {
            Subscription subscription = stripeService.CreateSubscription(userBillingSubscriptionRequest);
            UserBilling currentUserBilling = Get(userBillingSubscriptionRequest.Id);
            UserBilling userBillingToUpdate = new UserBilling()
            {
                Id = userBillingSubscriptionRequest.Id,
                UserId = currentUserBilling.UserId,
                StripeCustomerId = currentUserBilling.StripeCustomerId,
                StripeCurrentPeriodEnd = subscription.CurrentPeriodEnd.ToString(),
                StripePriceId = subscription.Items.Data[0].Price.Id,
                StripeSubscriptionId = subscription.Id,
                StripeStatus = subscription.Status
            };

            return Update(userBillingToUpdate);
        }

        // -----------------------------------------------------------------------------

        public UserBilling CancelSubscription(UserBillingCancelSubscriptionRequest userBillingCancelSubscriptionRequest)
        {
            Subscription subscription = stripeService.CancelSubscription(userBillingCancelSubscriptionRequest);
            UserBilling currentUserBilling = Get(userBillingCancelSubscriptionRequest.Id);
            UserBilling userBillingToUpdate = new UserBilling()
            {
                Id = userBillingCancelSubscriptionRequest.Id,
                UserId = currentUserBilling.UserId,
                StripeCustomerId = currentUserBilling.StripeCustomerId,
                StripeCurrentPeriodEnd = subscription.CurrentPeriodEnd.ToString(),
                StripePriceId = subscription.Items.Data[0].Price.Id,
                StripeSubscriptionId = subscription.Id,
                StripeStatus = subscription.Status
            };

            return Update(userBillingToUpdate);
        }
    }
}