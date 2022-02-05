using System.Collections.Generic;
using plannerBackEnd.Users.DataAccess;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Domain
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityDataAccessor userActivityDataAccessor;

        // -----------------------------------------------------------------------------

        public UserActivityService(
            IUserActivityDataAccessor userActivityDataAccessor
        )
        {
            this.userActivityDataAccessor = userActivityDataAccessor;
        }

        // -----------------------------------------------------------------------------

        public UserActivity Get(int requestedId)
        {
            return userActivityDataAccessor.Get(requestedId);
        }

        // -----------------------------------------------------------------------------

        public Dictionary<string, int> GetCount(UserActivityFilterRequest filter)
        {
            return userActivityDataAccessor.GetCount(filter);
        }

        // -----------------------------------------------------------------------------

        public UserActivity Create(UserActivity userActivity)
        {

            return userActivityDataAccessor.Create(userActivity);
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int requestedId)
        {
            return (userActivityDataAccessor.Delete(requestedId));
        }

    }
}