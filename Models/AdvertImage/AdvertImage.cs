using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindRoommate.Models.Advert;

namespace FindRoommate.Models.AdvertImage
{
    public class AdvertImage
    {
        public int AdvertImageId { get; set; }
        public string ImagePath { get; set; }

        public int AdvertId { get; set; }
        public Advert.Advert Advert { get; set; } 
    }
}
