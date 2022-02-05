using System.Collections.Generic;
using plannerBackEnd.Tasks.Domain.DomainObjects;

namespace plannerBackEnd.Tasks.DataAccess
{
    public interface ITaskSessionDataAccessor
    {
        TaskSession Get(int requestedId);
        List<TaskSession> GetList(int taskId);
        TaskSession Create(TaskSession taskSession);
        TaskSession Update(TaskSession taskSession);
        bool Delete(int requestedId);
    }
}