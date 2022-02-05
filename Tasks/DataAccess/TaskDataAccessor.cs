using System;
using AutoMapper;
using plannerBackEnd.Tasks.DataAccess.Dao;
using plannerBackEnd.Tasks.DataAccess.Entities;
using plannerBackEnd.Tasks.Domain.DomainObjects;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Subjects.DataAccess.Dao;

namespace plannerBackEnd.Tasks.DataAccess
{
    public class TaskDataAccessor : ITaskDataAccessor
    {
        private readonly IMapper mapper;
        private readonly TaskDao taskDao;

        // -----------------------------------------------------------------------------

        public TaskDataAccessor(IMapper mapper, TaskDao taskDao)
        {
            this.mapper = mapper;
            this.taskDao = taskDao;
        }

        // -----------------------------------------------------------------------------

        public Task Get(int requestedId)
        {
            return mapper.Map<TaskEntity, Task>(taskDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public List<Task> GetList(TaskFilterRequest filter)
        {
            return mapper.Map<List<TaskEntity>, List<Task>>
                (taskDao.GetList(filter));
        }

        // -----------------------------------------------------------------------------

        public Task Create(Task task)
        {
            return mapper.Map<TaskEntity, Task>(taskDao.Create(mapper.Map<Task, TaskEntity>(task)));
        }

        // -----------------------------------------------------------------------------

        public Task Update(Task task)
        {
            return mapper.Map<TaskEntity, Task>(taskDao.Update(mapper.Map<Task, TaskEntity>(task)));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int subjectId)
        {
            return taskDao.Delete(subjectId);
        }
    }
}