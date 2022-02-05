using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common;
using plannerBackEnd.Users.Controllers.Dto;
using plannerBackEnd.Users.Domain;
using plannerBackEnd.Users.Domain.DomainObjects;
using System.Collections.Generic;

namespace plannerBackEnd.Users.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfilesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUserProfileService userProfileService;

        // -----------------------------------------------------------------------------

        public UserProfilesController(IMapper mapper, IUserProfileService userProfileService)
        {
            this.mapper = mapper;
            this.userProfileService = userProfileService;
        }

        // -----------------------------------------------------------------------------
        // GET: api/userprofiles/6
        // Get by id
        [HttpGet("{id}")]
        public UserProfileDto Get(int id)
        {
            return mapper.Map<UserProfile, UserProfileDto>(userProfileService.Get(id));
        }

        // -----------------------------------------------------------------------------
        // GET: api/userprofiles/akaufman2000@gmail.com
        // Get by email
        [HttpGet("{email}/{limit}")]
        public UserProfileDto GetByEmail(string email, bool limit)
        {
            return mapper.Map<UserProfile, UserProfileDto>(userProfileService.Get(email, limit));
        }

        // -----------------------------------------------------------------------------
        // POST: api/userprofiles/list
        [HttpPost("list")]
        public List<UserProfileDto> GetList(UserProfileFilterRequestDto filterRequestDto)
        {
            UserProfileFilterRequest filterRequest = mapper.Map<UserProfileFilterRequestDto, UserProfileFilterRequest>(filterRequestDto);

            return mapper.Map<List<UserProfile>, List<UserProfileDto>>
                (userProfileService.GetList(filterRequest));
        }

        // -----------------------------------------------------------------------------
        // POST: api/userprofiles/create (Sign Up)
        [AllowAnonymous]
        [HttpPost("create")]
        public UserProfileDto Create([FromBody]UserProfileDto userProfileDto)
        {
            return mapper.Map < UserProfile, UserProfileDto >(userProfileService.Create(mapper.Map<UserProfileDto, UserProfile>(userProfileDto)));
        }

        // -----------------------------------------------------------------------------
        // PUT: api/userprofiles/3 (UPDATE)
        [HttpPut("{id}")]
        public UserProfileDto Update(int id, [FromBody] UserProfileDto userProfileDto)
        {
            userProfileDto.Id = id;
            return mapper.Map<UserProfile, UserProfileDto>(userProfileService.Update(mapper.Map<UserProfileDto, UserProfile>(userProfileDto)));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/userprofiles/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return userProfileService.Delete(id);
        }


        // -----------------------------------------------------------------------------
        // POST: api/userprofiles/resetpassword 
        [AllowAnonymous]
        [HttpPost("resetpassword")]
        public void ResetPassword([FromBody] string email)
        {
            userProfileService.ResetPassword(email);
        }

        // -----------------------------------------------------------------------------
        // POST: api/userprofiles/authenticate (Login)
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public UserToAuthenticateResponseDto Authenticate([FromBody] UserToAuthenticateDto userToAuthenticateDto)
        {
            var response = mapper.Map <UserToAuthenticateResponse, UserToAuthenticateResponseDto>
                (userProfileService.Authenticate(mapper.Map<UserToAuthenticateDto, UserToAuthenticate>(userToAuthenticateDto)));

            return response;
        }
    }
}