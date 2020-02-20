using FindRoommate.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using FindRoommate.ViewModels.Advert;
using AutoMapper;

namespace FindRoommate.Models.Advert
{
    public class EFAdvertRepository : IAdvertRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EFAdvertRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public IQueryable<Advert> Adverts => context.Adverts;

        public void CreateAdvert(AdvertCreateViewModel advertViewModel, string userId)
        {
            Advert advert = mapper.Map<Advert>(advertViewModel);
            advert.AppUserId = userId;
            context.Adverts.Add(advert);
            context.SaveChanges();
        }

        public void EditAdvert(AdvertEditViewModel advertViewModel)
        {
            Advert advert = Adverts.FirstOrDefault(a => a.AdvertId == advertViewModel.AdvertId);
            mapper.Map(advertViewModel, advert);

            context.SaveChanges();
        }

        public void DeleteAdvert(int advertId)
        {
            Advert dbEntry = Adverts.FirstOrDefault(a => a.AdvertId == advertId);
            if (dbEntry != null)
            {
                context.Adverts.Remove(dbEntry);
                context.SaveChanges();
            }
        }
    }
}
