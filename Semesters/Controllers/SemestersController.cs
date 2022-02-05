using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Semesters.Controllers.Dto;
using plannerBackEnd.Semesters.Domain;
using plannerBackEnd.Semesters.Domain.DomainObjects;
using System.Collections.Generic;
using plannerBackEnd.Common;

namespace plannerBackEnd.Semesters.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SemestersController : Controller
    {
        private readonly IMapper mapper;
        private readonly ISemesterService semesterService;

        // -----------------------------------------------------------------------------

        public SemestersController(IMapper mapper, ISemesterService semesterService)
        {
            this.mapper = mapper;
            this.semesterService = semesterService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/semesters/6
        // Get by id
        [HttpGet("{id}")]
        public SemesterDto Get(int id)
        {
            return mapper.Map<Semester, SemesterDto>(semesterService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/semesters/list
        [HttpPost("list")]
        public List<SemesterDto> GetList([FromBody] SemesterFilterRequestDto filterDto)
        {
            SemesterFilterRequest filter = mapper.Map<SemesterFilterRequestDto, SemesterFilterRequest>(filterDto);

            return mapper.Map<List<Semester>, List<SemesterDto>>
                (semesterService.GetList(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/semesters/create 
        [HttpPost("create")]
        public SemesterDto Create([FromBody] SemesterDto semesterDto)
        {
            return mapper.Map<Semester, SemesterDto>(semesterService.Create(mapper.Map<SemesterDto, Semester>(semesterDto)));
        }

        // -----------------------------------------------------------------------------
        // PUT: api/semesters/3 (UPDATE)
        [HttpPut("{id}")]
        public SemesterDto Update(int id, [FromBody] SemesterDto semesterDto)
        {
            semesterDto.Id = id;
            return mapper.Map<Semester, SemesterDto>(semesterService.Update(mapper.Map<SemesterDto, Semester>(semesterDto)));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/semesters/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return semesterService.Delete(id);
        }
    }
}