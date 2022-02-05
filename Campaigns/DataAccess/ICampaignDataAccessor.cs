using System.Collections.Generic;
using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Campaigns.DataAccess
{
    public interface ICampaignDataAccessor
    {
        Campaign Get(int requestedId);
        Campaign GetByUserId(int requestedUserId);
        ReferredUser GetList(BaseFilterRequest filter);
        List<Campaign> CreateList(List<Campaign> campaignsToCreate);
    }
}