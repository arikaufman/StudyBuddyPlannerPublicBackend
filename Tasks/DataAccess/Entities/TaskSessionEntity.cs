using System;
using plannerBackEnd.Common.automapper;
using plannerBackEnd.Tasks.Domain.DomainObjects;

namespace plannerBackEnd.Tasks.DataAccess.Entities
{
    public class TaskSessionEntity : IMaps<TaskSession>
    {
        public int Id { get; set; } = 0;
        public int Minutes { get; set; } = 0;
        public DateTime DateCompleted { get; set; } = DateTime.Now;
        public int TaskId { get; set; } = 0;
        public string Title { get; set; } = "";
    }
}