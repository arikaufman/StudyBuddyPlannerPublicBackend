namespace plannerBackEnd.Admin.Domain.DomainObjects
{
    public class LimitedPrice
    {
        public string Id { get; set; } = "";
        public string Interval { get; set; } = "";
        public double Amount { get; set; } = 0;
    }
}