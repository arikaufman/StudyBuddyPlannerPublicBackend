using plannerBackEnd.Common.automapper;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using System;

namespace plannerBackEnd.Feeds.DataAccess.Entities
{
    public class FeedEntity : IMaps<Feed>
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public string SelfDescription { get; set; } = "";
        public string GeneralDescription { get; set; } = "";
        public int Visibility { get; set; } = 0;
        public string DisplayType { get; set; } = "";
        public int ReferenceId { get; set; } = 0;
        public DateTime RowCreated { get; set; } = DateTime.Now;
    }
}