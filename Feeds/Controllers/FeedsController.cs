using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common;
using plannerBackEnd.Feeds.Controllers.Dto;
using plannerBackEnd.Feeds.Domain;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using plannerBackEnd.Friends.Controllers.Dto;
using System.Collections.Generic;

namespace plannerBackEnd.Feeds.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FeedsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IFeedService feedService;

        // -----------------------------------------------------------------------------

        public FeedsController(IMapper mapper, IFeedService feedService)
        {
            this.mapper = mapper;
            this.feedService = feedService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/feeds/6
        // Get by id
        [HttpGet("{id}")]
        public FeedDto Get(int id)
        {
            return mapper.Map<Feed, FeedDto>(feedService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/feeds/list
        [HttpPost("list")]
        public List<FeedDto> GetList([FromBody] FeedFilterRequestDto filterDto)
        {
            FeedFilterRequest filter = mapper.Map<FeedFilterRequestDto, FeedFilterRequest>(filterDto);

            return mapper.Map<List<Feed>, List<FeedDto>>(feedService.GetList(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/feeds/create 
        [HttpPost("create")]
        public FeedDto Create([FromBody] FeedDto feedDto)
        {
            return mapper.Map<Feed, FeedDto>(feedService.Create(mapper.Map<FeedDto, Feed>(feedDto)));
        }

        // -----------------------------------------------------------------------------
        // PUT: api/feeds/3 (UPDATE)
        [HttpPut("{id}")]
        public FeedDto Update(int id, [FromBody] FeedDto feedDto)
        {
            feedDto.Id = id;
            return mapper.Map<Feed, FeedDto>(feedService.Update(mapper.Map<FeedDto, Feed>(feedDto)));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/subjects/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return feedService.Delete(id);
        }

    }
}