using plannerBackEnd.Common.automapper;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Subjects.Controllers.Dto
{
    public class SubjectFilterRequestDto :  IMaps<SubjectFilterRequest>
    {
        public int UserId { get; set; } = 0;
        public int SemesterId { get; set; } = 0;

    }
}