using System.Collections.Generic;
using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Campaigns.Domain
{
    public interface ICampaignService
    {
        Campaign Get(int requestedId);
        Campaign GetByUserId(int requestedUserId);
        ReferredUser GetList(BaseFilterRequest filter);
        List<Campaign> CreateList(string campaignName);
    }
}