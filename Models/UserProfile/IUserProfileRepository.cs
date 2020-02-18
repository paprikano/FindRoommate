using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindRoommate.ViewModels.UserProfile;
using Microsoft.AspNetCore.Http;

namespace FindRoommate.Models.UserProfile
{
    public interface IUserProfileRepository
    {
        IQueryable<UserProfile> UserProfiles { get; }

        void AddUserProfile(IFormFile file, UserProfileCreateViewModel userProfileViewModel, string userId);
        void EditUserProfile(IFormFile file, UserProfileEditViewModel profileViewModel);
        void DeleteUserProfile(string userId);
    }
}
