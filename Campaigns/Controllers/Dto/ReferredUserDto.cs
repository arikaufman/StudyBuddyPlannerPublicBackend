using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;
using System.Collections.Generic;

namespace plannerBackEnd.Campaigns.Controllers.Dto
{
    public class ReferredUserDto : IMaps<ReferredUser>
    {
        public string CampaignCode { get; set; } = "";
        public List<ReferredUserItemDto> ReferredUsers { get; set; } = new List<ReferredUserItemDto>();
    }

    public class ReferredUserItemDto : IMaps<ReferredUserItem>
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string School { get; set; } = "";
        public string Faculty { get; set; } = "";
        public string Active { get; set; } = "";
        public string Major { get; set; } = "";
        public string Email { get; set; } = "";

    }
}