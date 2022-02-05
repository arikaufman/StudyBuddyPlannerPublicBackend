using System;
using System.Collections.Generic;
using AutoMapper;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Comparatives.DataAccess.Dao;
using plannerBackEnd.Comparatives.DataAccess.Entities;
using plannerBackEnd.Comparatives.Domain.DomainObjects;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.DataAccess
{
    public class ComparativeDataAccessor : IComparativeDataAccessor
    {
        private readonly IMapper mapper;
        private readonly ComparativeChartsDao comparativeChartsDao;

        // -----------------------------------------------------------------------------

        public ComparativeDataAccessor(IMapper mapper, ComparativeChartsDao comparativeChartsDao)
        {
            this.mapper = mapper;
            this.comparativeChartsDao = comparativeChartsDao;
        }

        //CHARTS
        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListPopulationBreakdown(BaseFilterRequest filter)
        {
            return comparativeChartsDao.GetListPopulationBreakdown(filter);
        }

        // -----------------------------------------------------------------------------
        public List<BaseFilterResponse> GetListHoursPerMonthComparative(BaseFilterRequest filter, DateTime startDate, Subject subject)
        {
            return comparativeChartsDao.GetListHoursPerMonthComparative(filter, startDate, subject);
        }

        // -----------------------------------------------------------------------------
        public List<BestDay> GetListBestDay(BaseFilterRequest filter)
        {
            return mapper.Map<List<BestDayEntity>, List<BestDay>>(comparativeChartsDao.GetListBestDay(filter));
        }

        // -----------------------------------------------------------------------------
        public List<BestAssignment> GetListBestAssignment(BaseFilterRequest filter)
        {
            return mapper.Map<List<BestAssignmentEntity>, List<BestAssignment>>(comparativeChartsDao.GetListBestAssignment(filter));
        }
        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListMarksHoursScatter(BaseFilterRequest filter)
        {
            return comparativeChartsDao.GetListMarksHoursScatter(filter);
        }

        // -----------------------------------------------------------------------------
        public BaseFilterResponse GetListSchoolFacultyScatter(BaseFilterRequest filter)
        {
            return comparativeChartsDao.GetListSchoolFacultyScatter(filter);
        }

    }
}