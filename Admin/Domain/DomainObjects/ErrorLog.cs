namespace plannerBackEnd.Admin.Domain.DomainObjects
{
    public class ErrorLog
    {
        public int Id { get; set; } = 0;
        public string Comments { get; set; } = "";
        public string CallStack { get; set; } = "";
        public int User { get; set; } = 0;

    }
}