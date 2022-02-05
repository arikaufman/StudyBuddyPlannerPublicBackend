using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Friends.Domain.DomainObjects;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Friends.DataAccess
{
    public interface IFriendDataAccessor
    {
        List<Friend> GetListFriends(FriendFilterRequest friendFilterRequest);
        List<ActiveFriend> GetListActiveFriends(ActiveFriendFilterRequest activeFriendFilterRequest);
        List<SuggestedFriend> GetListSuggestedFriends(UserProfile userProfile);
        BaseFilterResponse GetListFriendStreaks(BaseFilterRequest filter);
        Friend SendRequest(Friend friend);
        Friend AcceptRequest(int requestedId);
        Friend DeclineRequest(int requestedId);
        bool Delete(int requestedId);
    }
}