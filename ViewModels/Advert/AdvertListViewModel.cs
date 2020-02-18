using FindRoommate.Models.Advert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.ViewModels.Advert
{
    public class AdvertListViewModel
    {
        public IEnumerable<FindRoommate.Models.Advert.Advert> Adverts { get; set; }
        public AdvertPagingInfoViewModel AdvertPagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
