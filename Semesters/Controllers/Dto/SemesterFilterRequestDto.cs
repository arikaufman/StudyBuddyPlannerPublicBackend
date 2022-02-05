using plannerBackEnd.Common.automapper;
using plannerBackEnd.Semesters.Domain.DomainObjects;

namespace plannerBackEnd.Semesters.Controllers.Dto
{
    public class SemesterFilterRequestDto : IMaps<SemesterFilterRequest>
    {
        public int UserId { get; set; } = 0;
        public string School { get; set; } = "";

    }
}