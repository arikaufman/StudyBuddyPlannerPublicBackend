using System;

namespace plannerBackEnd.Friends.Domain.DomainObjects
{
    public class SuggestedFriend
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string School { get; set; } = "";
        public string Faculty { get; set; } = "";
        public int NumberOfFriends { get; set; } = 0;
        public int Active { get; set; } = 0;

    }
}