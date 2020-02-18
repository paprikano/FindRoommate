using AutoMapper;
using FindRoommate.Infrastructure;
using FindRoommate.Models;
using FindRoommate.ViewModels.UserProfile;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FindRoommate.Models.UserProfile
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Description { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "Proszę dodać zdjęcie")]
        public string ImagePath { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public static int CalculateAge (DateTime Birthday)
        {
            int age = DateTime.Today.Year - Birthday.Year;
            return (Birthday.Date > DateTime.Today.AddYears(-age)) ? --age : age;
        }
    }    
}
