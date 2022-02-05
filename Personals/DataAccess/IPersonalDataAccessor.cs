using System;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Personals.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.DataAccess
{
    public interface IPersonalDataAccessor
    {
        //CHARTS
        BaseFilterResponse GetListSubjectTotalHours(BaseFilterRequest filter);
        List<BaseFilterResponse> GetListSubjectBreakdown(BaseFilterRequest filter);
        BaseFilterResponse GetListHoursPerWeek(BaseFilterRequest filter);
        List<BaseFilterResponse> GetListHoursPerMonth(BaseFilterRequest filter);
        BaseFilterResponse GetListAverageHoursPerWeek(BaseFilterRequest filter);
        PersonalStatistics GetListPersonalStats(BaseFilterRequest filter);
        List<DetailedView> GetListDetailedView(BaseFilterRequest filter);
        BaseFilterResponse GetListCalendarView(BaseFilterRequest filter);

    }
}