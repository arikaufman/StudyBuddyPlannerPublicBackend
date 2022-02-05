namespace plannerBackEnd.Admin.Domain.DomainObjects
{
    public class SupportLog
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public string UserEmail { get; set; } = "";
        public string Description { get; set; } = "";
        public int Priority { get; set; } = 0;
        public string RequestType { get; set; } = "";
        public string Status { get; set; } = "";

    }
}