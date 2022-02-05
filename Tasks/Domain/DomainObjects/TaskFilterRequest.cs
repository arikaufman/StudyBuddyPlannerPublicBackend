namespace plannerBackEnd.Tasks.Domain.DomainObjects
{
    public class TaskFilterRequest
    {
        public int UserId { get; set; } = 0;
        public int IsDone { get; set; } = 0;
        public bool FilterBySubject { get; set; } = false;
        public bool FilterByDueDate { get; set; } = false;

    }
}