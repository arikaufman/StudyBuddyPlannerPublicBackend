using System;
using plannerBackEnd.Tasks.Domain.DomainObjects;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Tasks.DataAccess
{
    public interface ITaskDataAccessor
    {
        Task Get(int requestedId);
        List<Task> GetList(TaskFilterRequest filter);
        Task Create(Task task);
        Task Update(Task task);
        bool Delete(int requestedId);
    }
}