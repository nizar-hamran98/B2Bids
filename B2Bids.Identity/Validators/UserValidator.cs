using Identity.Domain.Entities;
using FluentValidation;
using FluentValidation.Results; 
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Identity.Domain.Validators
{
    public sealed class UserValidator : AbstractValidator<User>
    {
        
        public UserValidator()
        {
            RuleFor(x => x.StatusId)
                .Equal((short)EntityStatus.Active)
                .WithMessage("User is inactive");

            RuleFor(x => x.Role.StatusId)
                .Equal((short)EntityStatus.Active)
                .WithMessage("User Role is inactive");
            
        }

        /*public override ValidationResult Validate(ValidationContext<Result<User>> context)
        {
            var validationResult = base.Validate(context);

            if (!validationResult.IsValid)
            {
                return validationResult;
                //Detail = validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty,               
            }

            return validationResult;
        }*/

    }
}
