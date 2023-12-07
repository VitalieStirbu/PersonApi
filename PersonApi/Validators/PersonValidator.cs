using FluentValidation;
using PersonApi.Models;

namespace PersonApi.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.FirstName).NotNull().Length(1, 10);
            RuleFor(x => x.LastName).NotNull().Length(1, 10);
            RuleFor(x => x.Age).InclusiveBetween(0, 120);
        }
    }
}
