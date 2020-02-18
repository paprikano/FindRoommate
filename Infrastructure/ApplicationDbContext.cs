using FindRoommate.Models.Advert;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindRoommate.Models.UserProfile;
using FindRoommate.Models;
using FindRoommate.Models.AdvertImage;

namespace FindRoommate.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Advert> Adverts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<AdvertImage> AdvertImages { get; set; }
    }
}
