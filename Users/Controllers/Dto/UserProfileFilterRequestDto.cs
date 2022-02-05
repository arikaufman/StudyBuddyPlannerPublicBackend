using plannerBackEnd.Common.automapper;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers.Dto
{
    public class UserProfileFilterRequestDto : IMaps<UserProfileFilterRequest>
    {
        public string Search { get; set; } = "";

        public bool Active { get; set; } = false;

    }
}