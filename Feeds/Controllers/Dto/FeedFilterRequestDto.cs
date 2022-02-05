using System;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Feeds.Domain.DomainObjects;

namespace plannerBackEnd.Friends.Controllers.Dto
{
    public class FeedFilterRequestDto : IMaps<FeedFilterRequest>
    {
        public int UserId { get; set; } = 0;
        public int Visibility { get; set; } = 0;
        public int Limit { get; set; } = 0;
        public bool DisplayFriends { get; set; } = false;

        //FOR GETREFERENCE
        public int ReferenceId { get; set; } = 0;
        public string DisplayType { get; set; } = "";
        public DateTime CurrentTime { get; set; } = DateTime.Now;
    }
}