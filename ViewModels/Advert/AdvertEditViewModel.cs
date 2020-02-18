using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.ViewModels.Advert
{
    public class AdvertEditViewModel
    {
        public int AdvertId { get; set; }
        [Required(ErrorMessage = "Proszę podać tytuł")]
        public string Title { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Proszę podać dodatnią cenę")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Proszę podać opis")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Proszę podać kategorię")]
        public string Category { get; set; }
    }
}
