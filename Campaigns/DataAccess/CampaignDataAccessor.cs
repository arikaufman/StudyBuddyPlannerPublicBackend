using AutoMapper;
using plannerBackEnd.Campaigns.DataAccess.Entities;
using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Tasks.DataAccess.Dao;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Campaigns.DataAccess
{
    public class CampaignDataAccessor : ICampaignDataAccessor
    {
        private readonly IMapper mapper;
        private readonly CampaignDao campaignDao;

        // -----------------------------------------------------------------------------

        public CampaignDataAccessor(IMapper mapper, CampaignDao campaignDao)
        {
            this.mapper = mapper;
            this.campaignDao = campaignDao;
        }

        // -----------------------------------------------------------------------------

        public Campaign Get(int requestedId)
        {
            return mapper.Map<CampaignEntity, Campaign>(campaignDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public Campaign GetByUserId(int requestedUserId)
        {
            return mapper.Map<CampaignEntity, Campaign>(campaignDao.GetByUserId(requestedUserId));
        }

        // -----------------------------------------------------------------------------

        public ReferredUser GetList(BaseFilterRequest filter)
        {
            return mapper.Map< ReferredUserEntity, ReferredUser>
                (campaignDao.GetList(filter));
        }

        // -----------------------------------------------------------------------------

        public List<Campaign> CreateList(List<Campaign> campaignsToCreate)
        {
            return mapper.Map<List<CampaignEntity>, List<Campaign>>(campaignDao.CreateList(mapper.Map< List<Campaign>, List<CampaignEntity>>( campaignsToCreate)));
        }

    }
}