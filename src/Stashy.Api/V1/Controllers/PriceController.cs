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
    public class PriceController : ControllerBase
    {
        private readonly ICoinService _coinService;

        public PriceController(ICoinService coinService)
        {
            _coinService = coinService;
        }

        /// <summary>
        ///     Get a list of Prices.
        /// </summary>
        /// <returns>A list of Prices</returns>
        /// <response code="200">The list of Prices was successfully retrieved</response>
        [HttpGet]
        [ProducesResponseType(typeof(PriceDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyCollection<PriceDto>>> GetAsync()
        {
            IReadOnlyCollection<PriceDto> prices = await _coinService.GetAllAsync<PriceDto>();

            return Ok(prices);
        }

        /// <summary>
        ///     Get a Price by Id.
        /// </summary>
        /// <returns>A Product</returns>
        /// <response code="200">The Price was successfully retrieved</response>
        /// <response code="404">The Price was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PriceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
