using AutoMapper;
using FindRoommate.Infrastructure;
using FindRoommate.Models;
using FindRoommate.ViewModels.Advert;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FindRoommate.Models.Advert
{
    public class Advert
    {
        [Key]
        public int AdvertId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<AdvertImage.AdvertImage> AdvertImages { get; set; }        
    }
}