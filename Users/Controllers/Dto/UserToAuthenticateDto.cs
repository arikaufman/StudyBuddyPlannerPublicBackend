using plannerBackEnd.Common.automapper;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers.Dto
{
    public class UserToAuthenticateDto : IMaps<UserToAuthenticate>
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

    }
}