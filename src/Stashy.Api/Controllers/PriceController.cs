using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stashy.Api.Infrastructure.Dtos;
using Stashy.Api.Infrastructure.Services;

namespace Stashy.Api.Controllers
{
    [Route("v2/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly ICoinService _coinService;

        public PriceController(ICoinService coinService)
        {
            _coinService = coinService;
        }

        [HttpGet]
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
