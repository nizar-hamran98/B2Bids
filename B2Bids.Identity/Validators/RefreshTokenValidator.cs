using Identity.Application.RequestHandler;
using FluentValidation;
using FluentValidation.Results; 
using Microsoft.AspNetCore.Http; 

namespace Identity.Domain.Validators
{
    public sealed class RefreshTokenValidator : AbstractValidator<RefreshToken>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty()
                .NotNull()
                .WithMessage("{0} No Token were found");


            RuleFor(x => x.RefreshTokenId)
              .NotEmpty()
              .NotNull()
              .WithMessage("{0} Refresh token is not present in the headers");
        }


        public override ValidationResult Validate(ValidationContext<RefreshToken> context)
        {
            var validationResult = base.Validate(context);

            if (!validationResult.IsValid)
            {
                return validationResult;
                /*throw new ProblemException(new ExceptionDetails
                {
                    Detail = validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    Code = "GN58",
                    Title = "Authentication",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "Login"
                });*/
            }

            return validationResult;
        }

    }
}
