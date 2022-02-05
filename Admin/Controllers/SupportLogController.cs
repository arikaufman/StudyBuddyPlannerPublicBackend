using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Admin.Controllers.Dto;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Admin.Domain.DomainObjects;
using System.Collections.Generic;
using plannerBackEnd.Common;

namespace plannerBackEnd.Admin.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SupportLogController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAdminService adminService;

        // -----------------------------------------------------------------------------

        public SupportLogController(IMapper mapper, IAdminService adminService)
        {
            this.mapper = mapper;
            this.adminService = adminService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/supportlog/6
        // Get by id
        [HttpGet("{id}")]
        public SupportLogDto Get(int id)
        {
            return mapper.Map<SupportLog, SupportLogDto>(adminService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/supportlog/list
        [HttpPost("list")]
        public List<SupportLogDto> GetList()
        {

            return mapper.Map<List<SupportLog>, List<SupportLogDto>>
                (adminService.GetList());
        }

        // -----------------------------------------------------------------------------
        // POST: api/supportlog/create 
        [HttpPost("create")]
        public SupportLogDto Create([FromBody] SupportLogDto supportLogDto)
        {
            return mapper.Map<SupportLog, SupportLogDto>(adminService.Create(mapper.Map<SupportLogDto, SupportLog>(supportLogDto)));
        }

        // -----------------------------------------------------------------------------
        // PUT: api/supportlog/3 (UPDATE)
        [HttpPut("{id}")]
        public SupportLogDto Update(int id, [FromBody] SupportLogDto supportLogDto)
        {
            supportLogDto.Id = id;
            return mapper.Map<SupportLog, SupportLogDto>(adminService.Update(mapper.Map<SupportLogDto, SupportLog>(supportLogDto)));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/supportlog/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return adminService.Delete(id);
        }
    }
}