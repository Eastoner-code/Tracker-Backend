using Microsoft.AspNetCore.Identity;

namespace TrackerApi.Models
{
    public sealed class IdentityAuthRole : IdentityRole<int>
    {
        public IdentityAuthRole()
        {

        }
        public IdentityAuthRole(string roleName)
        {
            Name = roleName;
            NormalizedName = roleName.ToUpper();
        }
    }
}
