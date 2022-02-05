using System;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Comparatives.DataAccess;
using plannerBackEnd.Comparatives.Domain.DomainObjects;
using plannerBackEnd.Feeds.Domain;
using plannerBackEnd.Feeds.Domain.DomainObjects;
using plannerBackEnd.Semesters.Domain;
using plannerBackEnd.Semesters.Domain.DomainObjects;
using plannerBackEnd.Subjects.Domain;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.Domain
{
    public class ComparativeService : IComparativeService
    {
        private readonly IComparativeDataAccessor comparativeDataAccessor;
        private readonly IFeedService feedService;
        private readonly ISemesterService semesterService;
        private readonly ISubjectService subjectService;

        // -----------------------------------------------------------------------------

        public ComparativeService(
            IComparativeDataAccessor comparativeDataAccessor,
            IFeedService feedService,
            ISemesterService semesterService,
            ISubjectService subjectService
        )
        {
            this.comparativeDataAccessor = comparativeDataAccessor;
            this.feedService = feedService;
            this.semesterService = semesterService;
            this.subjectService = subjectService;
        }

        //CHARTS
        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListPopulationBreakdown(BaseFilterRequest filter)
        {
            BaseFilterResponse response = comparativeDataAccessor.GetListPopulationBreakdown(filter);
            return response;
        }

        // -----------------------------------------------------------------------------
        public List<BaseFilterResponse> GetListHoursPerMonthComparative(BaseFilterRequest filter)
        {
            List<BaseFilterResponse> response = new List<BaseFilterResponse>();

            SemesterFilterRequest semesterFilter = new SemesterFilterRequest() { UserId = filter.UserId };
            List<Semester> semesters = semesterService.GetList(semesterFilter);

            Subject subject = subjectService.Get(filter.SubjectId);
            foreach (Semester semester in semesters)
            {
                if (semester.EndDate > DateTime.Now && semester.StartDate < DateTime.Now)
                    response = comparativeDataAccessor.GetListHoursPerMonthComparative(filter, semester.StartDate, subject);
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
        public List<BestDay> GetListBestDay(BaseFilterRequest filter)
        {
            List<BestDay> bestDays = comparativeDataAccessor.GetListBestDay(filter);
            foreach (BestDay bestDay in bestDays)
            {
                //for feed
                if (bestDay.BestDayDate == DateTime.Now.Date)
                {
                    
                    FeedFilterRequest feedFilter = new FeedFilterRequest()
                    {
                        CurrentTime = DateTime.Today,
                        DisplayType = "bestday",
                        ReferenceId = bestDay.Id
                    };

                    if (feedService.GetReferenceId(feedFilter).GeneralDescription == "")
                    {
                        Feed feed = new Feed()
                            {
                                UserId = bestDay.Id,
                                ReferenceId = bestDay.Id,
                                GeneralDescription = Math.Floor(bestDay.Minutes / 60).ToString() + "hrs., " + (bestDay.Minutes % 60).ToString() + "min.",
                                DisplayType = "bestday",
                                Visibility = 1
                            };
                        Feed returnFeed = feedService.Create(feed);
                        
                    }
                }
            }
            return comparativeDataAccessor.GetListBestDay(filter);
        }
        // -----------------------------------------------------------------------------
        public List<BestAssignment> GetListBestAssignment(BaseFilterRequest filter)
        {

            return comparativeDataAccessor.GetListBestAssignment(filter); ;
        }
        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListMarksHoursScatter(BaseFilterRequest filter)
        {
            BaseFilterResponse response = comparativeDataAccessor.GetListMarksHoursScatter(filter);
            foreach (BaseFilterResponseItem item in response.ResponseItems)
            {
                item.Value2 = item.Value2 / 60;
            }
            return response;
        }

        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListSchoolFacultyScatter(BaseFilterRequest filter)
        {
            return comparativeDataAccessor.GetListSchoolFacultyScatter(filter);
        }
    }
}