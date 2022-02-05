namespace plannerBackEnd.Semesters.Domain.DomainObjects
{
    public class SemesterFilterRequest
    {
        public int UserId { get; set; } = 0;
        public string School { get; set; } = "";
    }
}