using Newtonsoft.Json;

namespace plannerBackEnd.Users.Domain.DomainObjects
{
    public class UserBillingSubscriptionRequest
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        [JsonProperty("paymentMethodId")] public string PaymentMethod { get; set; } = "";

        [JsonProperty("customerId")] public string Customer { get; set; } = "";

        [JsonProperty("priceId")] public string Price { get; set; } = "";
    }
}