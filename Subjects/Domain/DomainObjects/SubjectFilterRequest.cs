namespace plannerBackEnd.Subjects.Domain.DomainObjects
{
    public class SubjectFilterRequest
    {
        public int UserId { get; set; } = 0;
        public int SemesterId { get; set; } = 0;
    }
}