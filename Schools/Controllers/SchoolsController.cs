using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Schools.Controllers.Dto;
using plannerBackEnd.Schools.Domain;
using System.Collections.Generic;
using plannerBackEnd.Common;

namespace plannerBackEnd.Schools.Controllers
{
    [EntryFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ISchoolService schoolService;

        // -----------------------------------------------------------------------------

        public SchoolsController(IMapper mapper, ISchoolService schoolService)
        {
            this.mapper = mapper;
            this.schoolService = schoolService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/schools/6
        // Get by id
        [HttpGet("{id}")]
        public SchoolDto Get(int id)
        {
            return mapper.Map<School, SchoolDto>(schoolService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/schools/list
        [HttpPost("list")]
        public List<SchoolDto> GetList()
        {

            return mapper.Map<List<School>, List<SchoolDto>>
                (schoolService.GetList());
        }
    }
}