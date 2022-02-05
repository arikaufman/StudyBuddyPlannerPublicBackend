using plannerBackEnd.Semesters.Domain;
using plannerBackEnd.Semesters.Domain.DomainObjects;
using plannerBackEnd.Subjects.DataAccess;
using plannerBackEnd.Subjects.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Personals.Domain;

namespace plannerBackEnd.Subjects.Domain
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectDataAccessor subjectDataAccessor;
        private readonly ISemesterService semesterService;
        private readonly IPersonalService personalService;
        private readonly RequestContext requestContext;

        // -----------------------------------------------------------------------------

        public SubjectService(
            ISubjectDataAccessor subjectDataAccessor,
            ISemesterService semesterService,
            IPersonalService personalService,
            RequestContext requestContext
        )
        {
            this.subjectDataAccessor = subjectDataAccessor;
            this.semesterService = semesterService;
            this.personalService = personalService;
            this.requestContext = requestContext;
        }

        // -----------------------------------------------------------------------------

        public Subject Get(int requestedId)
        {
            return subjectDataAccessor.Get(requestedId);
        }

        // -----------------------------------------------------------------------------

        public List<Subject> GetList(SubjectFilterRequest filter)
        {
            List<Subject> subjects = subjectDataAccessor.GetList(filter);

            BaseFilterRequest baseFilter = new BaseFilterRequest(){UserId = filter.UserId};
            List<BaseFilterResponse> responses = personalService.GetListSubjectBreakdown(baseFilter);

            foreach (Subject subject in subjects)
            {
                foreach (BaseFilterResponse response in responses)
                {
                    if (subject.Id.ToString() == response.Title)
                    {
                        subject.SubjectBreakdown = response;
                    }
                }
            }

            return subjects;
        }
        // -----------------------------------------------------------------------------

        public Subject Create(Subject subject)
        {
            Semester semester = semesterService.Get(subject.SemesterId);
            if (semester.Active == 1)
            {
                subject.Active = 1;
            }

            if (requestContext.UserId == subject.UserId)
            {
                return subjectDataAccessor.Create(subject);
            }
            else
            {
                throw new AuthenticationException("Unauthorized to Access User Information");
            }
        }

        // -----------------------------------------------------------------------------

        public Subject Update(Subject subject)
        {
            Semester semester = semesterService.Get(subject.SemesterId);
            if (semester.Active == 1)
            {
                subject.Active = 1;
            }

            if (requestContext.UserId == subject.UserId)
            {
                return subjectDataAccessor.Update(subject);
            }
            else
            {
                throw new AuthenticationException("Unauthorized to Access User Information");
            }
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int requestedId)
        {
            Subject subjectToDelete = Get(requestedId);
            if (requestContext.UserId == subjectToDelete.UserId)
            {
                return (subjectDataAccessor.Delete(requestedId));
            }
            else { throw new AuthenticationException("Unauthorized to Access User Information"); }
        }
    }
}