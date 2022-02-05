using System;
using plannerBackEnd.Campaigns.DataAccess;
using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Common.DomainObjects;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Users.Domain;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Campaigns.Domain
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignDataAccessor campaignDataAccessor;
        private readonly IUserProfileService userProfileService;
        private readonly RequestContext requestContext;

        // -----------------------------------------------------------------------------

        public CampaignService(
            ICampaignDataAccessor campaignDataAccessor,
            IUserProfileService userProfileService,
            RequestContext requestContext
        )
        {
            this.campaignDataAccessor = campaignDataAccessor;
            this.userProfileService = userProfileService;
            this.requestContext = requestContext;
        }

        // -----------------------------------------------------------------------------

        public Campaign Get(int requestedId)
        {
            return campaignDataAccessor.Get(requestedId);
        }


        // -----------------------------------------------------------------------------

        public Campaign GetByUserId(int requestedUserId)
        {
            return campaignDataAccessor.GetByUserId(requestedUserId);
        }

        // -----------------------------------------------------------------------------

        public ReferredUser GetList(BaseFilterRequest filter)
        {
            ReferredUser response = campaignDataAccessor.GetList(filter);

            response.CampaignCode = GetByUserId(filter.UserId).CampaignCode;

            return response;
        }
        // -----------------------------------------------------------------------------

        public List<Campaign> CreateList(string campaignName)
        {
            UserProfileFilterRequest filter = new UserProfileFilterRequest()
            {
                Active = true
            };

            List<UserProfile> userProfiles = userProfileService.GetList(filter);

            List<Campaign> campaignsToCreate = new List<Campaign>();

            foreach (UserProfile userProfile in userProfiles)
            {
                Campaign newCampaign = new Campaign()
                {
                    UserId = userProfile.Id,
                    CampaignCode = userProfile.Id.ToString() + campaignName,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(2)
                };

                campaignsToCreate.Add(newCampaign);
            }

            return campaignDataAccessor.CreateList(campaignsToCreate);

        }

    }
}