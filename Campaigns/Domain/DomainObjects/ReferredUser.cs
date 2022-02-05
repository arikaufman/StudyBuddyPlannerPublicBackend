using System;
using System.Collections.Generic;
using plannerBackEnd.Campaigns.DataAccess.Entities;

namespace plannerBackEnd.Campaigns.Domain.DomainObjects
{
    public class ReferredUser
    {
        public string CampaignCode { get; set; } = "";
        public List<ReferredUserItem> ReferredUsers { get; set; } = new List<ReferredUserItem>();

    }

    public class ReferredUserItem
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