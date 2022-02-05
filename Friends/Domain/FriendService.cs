using Common.Enums;
using Microsoft.OpenApi.Extensions;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Feeds.Domain;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using plannerBackEnd.Friends.DataAccess;
using plannerBackEnd.Friends.Domain.DomainObjects;
using plannerBackEnd.Schools;
using plannerBackEnd.Schools.Domain;
using plannerBackEnd.Users.Domain;
using plannerBackEnd.Users.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.IO;

namespace plannerBackEnd.Friends.Domain
{
    public class FriendService : IFriendService
    {
        private readonly IFriendDataAccessor friendDataAccessor;
        private readonly IFeedService feedService;
        private readonly IUserProfileService userProfileService;
        private readonly IAdminService adminService;
        private readonly ISchoolService schoolService;
        private readonly RequestContext requestContext;

        // -----------------------------------------------------------------------------

        public FriendService(
            IFriendDataAccessor friendDataAccessor,
            IFeedService feedService,
            IUserProfileService userProfileService,
            IAdminService adminService,
            ISchoolService schoolService,
            RequestContext requestContext
        )
        {
            this.friendDataAccessor = friendDataAccessor;
            this.feedService = feedService;
            this.userProfileService = userProfileService;
            this.adminService = adminService;
            this.schoolService = schoolService;
            this.requestContext = requestContext;
        }

        // -----------------------------------------------------------------------------
        public List<Friend> GetListFriends(FriendFilterRequest friendFilterRequest)
        {
            List<Friend> friendsPending = friendDataAccessor.GetListFriends(friendFilterRequest);
            foreach (Friend request in friendsPending)
            {
                if (request.UserId1 == friendFilterRequest.Id)
                {
                    request.DisplayType = FriendDisplayType.Sent.GetDisplayName();
                    UserProfile userProfile = userProfileService.Get(request.UserId2);
                    request.RequestEmail = userProfile.Email;
                    request.RequestFirstName = userProfile.FirstName;
                    request.RequestLastName = userProfile.LastName;
                }
                else if (request.UserId2 == friendFilterRequest.Id)
                {
                    request.DisplayType = FriendDisplayType.AcceptDecline.GetDisplayName();
                    UserProfile userProfile = userProfileService.Get(request.UserId1);
                    request.RequestEmail = userProfile.Email;
                    request.RequestFirstName = userProfile.FirstName;
                    request.RequestLastName = userProfile.LastName;
                }
            }

            return friendsPending;
        }

        // -----------------------------------------------------------------------------
        public List<ActiveFriend> GetListActiveFriends(ActiveFriendFilterRequest activeFriendFilterRequest)
        {
            List<ActiveFriend> friends = friendDataAccessor.GetListActiveFriends(activeFriendFilterRequest);
            foreach (ActiveFriend friend in friends)
            {
                if (friend.Active == 0)
                {
                    TimeSpan timeSpan = activeFriendFilterRequest.CurrentTime - friend.LastActive; 
                    double minutes = timeSpan.TotalMinutes;
                    if (minutes < 60)
                    {
                        friend.LastActiveTime = Math.Floor(minutes).ToString();
                        friend.LastActiveUnit = "m";
                    }

                    if (minutes > 60)
                    {
                        friend.LastActiveTime = Math.Floor(minutes / 60).ToString();
                        friend.LastActiveUnit = "h";
                    }

                    if (minutes > 1440)
                    {
                        friend.LastActiveTime = Math.Floor(minutes / 1440).ToString();
                        friend.LastActiveUnit = "d";
                    }

                }
            }

            return friends;
        }

        // -----------------------------------------------------------------------------
        public List<SuggestedFriend> GetListSuggestedFriends(int requestedId)
        {
            UserProfile userProfile = userProfileService.Get(requestedId);
            List<SuggestedFriend> suggestedFriends = friendDataAccessor.GetListSuggestedFriends(userProfile);
            return suggestedFriends;
        }

        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListFriendStreaks(BaseFilterRequest filter)
        {
            return friendDataAccessor.GetListFriendStreaks(filter);
        }

        // -----------------------------------------------------------------------------
        public Friend SendRequest(Friend friend)
        {
            FriendFilterRequest friendFilterRequest = new FriendFilterRequest(){Id = friend.UserId1, Pending = false};
            List<Friend> friends = GetListFriends(friendFilterRequest);
            foreach (Friend existingFriend in friends)
            {
                if (existingFriend.UserId1 == friend.UserId2)
                    throw new InvalidDataException();
                if (existingFriend.UserId2 == friend.UserId2)
                    throw new InvalidDataException();
            }

            UserProfile userProfile = userProfileService.Get(friend.UserId1);
            UserProfile currentUserProfile = userProfileService.Get(friend.UserId2);
            School school = schoolService.Get(userProfile.SchoolId);
            adminService.SendEmail("FriendNotification", currentUserProfile.FirstName, currentUserProfile.LastName, currentUserProfile.Email, 
                userProfile.FirstName, userProfile.LastName, school.Name);
            return friendDataAccessor.SendRequest(friend);
            
        }

        // -----------------------------------------------------------------------------

        public Friend AcceptRequest(int requestedId)
        {
            Feed feed = new Feed()
                    {
                        UserId = requestContext.UserId,
                        ReferenceId = requestedId,
                        DisplayType = "friendAccept",
                        Visibility = 1
                    };
            Feed returnFeed = feedService.Create(feed);

            
            return friendDataAccessor.AcceptRequest(requestedId);
        }

        // -----------------------------------------------------------------------------

        public Friend DeclineRequest(int requestedId)
        {
            return friendDataAccessor.DeclineRequest(requestedId);
        }

        // -----------------------------------------------------------------------------
        public bool Delete(int requestedId)
        {
            return (friendDataAccessor.Delete(requestedId));
        }
    }
}