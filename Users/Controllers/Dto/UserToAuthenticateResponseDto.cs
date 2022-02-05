using plannerBackEnd.Common.automapper;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers.Dto
{
    public class UserToAuthenticateResponseDto : IMaps<UserToAuthenticateResponse>
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Token { get; set; } = "";

    }
}