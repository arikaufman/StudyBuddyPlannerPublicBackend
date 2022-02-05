using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;
using System;
using System.Collections.Generic;

namespace plannerBackEnd.Campaigns.DataAccess.Entities
{
    public class ReferredUserEntity : IMaps<ReferredUser>
    {
        public string CampaignCode { get; set; } = "";
        public List<ReferredUserItemEntity> ReferredUsers { get; set; } = new List<ReferredUserItemEntity>();
        
    }
    public class ReferredUserItemEntity : IMaps<ReferredUserItem>
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