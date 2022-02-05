using Newtonsoft.Json;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers.Dto
{
    public class UserBillingSubscriptionRequestDto : IMaps<UserBillingSubscriptionRequest>
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        [JsonProperty("paymentMethodId")] public string PaymentMethod { get; set; } = "";

        [JsonProperty("customerId")] public string Customer { get; set; } = "";

        [JsonProperty("priceId")] public string Price { get; set; } = "";

    }
}