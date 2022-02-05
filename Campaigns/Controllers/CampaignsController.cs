using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Campaigns.Controllers.Dto;
using plannerBackEnd.Campaigns.Domain;
using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Common;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Common.Filters.Dto;
using plannerBackEnd.Common.Swagger;
using System.Collections.Generic;

namespace plannerBackEnd.Campaigns.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICampaignService campaignService;

        // -----------------------------------------------------------------------------

        public CampaignsController(IMapper mapper, ICampaignService campaignService)
        {
            this.mapper = mapper;
            this.campaignService = campaignService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/campaigns/6
        // Get by id
        [HttpGet("{id}")]
        public CampaignDto Get(int id)
        {
            return mapper.Map<Campaign, CampaignDto>(campaignService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/campaigns/list
        [SwaggerRouteDescription(Description = "REQUIRES: UserId.")]
        [HttpPost("list")]
        public ReferredUserDto GetListDetailedView([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<ReferredUser, ReferredUserDto>(campaignService.GetList(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/campaigns/createlist
        [HttpPost("createlist")]
        public List<CampaignDto> CreateList([FromBody] string campaignName)
        {
            return mapper.Map<List<Campaign>, List<CampaignDto>>
                (campaignService.CreateList(campaignName));
        }
    }
}