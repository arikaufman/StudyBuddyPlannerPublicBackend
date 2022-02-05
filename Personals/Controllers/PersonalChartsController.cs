using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Common.Filters.Dto;
using plannerBackEnd.Common.Swagger;
using plannerBackEnd.Personals.Controllers.Dto;
using plannerBackEnd.Personals.Domain;
using plannerBackEnd.Personals.Domain.DomainObjects;

namespace plannerBackEnd.Personals.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalChartsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPersonalService personalService;

        // -----------------------------------------------------------------------------

        public PersonalChartsController(IMapper mapper, IPersonalService personalService)
        {
            this.mapper = mapper;
            this.personalService = personalService;
        }

        // -----------------------------------------------------------------------------
        // POST: api/personalcharts/listsubjecttotalhours
        [SwaggerRouteDescriptionAttribute(Description = "REQUIRES: UserId.")]
        [HttpPost("listsubjecttotalhours")]
        public BaseFilterResponseDto GetListSubjectTotalHours([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(personalService.GetListSubjectTotalHours(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/personalcharts/listsubjectbreakdown
        [SwaggerRouteDescription(Description = "REQUIRES: UserId, SubjectId.")]
        [HttpPost("listsubjectbreakdown")]
        public List<BaseFilterResponseDto> GetListSubjectBreakdown([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<List<BaseFilterResponse>, List<BaseFilterResponseDto>>(personalService.GetListSubjectBreakdown(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/personalcharts/listhoursperweek
        [SwaggerRouteDescription(Description = "REQUIRES: UserId, Date.")]
        [HttpPost("listhoursperweek")]
        public BaseFilterResponseDto GetListHoursPerWeek([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(personalService.GetListHoursPerWeek(filter));
        }

        // ---------------------------------------------------------------------------
        // POST: api/personalcharts/listhourspermonth
        [AllowAnonymous]
        [SwaggerRouteDescription(Description = "REQUIRES: UserId.")]
        [HttpPost("listhourspermonth")]
        public List<BaseFilterResponseDto> GetListHoursPerMonth([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<List<BaseFilterResponse>, List<BaseFilterResponseDto>>(personalService.GetListHoursPerMonth(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/personalcharts/listaveragehoursperweek
        [SwaggerRouteDescription(Description = "REQUIRES: UserId, Date.")]
        [HttpPost("listaveragehoursperweek")]
        public BaseFilterResponseDto GetListAverageHoursPerWeek([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(personalService.GetListAverageHoursPerWeek(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/personalcharts/listpersonalstats
        [SwaggerRouteDescription(Description = "REQUIRES: UserId, Date.")]
        [HttpPost("listpersonalstats")]
        public PersonalStatisticsDto GetListPersonalStats([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<PersonalStatistics, PersonalStatisticsDto>(personalService.GetListPersonalStats(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/personalcharts/listdetailedview
        [SwaggerRouteDescription(Description = "REQUIRES: UserId, Tasktype.")]
        [HttpPost("listdetailedview")]
        public List<DetailedViewDto> GetListDetailedView([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<List<DetailedView>, List<DetailedViewDto>>(personalService.GetListDetailedView(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/personalcharts/listcalendarview
        [SwaggerRouteDescription(Description = "REQUIRES: UserId.")]
        [HttpPost("listcalendarview")]
        public BaseFilterResponseDto GetListCalendarView([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(personalService.GetListCalendarView(filter));
        }

    }
}