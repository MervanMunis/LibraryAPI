using LibraryAPI.Repositories.Abstracts;
using LibraryAPI.Repositories.Data;
using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Repositories.Concrete
{
    public class UserTokenRepository : RepositoryBase<IdentityUserToken<string>>, IUserTokenRepository
    {
        public UserTokenRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
