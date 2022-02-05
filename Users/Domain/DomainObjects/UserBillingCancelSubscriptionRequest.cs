using Newtonsoft.Json;

namespace plannerBackEnd.Users.Domain.DomainObjects
{
    public class UserBillingCancelSubscriptionRequest
    {
        public int Id { get; set; } = 0;
        public string SubscriptionId { get; set; } = "";
    }
}