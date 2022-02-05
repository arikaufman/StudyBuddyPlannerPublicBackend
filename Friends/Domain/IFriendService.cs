using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Friends.Domain.DomainObjects;

namespace plannerBackEnd.Friends.Domain
{
    public interface IFriendService
    {
        List<Friend> GetListFriends(FriendFilterRequest friendFilterRequest);
        List<ActiveFriend> GetListActiveFriends(ActiveFriendFilterRequest activeFriendFilterRequest);
        List<SuggestedFriend> GetListSuggestedFriends(int requestedId);
        BaseFilterResponse GetListFriendStreaks(BaseFilterRequest filter);
        Friend SendRequest(Friend friend);
        Friend AcceptRequest(int requestedId);
        Friend DeclineRequest(int requestedId);
        bool Delete(int requestedId);

    }
}