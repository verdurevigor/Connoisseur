using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using TheConnoisseur.Models;

namespace TheConnoisseur
{
    public class AppUserClaimsIdentityFactory : ClaimsIdentityFactory<AppUser>
    {
        public async override Task<ClaimsIdentity> CreateAsync(
            UserManager<AppUser, string> manager,
            AppUser user,
            string authenticationType)
        {
            var identity = await base.CreateAsync(manager, user, authenticationType);
            identity.AddClaim(new Claim(ClaimTypes.StateOrProvince, user.State));     // TODO: Add necessary Claims here for when a user registers. Not sure what I'll need yet, but add ClaimTypes.Country, user.Country cause errors as It's not part of the RegisterModel (I think).
                                                                                    // Ctrl + (Space) after ClaimTypes. brings up a list of claims from ASP.NET Identity. Not sure how to create custom claims...
            return identity;
        }
    }
}