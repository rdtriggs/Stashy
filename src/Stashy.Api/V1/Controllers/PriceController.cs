using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stashy.Api.Infrastructure.Services;
using Stashy.Api.V1.Dtos;

namespace Stashy.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly ICoinService _coinService;

        public PriceController(ICoinService coinService)
        {
            _coinService = coinService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CoinDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyCollection<CoinDto>>> GetAsync()
        {
            IReadOnlyCollection<PriceDto> prices = await _coinService.GetAllAsync<PriceDto>();

            return Ok(prices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PriceDto>> GetAsync(string id)
        {
            PriceDto price = await _coinService.GetByIdAsync<PriceDto>(id);

            if (price == null)
            {
                return NotFound();
            }

            return Ok(price);
        }
    }
}
