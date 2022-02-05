using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;

namespace plannerBackEnd.Admin.DataAccess.Entities
{
    public class ErrorLogEntity : IMaps<ErrorLog>
    {
        public int Id { get; set; } = 0;
        public string Comments { get; set; } = "";
        public string CallStack { get; set; } = "";
        public int User { get; set; } = 0;
    }
}