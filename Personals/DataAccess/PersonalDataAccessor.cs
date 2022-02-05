using System;
using System.Collections.Generic;
using AutoMapper;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Personals.DataAccess.Dao;
using plannerBackEnd.Personals.DataAccess.Entities;
using plannerBackEnd.Personals.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.DataAccess
{
    public class PersonalDataAccessor : IPersonalDataAccessor
    {
        private readonly IMapper mapper;
        private readonly PersonalChartsDao personalChartsDao;

        // -----------------------------------------------------------------------------

        public PersonalDataAccessor(IMapper mapper, PersonalChartsDao personalChartsDao)
        {
            this.mapper = mapper;
            this.personalChartsDao = personalChartsDao;
        }

        //CHARTS
        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListSubjectTotalHours(BaseFilterRequest filter)
        {
            return personalChartsDao.GetListSubjectTotalHours(filter);
        }

        // -----------------------------------------------------------------------------

        public List<BaseFilterResponse> GetListSubjectBreakdown(BaseFilterRequest filter)
        {
            return personalChartsDao.GetListSubjectBreakdown(filter);
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListHoursPerWeek(BaseFilterRequest filter)
        {
            return personalChartsDao.GetListHoursPerWeek(filter);
        }

        // -----------------------------------------------------------------------------

        public List<BaseFilterResponse> GetListHoursPerMonth(BaseFilterRequest filter)
        {
            return personalChartsDao.GetListHoursPerMonth(filter);
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListAverageHoursPerWeek(BaseFilterRequest filter)
        {
            return personalChartsDao.GetListAverageHoursPerWeek(filter);
        }

        // -----------------------------------------------------------------------------

        public PersonalStatistics GetListPersonalStats(BaseFilterRequest filter)
        {
            return mapper.Map<PersonalStatisticsEntity, PersonalStatistics>
                (personalChartsDao.GetListPersonalStats(filter));
        }

        // -----------------------------------------------------------------------------

        public List<DetailedView> GetListDetailedView(BaseFilterRequest filter)
        {
            return mapper.Map<List<DetailedViewEntity>, List<DetailedView>>
                (personalChartsDao.GetListDetailedView(filter));
        }

        // -----------------------------------------------------------------------------

        public BaseFilterResponse GetListCalendarView(BaseFilterRequest filter)
        {
            return personalChartsDao.GetListCalendarView(filter);
        }
    }
}