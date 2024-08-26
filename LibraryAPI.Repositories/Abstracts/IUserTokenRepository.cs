using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IUserTokenRepository : IRepositoryBase<IdentityUserToken<string>>
    {
    }
}
