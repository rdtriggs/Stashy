using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stashy.Api.Infrastructure.Services;
using Stashy.Api.V1.Dtos;

namespace Stashy.Api.V1.Controllers
{
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly ICoinService _coinService;

        public CoinController(ICoinService coinService)
        {
            _coinService = coinService;
        }

        /// <summary>
        ///     Get a list of Coins.
        /// </summary>
        /// <returns>A list of Coins</returns>
        /// <response code="200">The list of Coins was successfully retrieved</response>
        [HttpGet]
        [ProducesResponseType(typeof(CoinDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyCollection<CoinDto>>> GetAsync()
        {
            IReadOnlyCollection<CoinDto> coins = await _coinService.GetAllAsync<CoinDto>();

            return Ok(coins);
        }
    }
}
