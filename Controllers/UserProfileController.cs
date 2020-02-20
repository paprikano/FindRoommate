using FindRoommate.Infrastructure;
using FindRoommate.Models.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using FindRoommate.ViewModels;
using Vereyon.Web;
using Microsoft.AspNetCore.Http;
using FindRoommate.ViewModels.UserProfile;
using Microsoft.EntityFrameworkCore;
using FindRoommate.Models;
using AutoMapper;

namespace FindRoommate.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IHostingEnvironment environment;
        private readonly IFlashMessage flashMessage;
        private readonly IMapper mapper;
        private readonly IImageService imageService;

        public UserProfileController(
            IUserProfileRepository userProfileRepository, 
            UserManager<AppUser> userManager,
            ApplicationDbContext context,
            IHostingEnvironment environment,
            IFlashMessage flashMessage,
            IMapper mapper,
            IImageService imageService)
        {
            this.userProfileRepository = userProfileRepository;
            this.userManager = userManager;
            this.context = context;
            this.environment = environment;
            this.flashMessage = flashMessage;
            this.mapper = mapper;
            this.imageService = imageService;
        }

        [AllowAnonymous]
        public IActionResult Details(string userId)
        {
            if (userId == null)
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserProfile userProfile = userProfileRepository.UserProfiles.Include(p => p.AppUser).FirstOrDefault(p => p.AppUserId == userId);
            if (userProfile != null)
            {
                return View(userProfile);
            }
            else
            {
                flashMessage.Info("Profil nie został uzupełniony");
                return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
            }
        }

        [HttpGet]
        [Authorize]
        public ViewResult Create() => View(new UserProfileCreateViewModel());

        [HttpPost]
        [Authorize]
        public IActionResult Create(IFormFile file, UserProfileCreateViewModel userProfileViewModel)        
        {
            if (ModelState.IsValid && file != null)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                userProfileRepository.AddUserProfile(file, userProfileViewModel, userId);

                flashMessage.Confirmation("Profil został utworzony");
                return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));

            }
            else
            {
                if (file == null)
                    ModelState.AddModelError("No image", "Proszę dodać zdjęcie");
                return View(userProfileViewModel);
            }
        }

        [HttpGet]
        [Authorize]
        public ViewResult Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = userProfileRepository.UserProfiles.FirstOrDefault(p => p.AppUserId == userId);
            UserProfileEditViewModel userProfileViewModel = mapper.Map<UserProfileEditViewModel>(userProfile);
            return View(userProfileViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(IFormFile file, UserProfileEditViewModel userProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                userProfileRepository.EditUserProfile(file, userProfileViewModel);

                flashMessage.Confirmation("Profil został zedytowany");
                return RedirectToAction(nameof(UserProfileController.Details), nameof(UserProfileController).Replace("Controller", ""));
            }
            else
            {
                return View(userProfileViewModel);
            }
        }

        [Authorize]
        public IActionResult Delete()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            userProfileRepository.DeleteUserProfile(userId);

            flashMessage.Confirmation("Profil został usunięty");
            return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
        }
    }
}
