using System.Collections.Generic;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Domain
{
    public interface IUserProfileService
    {
        UserProfile Get(int requestedId);
        UserProfile Get(string email, bool limit);
        List<UserProfile> GetList(UserProfileFilterRequest filterRequest);
        UserProfile Create(UserProfile userProfile);
        UserProfile Update(UserProfile userProfile);
        bool Delete(int requestedId);
        void ResetPassword(string email);
        UserToAuthenticateResponse Authenticate(UserToAuthenticate userToAuthenticate);

    }
}