using Common.Enums;

namespace plannerBackEnd.Friends.Domain.DomainObjects
{
    public class FriendFilterRequest
    {
        public int Id { get; set; } = 0;
        public bool Pending { get; set; } = false;
    }
}