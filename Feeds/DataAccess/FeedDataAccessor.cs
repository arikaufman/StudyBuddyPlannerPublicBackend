using AutoMapper;
using plannerBackEnd.Feeds.DataAccess.Dao;
using plannerBackEnd.Feeds.DataAccess.Entities;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Feeds.DataAccess
{
    public class FeedDataAccessor : IFeedDataAccessor
    {
        private readonly IMapper mapper;
        private readonly FeedDao feedDao;


        // -----------------------------------------------------------------------------

        public FeedDataAccessor(IMapper mapper, FeedDao feedDao)
        {
            this.mapper = mapper;
            this.feedDao = feedDao;
        }

        // -----------------------------------------------------------------------------

        public Feed Get(int requestedId)
        {
            return mapper.Map<FeedEntity, Feed>(feedDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public Feed GetReferenceId(FeedFilterRequest filter)
        {
            return mapper.Map<FeedEntity, Feed>(feedDao.GetReferenceId(filter));
        }

        // -----------------------------------------------------------------------------

        public List<Feed> GetList(FeedFilterRequest filter)
        {
            return mapper.Map<List<FeedEntity>, List<Feed>>
                (feedDao.GetList(filter));
        }

        // -----------------------------------------------------------------------------

        public Feed Create(Feed feed)
        {
            return mapper.Map<FeedEntity, Feed>(feedDao.Create(mapper.Map<Feed, FeedEntity>(feed)));
        }

        // -----------------------------------------------------------------------------

        public Feed Update(Feed feed)
        {
            return mapper.Map<FeedEntity, Feed>(feedDao.Update(mapper.Map<Feed, FeedEntity>(feed)));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int feedId)
        {
            return feedDao.Delete(feedId);
        }

        // -----------------------------------------------------------------------------

        public bool DeleteReferenceItem(int referenceId, string displayType)
        {
            return feedDao.DeleteReferenceItem(referenceId, displayType);
        }

    }
}