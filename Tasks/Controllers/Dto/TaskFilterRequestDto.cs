using plannerBackEnd.Common.automapper;
using plannerBackEnd.Tasks.Domain.DomainObjects;

namespace plannerBackEnd.Tasks.Controllers.Dto
{
    public class TaskFilterRequestDto : IMaps<TaskFilterRequest>
    {
        public int UserId { get; set; } = 0;
        public int IsDone { get; set; } = 0;
        public bool FilterBySubject { get; set; } = false;
        public bool FilterByDueDate { get; set; } = false;

    }
}