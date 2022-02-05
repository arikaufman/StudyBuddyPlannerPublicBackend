using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Subjects.Domain.DomainObjects
{
    public class Subject
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string ClassCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string Professor { get; set; } = "";
        public double Credits { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public string Color { get; set; } = "";
        public int SemesterId { get; set; } = 0;
        public int Active { get; set; } = 0;

        //Generated Fields
        public BaseFilterResponse SubjectBreakdown { get; set; } = new BaseFilterResponse();
    }
}