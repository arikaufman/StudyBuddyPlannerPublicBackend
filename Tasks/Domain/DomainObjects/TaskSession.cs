using System;

namespace plannerBackEnd.Tasks.Domain.DomainObjects
{
    public class TaskSession
    {
        public int Id { get; set; } = 0;
        public int Minutes { get; set; } = 0;
        public DateTime DateCompleted { get; set; } = DateTime.Now;
        public int TaskId { get; set; } = 0;
        public string Title { get; set; } = "";

    }
}