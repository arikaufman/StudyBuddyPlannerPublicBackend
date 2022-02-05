using System.Collections.Generic;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess
{
    public interface IUserProfileDataAccessor
    {
        UserProfile Get(int requestedId);
        UserProfile Get(string email);
        List<UserProfile> GetList(UserProfileFilterRequest filterRequest);
        UserProfile Create(UserProfile userProfile);
        UserProfile Update(UserProfile userProfile);
        bool Delete(int requestedId);
    }
}