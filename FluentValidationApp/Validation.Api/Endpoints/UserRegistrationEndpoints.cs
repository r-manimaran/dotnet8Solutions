using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Validation.Api.Models;
using Validation.Api.ModelValidators;

namespace Validation.Api.Endpoints;

public class UserRegistrationEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var groupBuilder = app.MapGroup("/api");
        groupBuilder.MapPost("/register", UserRegistration);
    }

    public async Task<IResult> UserRegistration(UserRegistrationDto registration, 
                                            IValidator<UserRegistrationDto> validator)
    {
        List<string> errors = new();
        if (registration == null)
        {
            var problemDetails = new HttpValidationProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "No payload",
                Detail = "Pay load to this request is required.",
                Instance = "/api/register"
            };
            return Results.Problem(problemDetails);
        }

        var validationResult = validator.Validate(registration);
        if(!validationResult.IsValid)
        {
            var problemDetails  = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = "One or more validation errors occurred.",
                Instance = "/api/register"
            };
            return Results.Problem(problemDetails);            
        }
        
        return Results.Ok("Registered Successfully.");
    }
}

