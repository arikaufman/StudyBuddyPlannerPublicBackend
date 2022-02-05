using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Tasks.Controllers.Dto;
using plannerBackEnd.Tasks.Domain;
using plannerBackEnd.Tasks.Domain.DomainObjects;
using System.Collections.Generic;
using plannerBackEnd.Common;

namespace plannerBackEnd.Tasks.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskSessionsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITaskSessionService taskSessionService;

        // -----------------------------------------------------------------------------

        public TaskSessionsController(IMapper mapper, ITaskSessionService taskSessionService)
        {
            this.mapper = mapper;
            this.taskSessionService = taskSessionService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/tasksessions/6
        // Get by id
        [HttpGet("{id}")]
        public TaskSessionDto Get(int id)
        {
            return mapper.Map<TaskSession, TaskSessionDto>(taskSessionService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/tasksessions/list
        [HttpPost("list")]
        public List<TaskSessionDto> GetList([FromBody] int taskId)
        {
            return mapper.Map<List<TaskSession>, List<TaskSessionDto>>
                (taskSessionService.GetList(taskId));
        }

        // -----------------------------------------------------------------------------
        // POST: api/tasksessions/create 
        [HttpPost("create")]
        public TaskSessionDto Create([FromBody] TaskSessionDto taskSessionDto)
        {
            return mapper.Map<TaskSession, TaskSessionDto>(taskSessionService.Create(mapper.Map<TaskSessionDto, TaskSession>(taskSessionDto)));
        }

        // -----------------------------------------------------------------------------
        // PUT: api/tasksessions/3 (UPDATE)
        [HttpPut("{id}")]
        public TaskSessionDto Update(int id, [FromBody] TaskSessionDto taskSessionDto)
        {
            taskSessionDto.Id = id;
            return mapper.Map<TaskSession, TaskSessionDto>(taskSessionService.Update(mapper.Map<TaskSessionDto, TaskSession>(taskSessionDto)));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/tasksessions/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return taskSessionService.Delete(id);
        }

    }
}