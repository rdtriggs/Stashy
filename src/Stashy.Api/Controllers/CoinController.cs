using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stashy.Api.Infrastructure.Dtos;
using Stashy.Api.Infrastructure.Services;

namespace Stashy.Api.Controllers
{
    [Route("v2/[controller]")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly ICoinService _coinService;

        public CoinController(ICoinService coinService)
        {
            _coinService = coinService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<CoinDto>>> GetAsync()
        {
            IReadOnlyCollection<CoinDto> coins = await _coinService.GetAllAsync<CoinDto>();

            return Ok(coins);
        }
    }
}
