using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Admin.Controllers.Dto;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common;

namespace plannerBackEnd.Admin.Controllers
{
    [EntryFilter]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAdminService adminService;

        // -----------------------------------------------------------------------------

        public AdminController(IMapper mapper, IAdminService adminService)
        {
            this.mapper = mapper;
            this.adminService = adminService;
        }

        // -----------------------------------------------------------------------------
        // POST: api/admin/createSmokeTest
        [AllowAnonymous]
        [HttpPost("createSmokeTest")]
        public SmokeTest CreateSmokeTest([FromBody]SmokeTestDto smokeTestDto)
        {
            return adminService.CreateSmokeTest(mapper.Map<SmokeTestDto, SmokeTest>(smokeTestDto));
        }

        // -----------------------------------------------------------------------------
        // POST: api/admin/testEmail
        [AllowAnonymous]
        [HttpPost("testEmail")]
        public void TestEmail()
        {
            adminService.SendEmail("AmbassadorEmail", "Gary", "Kaufman", "gskaufman@gmail.com", 
                "new");
        }
    }
}
