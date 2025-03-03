using Market.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMarketPriceService _marketPriceService;

        public PingController(IConfiguration configuration, IMarketPriceService marketPriceService)
        {
            _configuration = configuration;
            _marketPriceService = marketPriceService;
        }

        [HttpGet]
        public string Get()
        {
            return "pong";
        }
    }
}
