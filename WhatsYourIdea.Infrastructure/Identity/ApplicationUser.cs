using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace WhatsYourIdea.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public UserProfile UserProfile { get; set; }
    }
}