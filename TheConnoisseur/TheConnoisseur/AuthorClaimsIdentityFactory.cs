using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using TheConnoisseur.Models;

namespace TheConnoisseur
{
    public class AuthorClaimsIdentityFactory : ClaimsIdentityFactory<Author>
    {
        public async override Task<ClaimsIdentity> CreateAsync(
                                                        UserManager<Author, string> manager,
                                                        Author user,
                                                        string authenticationType)
        {
            var identity = await base.CreateAsync(manager, user, authenticationType);
            identity.AddClaim(new Claim(ClaimTypes.StateOrProvince, user.State));
            return identity;
        }
    }
}