using System;
using System.ComponentModel.DataAnnotations;

namespace FindRoommate.ViewModels.UserProfile
{
    public class UserProfileCreateViewModel
    {
        [Required(ErrorMessage = "Proszę podać datę urodzenia")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Proszę napisać coś o sobie")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Proszę określić płeć")]
        public string Gender { get; set; }
    }
}
