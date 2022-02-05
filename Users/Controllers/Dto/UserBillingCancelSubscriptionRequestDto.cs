using Newtonsoft.Json;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers.Dto
{
    public class UserBillingCancelSubscriptionRequestDto : IMaps<UserBillingCancelSubscriptionRequest>
    {
        public int Id { get; set; } = 0;
        public string SubscriptionId { get; set; } = "";

    }
}