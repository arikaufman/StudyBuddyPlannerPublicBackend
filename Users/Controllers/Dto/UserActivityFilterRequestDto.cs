using plannerBackEnd.Common.automapper;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers.Dto
{
    public class UserActivityFilterRequestDto : IMaps<UserActivityFilterRequest>
    {
        public int Faculty { get; set; } = 0;
        public int School { get; set; } = 0;

    }
}