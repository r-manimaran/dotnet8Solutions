
namespace password_hash;

public class UserEndpoints
{
    public static IEndpointRouteBuilder Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("register", async(RegisterUser.Request request, RegisterUser useCase)=>
            await useCase.Handle(request));

        builder.MapPost("login",async(LoginUser.Request request, LoginUser useCase)=>
            await useCase.Handle(request)); 

        return builder;
    }    
}