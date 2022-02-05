using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common;
using plannerBackEnd.Users.Controllers.Dto;
using plannerBackEnd.Users.Domain;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserActivityController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUserActivityService userActivityService;

        // -----------------------------------------------------------------------------

        public UserActivityController(IMapper mapper, IUserActivityService userActivityService)
        {
            this.mapper = mapper;
            this.userActivityService = userActivityService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/useractivity/6
        // Get by id
        [HttpGet("{id}")]
        public UserActivityDto Get(int id)
        {
            return mapper.Map<UserActivity, UserActivityDto>(userActivityService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/useractivity/count
        [HttpPost("count")]
        public Dictionary<string, int> GetCount([FromBody] UserActivityFilterRequestDto filterDto)
        {
            UserActivityFilterRequest filter = mapper.Map<UserActivityFilterRequestDto, UserActivityFilterRequest>(filterDto);

            return userActivityService.GetCount(filter);
        }

        // -----------------------------------------------------------------------------
        // POST: api/useractivity/create
        [HttpPost("create")]
        public UserActivityDto Create([FromBody] UserActivityDto userActivityDto)
        {
            return mapper.Map<UserActivity, UserActivityDto>(userActivityService.Create(mapper.Map<UserActivityDto, UserActivity>(userActivityDto)));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/useractivity/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return userActivityService.Delete(id);
        }

    }
}