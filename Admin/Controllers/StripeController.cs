using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using plannerBackEnd.Admin.Controllers.Dto;
using plannerBackEnd.Admin.Domain;
using plannerBackEnd.Admin.Domain.DomainObjects;
using Stripe;
using System.Collections.Generic;
using plannerBackEnd.Common;

namespace plannerBackEnd.Admin.Controllers
{
    [EntryFilter]
    [Route("api/[controller]")]
    public class StripeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IStripeService stripeService;

        // -----------------------------------------------------------------------------

        public StripeController(IMapper mapper, IStripeService stripeService)
        {
            this.mapper = mapper;
            this.stripeService = stripeService;
        }

        // -----------------------------------------------------------------------------
        // POST: api/stripe/12334567654
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{productkey}")]
        public Product GetProduct(string productkey)
        {
            return stripeService.GetProduct(productkey);
        }

        // -----------------------------------------------------------------------------
        // POST: api/stripe/listprices
        [HttpPost("listprices")]
        public List<LimitedPriceDto> GetListPrices()
        {
            return mapper.Map<List<LimitedPrice>, List<LimitedPriceDto>>(stripeService.GetListPrices());
        }
    }
}