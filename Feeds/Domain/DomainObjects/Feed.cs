using System;

namespace plannerBackEnd.Feeds.Domain.DomainObjects
{
    public class Feed
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public string SelfDescription { get; set; } = "";
        public string GeneralDescription { get; set; } = "";
        public int Visibility { get; set; } = 0;
        public string DisplayType { get; set; } = "";
        public int ReferenceId { get; set; } = 0;
        public DateTime RowCreated { get; set; } = DateTime.Now;

        //Generated Fields
        public string FeedTime { get; set; } = "";
        public string FeedUnit { get; set; } = "";
    }
}