using Kernel.Contract;
using Kernel.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;

namespace SharedKernel
{
    /// <summary>
    ///   Json web token--- middleware, it will catch the token and login with the user details
    /// </summary>
    public class JwtMiddleware(RequestDelegate next)
    {
        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context, IJwtManager jwtManager)
        {
            var @continue = true;
            //var authenticateResult = await context.AuthenticateAsync("AzureAd");
            //if (authenticateResult.Succeeded && authenticateResult.Principal != null)
            //{
            //    // If authenticated using Azure AD, skip the middleware
            //    await _next(context);
            //    return;
            //}
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    IEnumerable<Claim> claims = jwtManager.ParseToken(token);
                    if (claims != null && claims.Any())
                    {
                        //@continue = await IsUserValid(claims, context);
                        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "basic"));
                    }
                    @continue = true;
                }
                catch (SecurityTokenExpiredException ex)
                {
                    @continue = false;
                    var response = context.Response;
                    response.ContentType = "application/json";
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    var result = Result.Failure("Token is expired");                        
                    await response.WriteAsync(result.Serialize());
                }
                catch (Exception ex)
                {
                    @continue = false;
                    var response = context.Response;
                    response.ContentType = "application/json";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var result = Result.Failure("Invalid token");
                    await response.WriteAsync(result.Serialize());
                }
            }

            if (@continue)
            {
                await next(context);
            }
        }
       /* private async Task<bool> IsUserValid(IEnumerable<Claim> claims, Microsoft.AspNetCore.Http.HttpContext context)
        {
            var userName = claims.FirstOrDefault(claim => claim.Type == "UserName")?.Value;

            if (userName.IsNullOrEmpty()) return false;
            if (userName!.Equals("admin")) return true;

            Console.WriteLine(" User Name : " + userName);
            var user = await _cacheService.GetByPropertyAsync(_identityCacheDistributed, CachingKey.UserName, userName, ConfigurationCashingConstants.Users);
            Console.WriteLine(" User Name Object : " + user);
            if (string.IsNullOrEmpty(user))
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var result = new ProblemException(new ExceptionDetails
                {
                    Detail = "User Doesn't Exist",
                    Code = "GN58",
                    Title = "Authentication",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "Login"
                });

                await response.WriteAsync(result.Serialize());
                return false;
            }

            UserModel userDetails = new UserModel();
            try
            {
                userDetails = JsonDeserializer.DeserializeOrDefault<UserModel>(user);
            }
            catch
            {
                Console.WriteLine(" User Name : " + userName + " Check Cache List");
                return true;
            }

            if (userDetails.UserName.IsNullOrEmpty())
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var result = new ProblemException(new ExceptionDetails
                {
                    Detail = "User Doesn't Exist",
                    Code = "GN58",
                    Title = "Authentication",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "Login"
                });

                await response.WriteAsync(result.Serialize());
                return false;
            }

            if (userDetails.Status == EntityStatus.Inactive || !userDetails.IsAllowAccess)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var result = new ProblemException(new ExceptionDetails
                {
                    Detail = "Inactive User",
                    Code = "GN58",
                    Title = "Authentication",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "Login"
                });

                return false;
            }

            return true;
        }*/
    }
}
