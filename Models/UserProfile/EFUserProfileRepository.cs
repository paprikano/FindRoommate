using AutoMapper;
using FindRoommate.Infrastructure;
using FindRoommate.ViewModels.UserProfile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.Models.UserProfile
{
    public class EFUserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IImageService imageService;

        public EFUserProfileRepository(ApplicationDbContext context, IMapper mapper, IImageService imageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageService = imageService;
        }

        public IQueryable<UserProfile> UserProfiles => context.UserProfiles;

        public void AddUserProfile(IFormFile file, UserProfileCreateViewModel userProfileViewModel, string userId)
        {
            imageService.UploadImage(file);

            UserProfile userProfile = mapper.Map<UserProfile>(userProfileViewModel);
            userProfile.ImagePath = file.FileName;
            userProfile.AppUserId = userId;
            userProfile.Age = CalculateAge(userProfileViewModel.Birthday);

            context.UserProfiles.Add(userProfile);
            context.SaveChanges();
        }

        public void EditUserProfile(IFormFile file, UserProfileEditViewModel userProfileViewModel)
        {
            UserProfile userProfile = context.UserProfiles.FirstOrDefault(p => p.UserProfileId == userProfileViewModel.UserProfileId);

            if (userProfile != null)
            {
                mapper.Map(userProfileViewModel, userProfile);

                if (file != null)
                    userProfile.ImagePath = file.FileName;
            }

            context.SaveChanges();
        }

        public void DeleteUserProfile(string userId)
        {
            UserProfile dbEntry = UserProfiles.FirstOrDefault(a => a.AppUserId == userId);
            if (dbEntry != null)
            {
                context.UserProfiles.Remove(dbEntry);
                context.SaveChanges();
            }
        }

        public static int CalculateAge(DateTime Birthday)
        {
            int age = DateTime.Today.Year - Birthday.Year;
            return (Birthday.Date > DateTime.Today.AddYears(-age)) ? --age : age;
        }
    }
}
