using FluentValidation;

namespace FluentValidationTest
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleSet("newPerson", () =>
            {
                RuleFor(x => x.Id).Equal(0);
            });

            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).Length(0, 10);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Age).InclusiveBetween(18, 60);
        }
    }
}
