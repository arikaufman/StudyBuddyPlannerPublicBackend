using System;
using System.Collections.Generic;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Comparatives.Domain.DomainObjects;
using plannerBackEnd.Subjects.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.Domain
{
    public interface IComparativeService
    {
        //CHARTS
        BaseFilterResponse GetListPopulationBreakdown(BaseFilterRequest filter);
        List<BaseFilterResponse> GetListHoursPerMonthComparative(BaseFilterRequest filter);
        List<BestDay> GetListBestDay(BaseFilterRequest filter);
        List<BestAssignment> GetListBestAssignment(BaseFilterRequest filter);
        BaseFilterResponse GetListMarksHoursScatter(BaseFilterRequest filter);
        BaseFilterResponse GetListSchoolFacultyScatter(BaseFilterRequest filter);

    }
}