using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common;
using plannerBackEnd.Subjects.Controllers.Dto;
using plannerBackEnd.Subjects.Domain;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Subjects.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ISubjectService subjectService;

        // -----------------------------------------------------------------------------

        public SubjectsController(IMapper mapper, ISubjectService subjectService)
        {
            this.mapper = mapper;
            this.subjectService = subjectService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/subjects/6
        // Get by id
        [HttpGet("{id}")]
        public SubjectDto Get(int id)
        {
            return mapper.Map<Subject, SubjectDto>(subjectService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/subjects/list
        [HttpPost("list")]
        public List<SubjectDto> GetList([FromBody] SubjectFilterRequestDto filterDto)
        {
            SubjectFilterRequest filter = mapper.Map<SubjectFilterRequestDto, SubjectFilterRequest>(filterDto);

            return mapper.Map<List<Subject>, List<SubjectDto>>
                (subjectService.GetList(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/subjects/create
        [HttpPost("create")]
        public SubjectDto Create([FromBody] SubjectDto subjectDto)
        {
            return mapper.Map<Subject, SubjectDto>(subjectService.Create(mapper.Map<SubjectDto, Subject>(subjectDto)));
        }

        // -----------------------------------------------------------------------------
        // PUT: api/subjects/3 (UPDATE)
        [HttpPut("{id}")]
        public SubjectDto Update(int id, [FromBody] SubjectDto subjectDto)
        {
            subjectDto.Id = id;
            return mapper.Map<Subject, SubjectDto>(subjectService.Update(mapper.Map<SubjectDto, Subject>(subjectDto)));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/subjects/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return subjectService.Delete(id);
        }

    }
}