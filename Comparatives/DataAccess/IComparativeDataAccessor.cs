using System;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Comparatives.Domain.DomainObjects;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.DataAccess
{
    public interface IComparativeDataAccessor
    {

        //CHARTS
        BaseFilterResponse GetListPopulationBreakdown(BaseFilterRequest filter);
        List<BaseFilterResponse> GetListHoursPerMonthComparative(BaseFilterRequest filter, DateTime startDate, Subject subject);
        List<BestDay> GetListBestDay(BaseFilterRequest filter);
        List<BestAssignment> GetListBestAssignment(BaseFilterRequest filter);
        BaseFilterResponse GetListMarksHoursScatter(BaseFilterRequest filter);
        BaseFilterResponse GetListSchoolFacultyScatter(BaseFilterRequest filter);

    }
}