using plannerBackEnd.Feeds.Domain.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Feeds.DataAccess
{
    public interface IFeedDataAccessor
    {
        Feed Get(int requestedId);
        Feed GetReferenceId(FeedFilterRequest filter);
        List<Feed> GetList(FeedFilterRequest filter);
        Feed Create(Feed feed);
        Feed Update(Feed feed);
        bool Delete(int requestedId);
        bool DeleteReferenceItem(int referenceId, string displayType);
    }
}