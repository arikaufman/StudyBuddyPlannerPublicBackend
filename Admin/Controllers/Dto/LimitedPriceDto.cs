using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;

namespace plannerBackEnd.Admin.Controllers.Dto
{
    public class LimitedPriceDto : IMaps<LimitedPrice>
    {
        public string Id { get; set; } = "";
        public string Interval { get; set; } = "";
        public double Amount { get; set; } = 0;
    }
}