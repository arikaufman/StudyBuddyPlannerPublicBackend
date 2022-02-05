using plannerBackEnd.Common.automapper;
using plannerBackEnd.Comparatives.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.DataAccess.Entities
{
    public class BestAssignmentEntity : IMaps<BestAssignment>
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string SubjectTitle { get; set; } = "";
        public string Color { get; set; } = "";
        public string TaskTitle { get; set; } = "";
        public string TaskType { get; set; } = "";
        public int Minutes { get; set; } = 0;

    }
}