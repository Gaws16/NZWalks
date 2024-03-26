using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        string GenerateJSONWebToken(IdentityUser user, ICollection<string> roles);
        
    }
}
