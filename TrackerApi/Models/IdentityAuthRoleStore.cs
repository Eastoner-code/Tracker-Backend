using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TrackerApi.Models
{
    public class IdentityAuthRoleStore : RoleStore<IdentityAuthRole, TrackerContext, int>
    {
        public IdentityAuthRoleStore(TrackerContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}