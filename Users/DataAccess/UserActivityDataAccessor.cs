using System.Collections.Generic;
using AutoMapper;
using plannerBackEnd.Users.DataAccess.Dao;
using plannerBackEnd.Users.DataAccess.Entities;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess
{
    public class UserActivityDataAccessor : IUserActivityDataAccessor
    {
        private readonly IMapper mapper;
        private readonly UserActivityDao userActivityDao;

        // -----------------------------------------------------------------------------

        public UserActivityDataAccessor(IMapper mapper, UserActivityDao userActivityDao)
        {
            this.mapper = mapper;
            this.userActivityDao = userActivityDao;
        }

        // -----------------------------------------------------------------------------

        public UserActivity Get(int requestedId)
        {
            return mapper.Map<UserActivityEntity, UserActivity>(userActivityDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public Dictionary<string,int> GetCount(UserActivityFilterRequest filter)
        {
            return userActivityDao.GetCount(filter);
        }

        // -----------------------------------------------------------------------------

        public UserActivity Create(UserActivity userActivity)
        {
            return mapper.Map<UserActivityEntity, UserActivity>(userActivityDao.Create(mapper.Map<UserActivity, UserActivityEntity>(userActivity)));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int userId)
        {
            return userActivityDao.Delete(userId);
        }

    }
}