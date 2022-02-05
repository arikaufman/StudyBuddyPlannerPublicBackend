using System.Collections.Generic;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess
{
    public interface IUserActivityDataAccessor
    {
        UserActivity Get(int requestedId);
        Dictionary<string, int> GetCount(UserActivityFilterRequest filter);
        UserActivity Create(UserActivity userActivity);
        bool Delete(int requestedId);
    }
}