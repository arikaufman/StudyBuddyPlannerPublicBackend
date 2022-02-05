using plannerBackEnd.Common.DomainObjects;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Comparatives.DataAccess;
using plannerBackEnd.Feeds.Domain;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using plannerBackEnd.Personals.Domain.DomainObjects;
using plannerBackEnd.Semesters.Domain;
using plannerBackEnd.Semesters.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace plannerBackEnd.Personals.Domain
{
    public class PersonalService : IPersonalService
    {
        private readonly IPersonalDataAccessor personalDataAccessor;
        private readonly ISemesterService semesterService;
        private readonly IFeedService feedService;
        private readonly RequestContext requestContext;
        // -----------------------------------------------------------------------------

        public PersonalService(
            IPersonalDataAccessor personalDataAccessor,
            ISemesterService semesterService,
            IFeedService feedService,
            RequestContext requestContext
        )
        {
            this.personalDataAccessor = personalDataAccessor;
            this.semesterService = semesterService;
            this.feedService = feedService;
            this.requestContext = requestContext;
        }

        //CHARTS
        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListSubjectTotalHours(BaseFilterRequest filter)
        {
            SemesterFilterRequest semesterFilter = new SemesterFilterRequest() { UserId = filter.UserId };

            List<Semester> semesters = semesterService.GetList(semesterFilter);
            if (filter.SemesterId == 0)
            {
                filter.SemesterId = semesters.FirstOrDefault().Id;
            }

            BaseFilterResponse response = personalDataAccessor.GetListSubjectTotalHours(filter);
            foreach (BaseFilterResponseItem item in response.ResponseItems)
            {
                item.Value1 = item.Value1 / 60;
            }

            return response;
        }

        // -----------------------------------------------------------------------------
        public List<BaseFilterResponse> GetListSubjectBreakdown(BaseFilterRequest filter)
        {
            List<BaseFilterResponse> response = personalDataAccessor.GetListSubjectBreakdown(filter);
            foreach (BaseFilterResponse responseItem in response)
            {
                foreach (BaseFilterResponseItem item in responseItem.ResponseItems)
                {
                    item.Value1 = item.Value1 / 60;
                }
            }

            return response;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListHoursPerWeek(BaseFilterRequest filter)
        {
            BaseFilterResponse response = personalDataAccessor.GetListHoursPerWeek(filter);
            foreach (BaseFilterResponseItem item in response.ResponseItems)
            {
                item.Value1 = item.Value1 / 60;
            }

            // for feed
            FeedFilterRequest feedFilter = new FeedFilterRequest()
            {
                CurrentTime = DateTime.Today.AddDays(2),
                DisplayType = "streak",
                ReferenceId = requestContext.UserId
            };

            if (feedService.GetReferenceId(feedFilter).GeneralDescription == "")
            {
                if ((Int32.Parse(response.Title) % 5 == 0) && (Int32.Parse(response.Title) > 0))
                {
                    Feed feed = new Feed()
                    {
                        UserId = requestContext.UserId,
                        ReferenceId = requestContext.UserId,
                        GeneralDescription = response.Title,
                        DisplayType = "streak",
                        Visibility = 1
                    };
                    Feed returnFeed = feedService.Create(feed);
                }
            }

            return response;
        }

        // -----------------------------------------------------------------------------

        public List<BaseFilterResponse> GetListHoursPerMonth(BaseFilterRequest filter)
        {
            List<BaseFilterResponse> response = new List<BaseFilterResponse>();

            SemesterFilterRequest semesterFilter = new SemesterFilterRequest() { UserId = filter.UserId };

            List<Semester> semesters = semesterService.GetList(semesterFilter);
            foreach (Semester semester in semesters)
            {
                if (semester.EndDate > DateTime.Now && semester.StartDate < DateTime.Now)
                    if (filter.SemesterId == 0)
                    {
                        filter.SemesterId = semesters.FirstOrDefault().Id;
                    }

                response = personalDataAccessor.GetListHoursPerMonth(filter);
            }

            foreach (BaseFilterResponse responseItem in response)
            {
                foreach (BaseFilterResponseItem item in responseItem.ResponseItems)
                {
                    item.Value1 = item.Value1 / 60;
                }
            }

            return response;
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListAverageHoursPerWeek(BaseFilterRequest filter)
        {
            BaseFilterResponse response = personalDataAccessor.GetListAverageHoursPerWeek(filter);
            foreach (BaseFilterResponseItem item in response.ResponseItems)
            {
                item.Value1 = item.Value1 / 60;
            }

            return response;
        }

        // -----------------------------------------------------------------------------

        public PersonalStatistics GetListPersonalStats(BaseFilterRequest filter)
        {
            PersonalStatistics response = personalDataAccessor.GetListPersonalStats(filter);
            // for feed
            FeedFilterRequest feedFilter = new FeedFilterRequest()
            {
                CurrentTime = DateTime.Today,
                DisplayType = "fiveHoursSpent",
                ReferenceId = requestContext.UserId
            };

            if (feedService.GetReferenceId(feedFilter).GeneralDescription == "")
            {
                if (response.CurrentDay > 300)
                {
                    Feed feed = new Feed()
                    {
                        UserId = requestContext.UserId,
                        ReferenceId = requestContext.UserId,
                        GeneralDescription = Math.Floor(response.CurrentDay / 60).ToString() + "hrs., " + (response.CurrentDay % 60).ToString() + "min." ,
                        DisplayType = "fiveHoursSpent",
                        Visibility = 1
                    };
                    Feed returnFeed = feedService.Create(feed);
                }
            }

            return response;
        }

        // -----------------------------------------------------------------------------

        public List<DetailedView>  GetListDetailedView(BaseFilterRequest filter)
        {
            List<DetailedView> response = personalDataAccessor.GetListDetailedView(filter);

            return response;
        }

        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListCalendarView(BaseFilterRequest filter)
        {
            BaseFilterResponse response = personalDataAccessor.GetListCalendarView(filter);
            foreach (BaseFilterResponseItem item in response.ResponseItems)
            {
                item.Name1 = item.Date1.ToString("yyyy-MM-dd");
                item.Date1 = DateTime.MinValue;
            }
            return response;
        }
    }
}