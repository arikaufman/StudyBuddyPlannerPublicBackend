using System.Collections.Generic;
using AutoMapper;
using plannerBackEnd.Users.DataAccess.Dao;
using plannerBackEnd.Users.DataAccess.Entities;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess
{
    public class UserProfileDataAccessor : IUserProfileDataAccessor
    {
        private readonly IMapper mapper;
        private readonly UserProfileDao userProfileDao;

        // -----------------------------------------------------------------------------

        public UserProfileDataAccessor(IMapper mapper, UserProfileDao userProfileDao)
        {
            this.mapper = mapper;
            this.userProfileDao = userProfileDao;
        }
        
        // -----------------------------------------------------------------------------

        public UserProfile Get(int requestedId)
        {
            return mapper.Map<UserProfileEntity, UserProfile>(userProfileDao.Get(requestedId));
        }


        // -----------------------------------------------------------------------------

        public UserProfile Get(string email)
        {
            return mapper.Map<UserProfileEntity, UserProfile>(userProfileDao.Get(email));
        }

        // -----------------------------------------------------------------------------

        public List<UserProfile> GetList(UserProfileFilterRequest filterRequest)
        {
            return mapper.Map<List<UserProfileEntity>, List<UserProfile>>(userProfileDao.GetList(filterRequest));
        }

        // -----------------------------------------------------------------------------

        public UserProfile Create(UserProfile userProfile)
        {
            return mapper.Map<UserProfileEntity, UserProfile>(userProfileDao.Create(mapper.Map<UserProfile, UserProfileEntity>(userProfile)));
        }

        // -----------------------------------------------------------------------------

        public UserProfile Update(UserProfile userProfile)
        {
            return mapper.Map<UserProfileEntity, UserProfile>(userProfileDao.Update(mapper.Map<UserProfile, UserProfileEntity>(userProfile)));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int userId)
        {
            return userProfileDao.Delete(userId);
        }

    }
}