using System;

namespace plannerBackEnd.Users.Domain.DomainObjects
{
    public class UserActivity
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public int Active { get; set; } = 0;
        public int CurrentTaskId { get; set; } = 0;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public int TimezoneOffset { get; set; } = 0;
    }
}