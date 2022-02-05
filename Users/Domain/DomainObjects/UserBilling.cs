namespace plannerBackEnd.Users.Domain.DomainObjects
{
    public class UserBilling
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public string StripeCustomerId { get; set; } = "";
        public string StripeSubscriptionId { get; set; } = "";
        public string StripePriceId { get; set; } = "";
        public string StripeCurrentPeriodEnd { get; set; } = "";
        public string StripeStatus { get; set; } = "";

    }
}