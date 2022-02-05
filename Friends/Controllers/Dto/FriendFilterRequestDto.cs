using plannerBackEnd.Common.automapper;
using plannerBackEnd.Friends.Domain.DomainObjects;

namespace plannerBackEnd.Friends.Controllers.Dto
{
    public class FriendFilterRequestDto : IMaps<FriendFilterRequest>
    {
        public int Id { get; set; } = 0;
        public bool Pending { get; set; } = false;
    }
}