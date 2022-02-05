using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Common.Filters.Dto;
using plannerBackEnd.Common.Swagger;
using plannerBackEnd.Comparatives.Controllers.Dto;
using plannerBackEnd.Comparatives.Domain;
using plannerBackEnd.Comparatives.Domain.DomainObjects;

namespace plannerBackEnd.Comparatives.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ComparativeChartsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IComparativeService comparativeService;

        // -----------------------------------------------------------------------------

        public ComparativeChartsController(IMapper mapper, IComparativeService comparativeService)
        {
            this.mapper = mapper;
            this.comparativeService = comparativeService;
        }

        // -----------------------------------------------------------------------------
        // POST: api/comparativecharts/populationbreakdown
        [SwaggerRouteDescriptionAttribute(Description = "REQUIRES: userid, breakdowntype. BREAKDOWN TYPE 1 = Faculty, BREAKDOWN TYPE 2 = Year.")]
        [HttpPost("populationbreakdown")]
        public BaseFilterResponseDto GetListPopulationBreakdown([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(comparativeService.GetListPopulationBreakdown(filter));
        }

        // ---------------------------------------------------------------------------
        // POST: api/comparativecharts/listhourspermonthcomparative
        [SwaggerRouteDescription(Description = "REQUIRES: SchoolId, subjectId, UserId.")]
        [HttpPost("listhourspermonthcomparative")]
        public List<BaseFilterResponseDto> GetListHoursPerMonth([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<List<BaseFilterResponse>, List<BaseFilterResponseDto>>(comparativeService.GetListHoursPerMonthComparative(filter));
        }

        // ---------------------------------------------------------------------------
        // POST: api/comparativecharts/listbestdays
        [SwaggerRouteDescription(Description = "REQUIRES: UserId. OPTIONAL: Personal.")]
        [HttpPost("listbestdays")]
        public List<BestDayDto> GetListBestDay([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<List<BestDay>, List<BestDayDto>>(comparativeService.GetListBestDay(filter));
        }

        // ---------------------------------------------------------------------------
        // POST: api/comparativecharts/listbestassignments
        [SwaggerRouteDescription(Description = "REQUIRES: UserId. OPTIONAL: Personal.")]
        [HttpPost("listbestassignments")]
        public List<BestAssignmentDto> GetListBestAssignment([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<List<BestAssignment>, List<BestAssignmentDto>>(comparativeService.GetListBestAssignment(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/comparativecharts/markshoursscatter
        [SwaggerRouteDescriptionAttribute(Description = "REQUIRES: userid")]
        [HttpPost("markshoursscatter")]
        public BaseFilterResponseDto GetListMarksHoursScatter([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(comparativeService.GetListMarksHoursScatter(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/comparativecharts/schoolfacultyscatter
        [ApiExplorerSettings(IgnoreApi = true)]
        [SwaggerRouteDescriptionAttribute(Description = "NOT CURRENTLY USABLE.")]
        [HttpPost("schoolfacultyscatter")]
        public BaseFilterResponseDto GetListSchoolFacultyScatter([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(comparativeService.GetListSchoolFacultyScatter(filter));
        }
    }
}