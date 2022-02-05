using System;
using Common.Enums;

namespace plannerBackEnd.Friends.Domain.DomainObjects
{
    public class ActiveFriendFilterRequest
    {
        public int Id { get; set; } = 0;
        public DateTime CurrentTime { get; set; } = DateTime.Now;
    }
}