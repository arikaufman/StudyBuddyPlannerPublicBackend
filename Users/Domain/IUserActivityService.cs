using System.Collections.Generic;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Domain
{
    public interface IUserActivityService
    {
        UserActivity Get(int requestedId);
        Dictionary<string, int> GetCount(UserActivityFilterRequest filter);
        UserActivity Create(UserActivity userActivity);
        bool Delete(int requestedId);
    }
}