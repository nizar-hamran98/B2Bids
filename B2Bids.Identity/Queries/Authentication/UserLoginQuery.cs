using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public sealed class UserLoginQuery : IQuery<UserLogin>
{
    public string? RefreshToken { get; set; }
    public long? UserId { get; set; }

    public class UserLoginQueryHandler(IRepositoryBase<UserLogin> userLoginRepository) : IQueryHandler<UserLoginQuery, UserLogin>
    {
        public async Task<UserLogin> Handle(UserLoginQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<UserLogin> userLogin = userLoginRepository
                    .GetAll()
                    .AsNoTracking();

            if (query.UserId.HasValue)
            {
                userLogin = userLogin.Where(login => login.UserId == query.UserId);
            }

            if (query.RefreshToken.IsNotNullOrEmpty())
            {
                userLogin = userLogin.Where(login => login.RefreshTokenId == query.RefreshToken);
            }

            return await userLogin.FirstOrDefaultAsync();
        }

    }
}