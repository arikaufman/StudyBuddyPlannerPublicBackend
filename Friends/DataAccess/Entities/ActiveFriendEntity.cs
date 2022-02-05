using System;
using Common.Enums;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Friends.Domain.DomainObjects;

namespace plannerBackEnd.Friends.DataAccess.Entities
{
    public class ActiveFriendEntity : IMaps<ActiveFriend>
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
    }
}