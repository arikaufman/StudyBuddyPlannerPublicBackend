using System.Text;

namespace plannerBackEnd.Users.Domain.DomainObjects
{
    public class UserProfileFilterRequest
    {
        public string Search { get; set; } = "";
        public bool Active { get; set; } = false;
    }
}