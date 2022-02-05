using plannerBackEnd.Semesters.Domain;
using plannerBackEnd.Tasks.DataAccess;
using plannerBackEnd.Tasks.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Feeds.Domain;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using plannerBackEnd.Subjects.Domain;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Tasks.Domain
{
    public class TaskService : ITaskService
    {
        private readonly ITaskDataAccessor taskDataAccessor;
        private readonly ITaskSessionService taskSessionService;
        private readonly ISemesterService semesterService;
        private readonly ISubjectService subjectService;
        private readonly IFeedService feedService;
        private readonly RequestContext requestContext;

        // -----------------------------------------------------------------------------

        public TaskService(
            ITaskDataAccessor taskDataAccessor,
            ITaskSessionService taskSessionService,
            ISemesterService semesterService,
            ISubjectService subjectService,
            IFeedService feedService,
            RequestContext requestContext
        )
        {
            this.taskDataAccessor = taskDataAccessor;
            this.taskSessionService = taskSessionService;
            this.semesterService = semesterService;
            this.subjectService = subjectService;
            this.feedService = feedService;
            this.requestContext = requestContext;

        }

        // -----------------------------------------------------------------------------

        public Task Get(int requestedId)
        {
            Task returnTask = taskDataAccessor.Get(requestedId);
            returnTask.TotalTime = TimeSpan.FromMinutes(returnTask.Minutes);
            returnTask.TaskSessions = taskSessionService.GetList(returnTask.Id);

            if (requestContext.UserId == returnTask.UserId) {
                return returnTask;
            }
            else { throw new AuthenticationException("Unauthorized to Access User Information");}
        }

        // -----------------------------------------------------------------------------

        public List<Task> GetList(TaskFilterRequest filter)
        {
            List<Task> returnTasks = taskDataAccessor.GetList(filter);
            foreach (Task task in returnTasks)
            {
                task.TaskSessions = taskSessionService.GetList(task.Id);
                task.TotalTime = TimeSpan.FromMinutes(task.Minutes);
            }

            if (requestContext.UserId == filter.UserId)
            {
                return returnTasks;
            }
            else { throw new AuthenticationException("Unauthorized to Access User Information"); }
        }

        // -----------------------------------------------------------------------------

        public Task Create(Task task)
        {
            Subject subject = subjectService.Get(task.SubjectId);
            if (subject.Active == 1)
            {
                task.Active = 1;
            }

            if (requestContext.UserId == task.UserId)
            {
                return taskDataAccessor.Create(task);
            }
            else { throw new AuthenticationException("Unauthorized to Access User Information"); }
        }

        // -----------------------------------------------------------------------------

        public Task Update(Task task)
        {
            Subject subject = subjectService.Get(task.SubjectId);
            if (subject.Active == 1)
            {
                task.Active = 1;
            }

            if (requestContext.UserId == task.UserId)
            {
                Task returnTask = taskDataAccessor.Update(task);
                returnTask.TotalTime = TimeSpan.FromMinutes(returnTask.Minutes);

                //for feed
                if (task.IsDone == 1 && task.Minutes > 180)
                {
                    Feed feed = new Feed()
                    {
                        UserId = requestContext.UserId,
                        ReferenceId = task.Id,
                        DisplayType = "taskcompleted",
                        Visibility = 1
                    };
                    feedService.Create(feed);
                }
                else if (task.IsDone == 0)
                {
                    bool feedDeletion = feedService.DeleteReferenceItem(task.Id, "taskcompleted");
                }

                return returnTask;

            }
            else { throw new AuthenticationException("Unauthorized to Access User Information"); }
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int requestedId)
        {
            Task taskToDelete = Get(requestedId);
            if (requestContext.UserId == taskToDelete.UserId)
            {
                return (taskDataAccessor.Delete(requestedId));
            }
            else { throw new AuthenticationException("Unauthorized to Access User Information"); }
        }
    }
}