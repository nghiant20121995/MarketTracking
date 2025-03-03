using Market.Domain.Entities;
using Market.Domain.Interfaces.Repository;
using Market.Domain.Interfaces.Services;
using Market.Domain.RequestModel;
using Market.Domain.Response;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketPriceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMarketPriceService _marketPriceService;

        public MarketPriceController(IConfiguration configuration, IMarketPriceService marketPriceService)
        {
            _configuration = configuration;
            _marketPriceService = marketPriceService;
        }

        [HttpGet]
        public async Task<MarketPriceTrackingResponse> Get(MarketPriceTrackingRequest? request)
        {
            var rs = await _marketPriceService.GetAllAsync(request);
            return rs;
        }
    }
}
