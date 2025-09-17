using Identity.Domain.DTOs;
using MediatR;
using SharedKernel;

namespace Identity.Domain.Models;
public class AuthenticationModel : IRequest<Result<UserDTO>>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class TokenModel
{
    public string UserName { get; set; }
    public string Password { get; set; }

}

