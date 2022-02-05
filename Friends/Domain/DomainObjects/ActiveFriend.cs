using System;

namespace plannerBackEnd.Friends.Domain.DomainObjects
{
    public class ActiveFriend
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int FriendRowId { get; set; } = 0;
        public string School { get; set; } = "";
        public string Faculty { get; set; } = "";
        public string Major { get; set; } = "";
        public string SubjectName { get; set; } = "";
        public string SubjectClassCode { get; set; } = "";
        public string TaskType { get; set; } = "";
        public string TaskDescription { get; set; } = "";
        public DateTime LastActive { get; set; } = DateTime.Now;
        public int Active { get; set; } = 0;
        public double TimezoneOffset { get; set; } = 0;

        //Generated Fields
        public string LastActiveTime { get; set; } = "";
        public string LastActiveUnit { get; set; } = "";
    }
}