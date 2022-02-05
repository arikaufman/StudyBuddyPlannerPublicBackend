using System.Collections.Generic;
using AutoMapper;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Friends.DataAccess.Dao;
using plannerBackEnd.Friends.DataAccess.Entities;
using plannerBackEnd.Friends.Domain.DomainObjects;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Friends.DataAccess
{
    public class FriendDataAccessor : IFriendDataAccessor
    {
        private readonly IMapper mapper;
        private readonly FriendDao friendDao;

        // -----------------------------------------------------------------------------

        public FriendDataAccessor(IMapper mapper, FriendDao friendDao)
        {
            this.mapper = mapper;
            this.friendDao = friendDao;
        }

        // -----------------------------------------------------------------------------

        public List<Friend> GetListFriends(FriendFilterRequest friendFilterRequest)
        {
            return mapper.Map<List<FriendEntity>, List<Friend>>(friendDao.GetListFriends(friendFilterRequest));
        }

        // -----------------------------------------------------------------------------

        public List<ActiveFriend> GetListActiveFriends(ActiveFriendFilterRequest activeFriendFilterRequest)
        {
            return mapper.Map<List<ActiveFriendEntity>, List<ActiveFriend>>(friendDao.GetListActiveFriends(activeFriendFilterRequest));
        }

        // -----------------------------------------------------------------------------

        public List<SuggestedFriend> GetListSuggestedFriends(UserProfile userProfile)
        {
            return mapper.Map<List<SuggestedFriendEntity>, List<SuggestedFriend>>(friendDao.GetListSuggestedFriends(userProfile));
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListFriendStreaks(BaseFilterRequest filter)
        {
            return (friendDao.GetListFriendStreaks(filter));
        }

        // -----------------------------------------------------------------------------

        public Friend SendRequest(Friend friend)
        {
            return mapper.Map<FriendEntity, Friend>(friendDao.SendRequest(mapper.Map<Friend, FriendEntity>(friend)));
        }

        // -----------------------------------------------------------------------------

        public Friend AcceptRequest(int requestedId)
        {
            return mapper.Map<FriendEntity, Friend>(friendDao.AcceptRequest(requestedId));
        }

        // -----------------------------------------------------------------------------

        public Friend DeclineRequest(int requestedId)
        {
            return mapper.Map<FriendEntity, Friend>(friendDao.DeclineRequest(requestedId));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int semesterId)
        {
            return friendDao.Delete(semesterId);
        }

    }
}