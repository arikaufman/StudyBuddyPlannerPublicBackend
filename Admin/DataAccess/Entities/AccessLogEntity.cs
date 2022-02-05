using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;

namespace plannerBackEnd.Admin.DataAccess.Entities
{
    public class AccessLogEntity : IMaps<AccessLog>
    {
        public int Id { get; set; } = 0;
        public string HttpMethod { get; set; } = "";
        public string HttpRequest { get; set; } = "";
        public string Url { get; set; } = "";
        public int User { get; set; } = 0;
        public int Duration { get; set; } = 0;
    }
}