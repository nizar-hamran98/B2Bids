using Identity.Application.RequestHandler;
using Identity.Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Kernel.Helpers;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Identity.Domain.Validators
{
    public sealed class AuthenticationModelValidator : AbstractValidator<AuthenticateUser>
    {
        public AuthenticationModelValidator()
        {

            RuleFor(model => model.Model.UserName)
                  .NotEmpty()
                      .When(model => string.IsNullOrEmpty(model.Model.Email))
                      .WithMessage("Either UserName or Email is required");

            RuleFor(model => model.Model.Email)
                .NotEmpty()
                    .When(model => string.IsNullOrEmpty(model.Model.UserName))
                    .WithMessage("Either UserName or Email is required");

            RuleFor(model => model.Model.Password)
                .NotEmpty().WithMessage("Password is required");

            RuleFor(model => model.Model.Password)
                .NotEmpty()
                    .When(model => !string.IsNullOrEmpty(model.Model.Password))
                    .Must((model, password) =>
                    {
                        model.Model.Password = HashPassword.Hash(password);
                        return true;
                    });



        }
      /*  public override ValidationResult Validate(ValidationContext<Result<AuthenticationModel>> context)
        {
            var validationResult = base.Validate(context);

            if (!validationResult.IsValid)
            {
                *//*throw new ProblemException(new ExceptionDetails
                {
                    Detail = validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    Code = "GN50",
                    Title = "Authentication",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "Login"
                });*//*
            }

            return validationResult;
        }*/

    }
}
