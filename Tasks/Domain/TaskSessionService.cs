using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Feeds.Domain;
using plannerBackEnd.Tasks.DataAccess;
using plannerBackEnd.Tasks.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using plannerBackEnd.Feeds.Domain.DomainObjects;

namespace plannerBackEnd.Tasks.Domain
{
    public class TaskSessionService : ITaskSessionService
    {
        private readonly ITaskSessionDataAccessor taskSessionDataAccessor;
        private readonly IFeedService feedService;
        private readonly RequestContext requestContext;

        // -----------------------------------------------------------------------------

        public TaskSessionService(
            ITaskSessionDataAccessor taskSessionDataAccessor,
            IFeedService feedService,
            RequestContext requestContext
        )
        {
            this.taskSessionDataAccessor = taskSessionDataAccessor;
            this.feedService = feedService;
            this.requestContext = requestContext;
        }

        // -----------------------------------------------------------------------------

        public TaskSession Get(int requestedId)
        {
            return taskSessionDataAccessor.Get(requestedId);
        }

        // -----------------------------------------------------------------------------

        public List<TaskSession> GetList(int taskId)
        {

            return taskSessionDataAccessor.GetList(taskId);
        }

        // -----------------------------------------------------------------------------

        public TaskSession Create(TaskSession taskSession)
        {
            if (taskSession.DateCompleted > DateTime.Now)
            {
                throw new SystemException("date completed is past current date.");
            }

            TaskSession createdTaskSession = taskSessionDataAccessor.Create(taskSession);

            if (taskSession.Minutes >= 120)
            {
                //for feed 
                Feed feed = new Feed()
                {
                    UserId = requestContext.UserId,
                    ReferenceId = createdTaskSession.Id,
                    DisplayType = "tasksession",
                    Visibility = 1
                };
                Feed returnFeed = feedService.Create(feed);
            }

            return createdTaskSession;
        }

        // -----------------------------------------------------------------------------

        public TaskSession Update(TaskSession taskSession)
        {
            return taskSessionDataAccessor.Update(taskSession);

        }

        // -----------------------------------------------------------------------------

        public bool Delete(int requestedId)
        {
            //for feed
            bool feedDeletion = feedService.DeleteReferenceItem(requestedId, "tasksession");

            return (taskSessionDataAccessor.Delete(requestedId));

        }

    }
}