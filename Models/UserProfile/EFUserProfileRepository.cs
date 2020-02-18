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

            context.UserProfiles.Add(userProfile);
            context.SaveChanges();
        }

        public void EditUserProfile(IFormFile file, UserProfileEditViewModel profileViewModel)
        {
            UserProfile dbEntry = context.UserProfiles.FirstOrDefault(p => p.UserProfileId == profileViewModel.UserProfileId);

            if (dbEntry != null)
            {
                dbEntry.Birthday = profileViewModel.Birthday;
                dbEntry.Description = profileViewModel.Description;
                dbEntry.Age = UserProfile.CalculateAge(profileViewModel.Birthday);
                dbEntry.Gender = profileViewModel.Gender;

                if (file != null)
                    dbEntry.ImagePath = file.FileName;
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
    }
}
