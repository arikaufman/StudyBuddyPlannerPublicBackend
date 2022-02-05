using System;
using Common.Enums;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Friends.Domain.DomainObjects
{
    public class Friend
    {
        public int Id { get; set; } = 0;
        public int UserId1 { get; set; } = 0;
        public int UserId2 { get; set; } = 0;
        public FriendStatus Status { get; set; } = FriendStatus.None;

        //GENERATED FIELDS
        public string DisplayType { get; set; } = "";
        public string RequestEmail { get; set; } = "";
        public string RequestFirstName { get; set; } = "";
        public string RequestLastName { get; set; } = "";
    }
}