using Microsoft.AspNetCore.Mvc;
using PersonApi.Repositories;
using PersonApi.Models;
using FluentValidation;
using FluentValidation.Results;

namespace PersonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly IValidator<Person> _validator;
        private readonly ILogger<PersonController> _logger;

        public PersonController(
            IPeopleRepository peopleRepository, 
            IValidator<Person> validator, 
            ILogger<PersonController> logger)
        {
            _peopleRepository = peopleRepository;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all people");

            var result = await _peopleRepository.GetPeopleAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Getting person with id {id}", id);

            var result = await _peopleRepository.GetPersonAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Person person)
        {
            _logger.LogInformation("Adding person {@person}", person);

            ValidationResult result = await _validator.ValidateAsync(person);

            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for person {@person}", person);

                return BadRequest(result.Errors);
            }

            await _peopleRepository.AddPersonAsync(person);

            return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Person person)
        {
            _logger.LogInformation("Updating person with id {id} to {@person}", id, person);

            if (id != person.Id)
            {
                _logger.LogWarning("Invalid id {id}", id);

                return BadRequest("Invalid id");
            }

            ValidationResult result = await _validator.ValidateAsync(person);

            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for person {@person}", person);

                return BadRequest(result.Errors);
            }

           await _peopleRepository.UpdatePersonAsync(person);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting person with id {id}", id);

            var person = await _peopleRepository.GetPersonAsync(id);

            if (person == null)
            {
                _logger.LogWarning("Person with id {id} not found", id);

                return NotFound();
            }

            await _peopleRepository.DeletePersonAsync(person);

            return NoContent();
        }
    }
}
