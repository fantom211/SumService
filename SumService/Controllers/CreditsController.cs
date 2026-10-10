using Microsoft.AspNetCore.Mvc;
using SumService.Models.DTOs;
using SumService.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SumService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditsController : ControllerBase
    {
        private readonly CreditCalculationService _service;

        public CreditsController(CreditCalculationService service)
        {
            _service = service;
        }
        // GET: api/<CreditController>
        [HttpGet]
        public async Task<ActionResult<List<CreditResponseDto>>> GetAll(
            [FromHeader(Name = "X-User-Id")] Guid userId)
        {
            var credits = await _service.GetAllAsync(userId);

            return Ok(credits);
        }

        // GET api/<CreditController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CreditResponseDto>> GetById(Guid id)
        {
            var credit = await _service.GetByIdAsync(id);

            if (credit is null)
                return NotFound();

            return Ok(credit);
        }

        // POST api/<CreditController>
        [HttpPost]
        public async Task<ActionResult<CreditResponseDto>> Create(
            [FromBody] CreditCreateDto dto,
            [FromHeader(Name ="X-User-Id")] Guid userId)
        {
            var result = await _service.CreateAsync(dto, userId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        // PUT api/<CreditController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CreditController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
