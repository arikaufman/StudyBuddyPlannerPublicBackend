using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;
using System;

namespace plannerBackEnd.Campaigns.DataAccess.Entities
{
    public class CampaignEntity : IMaps<Campaign>
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public string CampaignCode { get; set; } = "";
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
    }
}