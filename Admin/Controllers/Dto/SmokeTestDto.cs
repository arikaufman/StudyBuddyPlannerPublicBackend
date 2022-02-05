using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.automapper;

namespace plannerBackEnd.Admin.Controllers.Dto
{
    public class SmokeTestDto : IMaps<SmokeTest>
    {
        public int Id { get; set; } = 0;
        public string Description { get; set; } = "";
    }
}
