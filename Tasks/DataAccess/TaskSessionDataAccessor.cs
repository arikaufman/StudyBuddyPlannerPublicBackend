using AutoMapper;
using plannerBackEnd.Tasks.DataAccess.Dao;
using plannerBackEnd.Tasks.DataAccess.Entities;
using plannerBackEnd.Tasks.Domain.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Tasks.DataAccess
{
    public class TaskSessionDataAccessor : ITaskSessionDataAccessor
    {
        private readonly IMapper mapper;
        private readonly TaskSessionDao taskSessionDao;

        // -----------------------------------------------------------------------------

        public TaskSessionDataAccessor(IMapper mapper, TaskSessionDao taskSessionDao)
        {
            this.mapper = mapper;
            this.taskSessionDao = taskSessionDao;
        }

        // -----------------------------------------------------------------------------

        public TaskSession Get(int requestedId)
        {
            return mapper.Map<TaskSessionEntity, TaskSession>(taskSessionDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public List<TaskSession> GetList(int taskId)
        {
            return mapper.Map<List<TaskSessionEntity>, List<TaskSession>>
                (taskSessionDao.GetList(taskId));
        }

        // -----------------------------------------------------------------------------

        public TaskSession Create(TaskSession taskSession)
        {
            return mapper.Map<TaskSessionEntity, TaskSession>(taskSessionDao.Create(mapper.Map<TaskSession, TaskSessionEntity>(taskSession)));
        }

        // -----------------------------------------------------------------------------

        public TaskSession Update(TaskSession taskSession)
        {
            return mapper.Map<TaskSessionEntity, TaskSession>(taskSessionDao.Update(mapper.Map<TaskSession, TaskSessionEntity>(taskSession)));
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int subjectId)
        {
            return taskSessionDao.Delete(subjectId);
        }

    }
}