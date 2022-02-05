namespace plannerBackEnd.Users.Domain.DomainObjects
{
    public class UserToAuthenticate
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}