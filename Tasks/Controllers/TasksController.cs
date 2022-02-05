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
    public class TasksController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITaskService taskService;

        // -----------------------------------------------------------------------------

        public TasksController(IMapper mapper, ITaskService taskService)
        {
            this.mapper = mapper;
            this.taskService = taskService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/tasks/6
        // Get by id
        [HttpGet("{id}")]
        public TaskDto Get(int id)
        {
            return mapper.Map<Task, TaskDto>(taskService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // POST: api/tasks/list
        [HttpPost("list")]
        public List<TaskDto> GetList([FromBody] TaskFilterRequestDto filterDto)
        {
            TaskFilterRequest filter = mapper.Map<TaskFilterRequestDto, TaskFilterRequest>(filterDto);

            return mapper.Map<List<Task>, List<TaskDto>>
                (taskService.GetList(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/tasks/create 
        [HttpPost("create")]
        public TaskDto Create([FromBody] TaskDto taskDto)
        {
            return mapper.Map<Task, TaskDto>(taskService.Create(mapper.Map<TaskDto, Task>(taskDto)));
        }

        // -----------------------------------------------------------------------------
        // PUT: api/tasks/3 (UPDATE)
        [HttpPut("{id}")]
        public TaskDto Update(int id, [FromBody] TaskDto taskDto)
        {
            taskDto.Id = id;
            return mapper.Map<Task, TaskDto>(taskService.Update(mapper.Map<TaskDto, Task>(taskDto)));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/tasks/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return taskService.Delete(id);
        }

    }
}