using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Admin.Controllers.Dto;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Common.Filters.Dto;
using plannerBackEnd.Common.Swagger;
using plannerBackEnd.Users.Controllers.Dto;

namespace plannerBackEnd.Admin.Controllers
{
    [EntryFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminChartsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAdminService adminService;

        // -----------------------------------------------------------------------------

        public AdminChartsController(IMapper mapper, IAdminService adminService)
        {
            this.mapper = mapper;
            this.adminService = adminService;
        }

        // -----------------------------------------------------------------------------
        // POST: api/admincharts/listadminstats
        [SwaggerRouteDescription(Description = "NO BODY")]
        [HttpPost("listadminstats")]
        public AdminStatisticsDto GetListAdminStats()
        {
            return mapper.Map<AdminStatistics, AdminStatisticsDto>(adminService.GetListAdminStats());
        }

        // -----------------------------------------------------------------------------
        // POST: api/admincharts/listusers
        [SwaggerRouteDescription(Description = "NO BODY")]
        [HttpPost("listusers")]
        public List<UserAnalysisDto> GetListUsers()
        {
            return mapper.Map<List<UserAnalysis>, List<UserAnalysisDto>>(adminService.GetListUsers());
        }

        // -----------------------------------------------------------------------------
        // POST: api/admincharts/listnewusers
        [SwaggerRouteDescription(Description = "NO BODY")]
        [HttpPost("listnewusers")]
        public BaseFilterResponseDto GetListNewUsers()
        {
            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(adminService.GetListNewUsers());
        }
    }
}