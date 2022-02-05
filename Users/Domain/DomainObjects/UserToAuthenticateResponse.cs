namespace plannerBackEnd.Users.Domain.DomainObjects
{
    public class UserToAuthenticateResponse
    {
        public int Id { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Token { get; set; } = "";
    }
}