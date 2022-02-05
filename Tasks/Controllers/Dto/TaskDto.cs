using System;
using System.Collections.Generic;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Tasks.Domain.DomainObjects;

namespace plannerBackEnd.Tasks.Controllers.Dto
{
    public class TaskDto : IMaps<Task>
    {
        public int Id { get; set; } = 0;
        public string TaskType { get; set; } = "";
        public string Description { get; set; } = "";
        public int Minutes { get; set; } = 0;
        public int SubjectId { get; set; } = 0;
        public DateTime DueDate { get; set; } = DateTime.Now;
        public int IsDone { get; set; } = 0;
        public string Title { get; set; } = "";
        public int Active { get; set; } = 0;
        public int UserId { get; set; } = 0;


        //Generated Fields
        public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
        public List<TaskSession> TaskSessions { get; set; } = null;


    }
}