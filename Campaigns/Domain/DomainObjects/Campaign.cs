using System;

namespace plannerBackEnd.Campaigns.Domain.DomainObjects
{
    public class Campaign
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public string CampaignCode { get; set; } = "";
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

    }
}