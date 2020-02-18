using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindRoommate.ViewModels.Advert;

namespace FindRoommate.Models.Advert
{
    public interface IAdvertRepository
    {
        IQueryable<Advert> Adverts { get; }

        void CreateAdvert(AdvertCreateViewModel advertViewModel, string userId);
        void EditAdvert(AdvertEditViewModel advertViewModel);
        void DeleteAdvert(int advertId);
    }
}
