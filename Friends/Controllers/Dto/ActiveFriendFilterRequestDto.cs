using System;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Friends.Domain.DomainObjects;

namespace plannerBackEnd.Friends.Controllers.Dto
{
    public class ActiveFriendFilterRequestDto : IMaps<ActiveFriendFilterRequest>
    {
        public int Id { get; set; } = 0;
        public DateTime CurrentTime { get; set; } = DateTime.Now;
    }
}