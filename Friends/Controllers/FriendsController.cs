using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common;
using plannerBackEnd.Common.Filters.DomainObjects;
using plannerBackEnd.Common.Filters.Dto;
using plannerBackEnd.Common.Swagger;
using plannerBackEnd.Friends.Controllers.Dto;
using plannerBackEnd.Friends.Domain;
using plannerBackEnd.Friends.Domain.DomainObjects;

namespace plannerBackEnd.Friends.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IFriendService friendService;

        // -----------------------------------------------------------------------------

        public FriendsController(IMapper mapper, IFriendService friendService)
        {
            this.mapper = mapper;
            this.friendService = friendService;
        }

        // -----------------------------------------------------------------------------
        // POST: api/friends/getListFriends 
        [SwaggerRouteDescription(Description = "REQUIRES: Current UserId.")]
        [HttpPost("getListFriends")]
        public List<FriendDto> ListFriends([FromBody] FriendFilterRequestDto filterDto)
        {
            FriendFilterRequest filter = mapper.Map<FriendFilterRequestDto, FriendFilterRequest>(filterDto);

            return mapper.Map<List<Friend>, List<FriendDto>>
                (friendService.GetListFriends(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/friends/getListActiveFriends 
        [SwaggerRouteDescription(Description = "REQUIRES: Current UserId.")]
        [HttpPost("getListActiveFriends")]
        public List<ActiveFriendDto> ListActiveFriends([FromBody] ActiveFriendFilterRequestDto filterDto)
        {
            ActiveFriendFilterRequest filter = mapper.Map<ActiveFriendFilterRequestDto, ActiveFriendFilterRequest>(filterDto);

            return mapper.Map<List<ActiveFriend>, List<ActiveFriendDto>>(friendService.GetListActiveFriends(filter));
        }

        // -----------------------------------------------------------------------------
        // POST: api/friends/getListSuggestedFriends 
        [SwaggerRouteDescription(Description = "REQUIRES: Integer of UserId, no object in body. Purely a number.")]
        [HttpPost("getListSuggestedFriends")]
        public List<SuggestedFriendDto> ListSuggestedFriends([FromBody] int requestedId)
        {

            return mapper.Map<List<SuggestedFriend>, List<SuggestedFriendDto>>(friendService.GetListSuggestedFriends(requestedId));
        }

        // -----------------------------------------------------------------------------
        // POST: api/friends/getListFriendStreaks 
        [SwaggerRouteDescription(Description = "REQUIRES: Current UserId.")]
        [HttpPost("getListFriendStreaks")]
        public BaseFilterResponseDto ListSuggestedFriends([FromBody] BaseFilterRequestDto filterDto)
        {
            BaseFilterRequest filter = mapper.Map<BaseFilterRequestDto, BaseFilterRequest>(filterDto);

            return mapper.Map<BaseFilterResponse, BaseFilterResponseDto>(friendService.GetListFriendStreaks(filter));
        }


        // -----------------------------------------------------------------------------
        // POST: api/friends/sendRequest 
        [SwaggerRouteDescription(Description = "REQUIRES: UserId1, UserId2. UserId1 MUST BE the current user.")]
        [HttpPost("sendRequest")]
        public FriendDto SendRequest([FromBody] FriendDto friendDto)
        {
            return mapper.Map<Friend, FriendDto>(friendService.SendRequest(mapper.Map<FriendDto, Friend>(friendDto)));
        }

        // -----------------------------------------------------------------------------
        // POST: api/friends/acceptRequest 
        [SwaggerRouteDescription(Description = "REQUIRES: id of friends row.")]
        [HttpPost("acceptRequest")]
        public FriendDto AcceptRequest([FromBody] int requestedId)
        {
            return mapper.Map<Friend, FriendDto>(friendService.AcceptRequest(requestedId));
        }

        // -----------------------------------------------------------------------------
        // POST: api/friends/declineRequest 
        [SwaggerRouteDescription(Description = "REQUIRES: id of friends row.")]
        [HttpPost("declineRequest")]
        public FriendDto DeclineRequest([FromBody] int requestedId)
        {
            return mapper.Map<Friend, FriendDto>(friendService.DeclineRequest(requestedId));
        }

        // ------------------------------------------------------------------------------------------------
        // DELETE: api/friends/3
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return friendService.Delete(id);
        }
    }
}