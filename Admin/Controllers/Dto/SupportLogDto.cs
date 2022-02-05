using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;

namespace plannerBackEnd.Admin.Controllers.Dto
{
    public class SupportLogDto : IMaps<SupportLog>
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