using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Advert.Advert> Adverts { get; set; }
        public UserProfile.UserProfile UserProfile { get; set; }
    }
}
