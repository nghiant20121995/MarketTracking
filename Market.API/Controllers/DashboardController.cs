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
    public class DashboardController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMarketPriceService _marketPriceService;

        public DashboardController(IConfiguration configuration, IMarketPriceService marketPriceService)
        {
            _configuration = configuration;
            _marketPriceService = marketPriceService;
        }

        [Route("maxprice")]
        [HttpGet]
        public async Task<decimal> Get(MarketPriceTrackingRequest? request)
        {
            var rs = await _marketPriceService.GetMaxPriceAsync(request);
            return rs;
        }

        [Route("minprice")]
        [HttpGet]
        public async Task<decimal> GetMin(MarketPriceTrackingRequest? request)
        {
            var rs = await _marketPriceService.GetMinPriceAsync(request);
            return rs;
        }

        [Route("averageprice")]
        [HttpGet]
        public async Task<decimal> GetAverage(MarketPriceTrackingRequest? request)
        {
            var rs = await _marketPriceService.GetAveragePriceAsync(request);
            return rs;
        }
    }
}
