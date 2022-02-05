using System;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers.Dto
{
    public class UserActivityDto : IMaps<UserActivity>
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public int Active { get; set; } = 0;
        public int CurrentTaskId { get; set; } = 0;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public int TimezoneOffset { get; set; } = 0;
    }
}