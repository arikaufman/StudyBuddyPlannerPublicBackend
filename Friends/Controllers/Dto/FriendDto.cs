using plannerBackEnd.Common.automapper;
using plannerBackEnd.Friends.Domain.DomainObjects;

namespace plannerBackEnd.Friends.Controllers.Dto
{
    public class FriendDto : IMaps<Friend>
    {
        public int Id { get; set; } = 0;
        public int UserId1 { get; set; } = 0;
        public int UserId2 { get; set; } = 0;

        //GENERATED FIELDS
        public string DisplayType { get; set; } = "";
        public string RequestEmail { get; set; } = "";
        public string RequestFirstName { get; set; } = "";
        public string RequestLastName { get; set; } = "";
    }
}