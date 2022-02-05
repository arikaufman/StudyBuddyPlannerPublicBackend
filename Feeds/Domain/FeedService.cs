using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Feeds.DataAccess;
using plannerBackEnd.Feeds.Domain;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using plannerBackEnd.Users.Domain;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Subjects.Domain
{
    public class FeedService : IFeedService
    {
        private readonly IFeedDataAccessor feedDataAccessor;
        private readonly IUserProfileService userProfileService;
        private readonly RequestContext requestContext;

        // -----------------------------------------------------------------------------

        public FeedService(
            IFeedDataAccessor feedDataAccessor,
            IUserProfileService userProfileService,
            RequestContext requestContext
        )
        {
            this.feedDataAccessor = feedDataAccessor;
            this.userProfileService = userProfileService;
            this.requestContext = requestContext;
        }

        // -----------------------------------------------------------------------------

        public Feed Get(int requestedId)
        {
            return feedDataAccessor.Get(requestedId);
        }

        // -----------------------------------------------------------------------------

        public Feed GetReferenceId(FeedFilterRequest filter)
        {
            return feedDataAccessor.GetReferenceId(filter);
        }

        // -----------------------------------------------------------------------------

        public List<Feed> GetList(FeedFilterRequest filter)
        {
            UserProfile userProfile = userProfileService.Get(filter.UserId);
            List<Feed> feedItems = feedDataAccessor.GetList(filter);
            foreach (Feed feed in feedItems)
            {
                //if user is not premium, make premium changes to best day.
                if (feed.UserId == filter.UserId)
                {
                    feed.GeneralDescription = feed.SelfDescription;
                }

                
                TimeSpan timeSpan = DateTime.UtcNow - feed.RowCreated;
                double minutes = timeSpan.TotalMinutes;
                if (minutes < 60)
                {
                    feed.FeedTime = Math.Floor(minutes).ToString();
                    feed.FeedUnit = "minutes ago";
                }

                if (minutes >= 60 && minutes < 120)
                {
                    feed.FeedTime = Math.Floor(minutes / 60).ToString();
                    feed.FeedUnit = "hour ago";
                }
                else if (minutes >= 120)
                {
                    feed.FeedTime = Math.Floor(minutes / 60).ToString();
                    feed.FeedUnit = "hours ago";
                }


                if (minutes > 1440 && minutes < 2880)
                {
                    feed.FeedTime = Math.Floor(minutes / 1440).ToString();
                    feed.FeedUnit = "day ago";
                }
                else if (minutes >= 2880)
                {
                    feed.FeedTime = Math.Floor(minutes / 1440).ToString();
                    feed.FeedUnit = "days ago";
                }
            }

            return feedItems;
        }

        // -----------------------------------------------------------------------------

        public Feed Create(Feed feed)
        {
            return feedDataAccessor.Create(feed);
        }

        // -----------------------------------------------------------------------------

        public Feed Update(Feed feed)
        {
            return feedDataAccessor.Update(feed);
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int requestedId)
        {
            return feedDataAccessor.Delete(requestedId);

        }


        // -----------------------------------------------------------------------------

        public bool DeleteReferenceItem(int referenceId, string displayType)
        {
            return feedDataAccessor.DeleteReferenceItem(referenceId, displayType);
        }
    }
}