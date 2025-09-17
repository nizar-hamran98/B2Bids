using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public sealed class UserLoginQuery : IQuery<UserLogin>
{
    public string? RefreshToken { get; set; }
    public long? UserId { get; set; }

    public class UserLoginQueryHandler : IQueryHandler<UserLoginQuery, UserLogin>
    {
        private readonly IRepositoryBase<UserLogin> _userLoginRepository;

        public UserLoginQueryHandler(IRepositoryBase<UserLogin> userLoginRepository) => _userLoginRepository = userLoginRepository;

        public async Task<UserLogin> Handle(UserLoginQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<UserLogin> userLogin = _userLoginRepository
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