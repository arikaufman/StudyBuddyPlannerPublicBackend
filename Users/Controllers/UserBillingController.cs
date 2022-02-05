using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Common;
using plannerBackEnd.Users.Controllers.Dto;
using plannerBackEnd.Users.Domain;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.Controllers
{
    [EntryFilter]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserBillingController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUserBillingService userBillingService;

        // -----------------------------------------------------------------------------

        public UserBillingController(IMapper mapper, IUserBillingService userBillingService)
        {
            this.mapper = mapper;
            this.userBillingService = userBillingService;
        }

        // -----------------------------------------------------------------------------
        // POST: api/userbilling/createsubscription (Sign Up)
        [AllowAnonymous]
        [HttpPost("createsubscription")]
        public UserBillingDto Create([FromBody] UserBillingSubscriptionRequestDto userBillingSubscriptionRequestDto)
        {
            return mapper.Map<UserBilling, UserBillingDto>
                (userBillingService.CreateSubscription(mapper.Map<UserBillingSubscriptionRequestDto, UserBillingSubscriptionRequest>(userBillingSubscriptionRequestDto)));
        }

        // -----------------------------------------------------------------------------
        // POST: api/userbilling/cancelsubscription (Sign Up)
        [AllowAnonymous]
        [HttpPost("cancelsubscription")]
        public UserBillingDto Cancel([FromBody] UserBillingCancelSubscriptionRequestDto userBillingCancelSubscriptionRequestDto)
        {
            return mapper.Map<UserBilling, UserBillingDto>
                (userBillingService.CancelSubscription(mapper.Map<UserBillingCancelSubscriptionRequestDto, UserBillingCancelSubscriptionRequest>(userBillingCancelSubscriptionRequestDto)));
        }
    }
}