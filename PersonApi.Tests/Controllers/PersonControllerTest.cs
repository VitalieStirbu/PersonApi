using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonApi.Controllers;
using PersonApi.Models;
using PersonApi.Repositories;

namespace PersonApi.Tests.Controllers
{
    public class PersonControllerTest
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly IValidator<Person> _validator;
        private readonly PersonController _personController;

        public PersonControllerTest()
        {
            _peopleRepository = A.Fake<IPeopleRepository>();
            _validator = A.Fake<IValidator<Person>>();

            _personController = new PersonController(_peopleRepository, _validator, A.Fake<ILogger<PersonController>>());
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithListOfPeople()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Doe" },
                new Person { Id = 2, FirstName = "Jane", LastName = "Doe" }
            };

            A.CallTo(() => _peopleRepository.GetPeopleAsync()).Returns(people);

            // Act
            var result = await _personController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Person>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Get_WithId_ReturnsOkObjectResult_WithPerson()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe" };

            A.CallTo(() => _peopleRepository.GetPersonAsync(1)).Returns(person);

            // Act
            var result = await _personController.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Person>(okResult.Value);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Get_WithId_ReturnsNotFoundResult()
        {
            // Arrange
            A.CallTo(() => _peopleRepository.GetPersonAsync(1)).Returns(default(Person));

            // Act
            var result = await _personController.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_WithValidPerson_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe" };

            A.CallTo(() => _validator.ValidateAsync(person, default)).Returns(new ValidationResult());

            // Act
            var result = await _personController.Post(person);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Post_WithInvalidPerson_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = null , LastName = "Doe" };

            A.CallTo(() => _validator.ValidateAsync(person, default)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("FirstName", "First name is required") }));

            // Act
            var result = await _personController.Post(person);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(badRequestObjectResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task Put_WithValidPerson_ReturnsNoContentResult()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe" };

            A.CallTo(() => _validator.ValidateAsync(person, default)).Returns(new ValidationResult());

            // Act
            var result = await _personController.Put(1, person);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_WithInvalidPerson_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = null, LastName = "Doe" };

            A.CallTo(() => _validator.ValidateAsync(person, default)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("FirstName", "First name is required") }));

            // Act
            var result = await _personController.Put(1, person);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(badRequestObjectResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task Put_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe" };

            // Act
            var result = await _personController.Put(2, person);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WithValidPerson_ReturnsNoContentResult()
        {
            // Arrange
            var personId = 1;

            // Act
            var result = await _personController.Delete(personId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WithInvalidPerson_ReturnsNotFoundResult()
        {
            // Arrange
            var personId = 1;

            A.CallTo(() => _peopleRepository.GetPersonAsync(personId)).Returns(default(Person));

            // Act
            var result = await _personController.Delete(personId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
