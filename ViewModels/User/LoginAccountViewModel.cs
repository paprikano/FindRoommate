using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.ViewModels.User
{
    public class LoginUserViewModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("email")]
        public string Password { get; set; }
    }
}
