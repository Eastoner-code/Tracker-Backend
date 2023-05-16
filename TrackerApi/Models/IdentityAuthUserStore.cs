using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TrackerApi.Models
{
    public class IdentityAuthUserStore : UserStore<IdentityAuthUser, IdentityAuthRole, TrackerContext, int>
    {
        public IdentityAuthUserStore(TrackerContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}