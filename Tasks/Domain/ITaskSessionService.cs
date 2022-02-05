using plannerBackEnd.Tasks.Domain.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Tasks.Domain
{
    public interface ITaskSessionService
    {
        TaskSession Get(int requestedId);
        List<TaskSession> GetList(int taskId);
        TaskSession Create(TaskSession taskSession);
        TaskSession Update(TaskSession taskSession);
        bool Delete(int requestedId);

    }
}