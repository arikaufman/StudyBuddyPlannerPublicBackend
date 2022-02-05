using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Schools.Domain;
using plannerBackEnd.Semesters.DataAccess;
using plannerBackEnd.Semesters.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;

namespace plannerBackEnd.Semesters.Domain
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterDataAccessor semesterDataAccessor;
        private readonly ISchoolService schoolService;
        private readonly RequestContext requestContext;

        // -----------------------------------------------------------------------------

        public SemesterService(
            ISemesterDataAccessor semesterDataAccessor,
            ISchoolService schoolService,
            RequestContext requestContext
        )
        {
            this.semesterDataAccessor = semesterDataAccessor;
            this.schoolService = schoolService;
            this.requestContext = requestContext;
        }

        // -----------------------------------------------------------------------------

        public Semester Get(int requestedId)
        {
            Semester semester = semesterDataAccessor.Get(requestedId);
            if (requestContext.UserId == semester.UserId)
            {
                return semester;
            }
            else
            {
                throw new AuthenticationException("Unauthorized to Access User Information");
            }
        }

        // -----------------------------------------------------------------------------

        public List<Semester> GetList(SemesterFilterRequest filter)
        {
            /*if (requestContext.UserId == filter.UserId)
            {
                return semesterDataAccessor.GetList(filter);
            }
            else
            {
                throw new AuthenticationException("Unauthorized to Access User Information");
            }*/
            return semesterDataAccessor.GetList(filter);
        }

        // -----------------------------------------------------------------------------

        public Semester Create(Semester semester)
        {
            SemesterFilterRequest filter = new SemesterFilterRequest(){UserId = semester.UserId};
            List<Semester> semesters = GetList(filter);
            foreach (Semester existingSemester in semesters)
            {
                if (DateTime.Compare(existingSemester.EndDate, semester.StartDate) > 0)
                {
                    throw new InvalidDataException("Semester cannot have given start date, as previous semester is not completed.");
                }

                if (semester.StartDate.DayOfWeek != DayOfWeek.Monday)
                {
                    throw new InvalidDataException("Semester must start on a Monday.");
                }
            }
            /*UserProfile userProfile = userProfileService.Get(semester.UserId);
            if (semester.Startgpa > 0)
            {
                switch (schoolService.Get(userProfile.SchoolId).Region)
                {
                    case "British Columbia":
                        semester.Startpercentage = (14.9846 * semester.Startgpa) + 33.1925;
                        break;
                    default: //for now group rest here
                        semester.Startpercentage = (11.5689 * semester.Startgpa) + 41.8385;
                        break;
                }
            }
            else if (semester.Startpercentage > 0)
            {
                switch (schoolService.Get(userProfile.SchoolId).Region)
                {
                    case "British Columbia":
                        semester.Startgpa = (0.0652783 * semester.Startpercentage) - 2.1065;
                        break;
                    default: //for now group rest here
                        semester.Startgpa = (0.083364 * semester.Startpercentage) - 3.4008;
                        break;
                }
            }

            if (semester.Startgpa > 4)
            {
                semester.Startgpa = 4.0;
            }*/

            if (DateTime.Compare(DateTime.Now, semester.EndDate) < 0 && DateTime.Compare(DateTime.Now, semester.StartDate) > 0)
            {
                semester.Active = 1;
            }

            return semesterDataAccessor.Create(semester);
        }

        // -----------------------------------------------------------------------------

        public Semester Update(Semester semester)
        {
            if (semester.StartDate.DayOfWeek != DayOfWeek.Monday)
            {
                throw new InvalidDataException("Semester must start on a Monday.");
            }

            if (DateTime.Compare(DateTime.Now, semester.EndDate) < 0 && DateTime.Compare(DateTime.Now, semester.StartDate) > 0)
            {
                semester.Active = 1;
            }

            return semesterDataAccessor.Update(semester);
        }

        // -----------------------------------------------------------------------------

        public bool Delete(int requestedId)
        {
            return (semesterDataAccessor.Delete(requestedId));
        }

    }
}