using Common.Enums;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Friends.Domain.DomainObjects;

namespace plannerBackEnd.Friends.DataAccess.Entities
{
    public class FriendEntity : IMaps<Friend>
    {
        public int Id { get; set; } = 0;
        public int UserId1 { get; set; } = 0;
        public int UserId2 { get; set; } = 0;
        public FriendStatus Status { get; set; } = FriendStatus.None;
    }
}