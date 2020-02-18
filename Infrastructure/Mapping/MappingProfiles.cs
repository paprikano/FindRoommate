using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FindRoommate.Models.Advert;
using FindRoommate.Models.UserProfile;
using FindRoommate.ViewModels.Advert;
using FindRoommate.ViewModels.UserProfile;

namespace FindRoommate.Infrastructure.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Advert mappings
            CreateMap<AdvertCreateViewModel, Advert>();
            CreateMap<Advert, AdvertEditViewModel>();

            //Profile mappings
            CreateMap<UserProfileCreateViewModel, UserProfile>();
            CreateMap<UserProfile, UserProfileEditViewModel>();
        }
    }
}
