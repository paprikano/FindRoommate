using FindRoommate.Models;
using FindRoommate.Models.Advert;
using FindRoommate.Models.UserProfile;
using FindRoommate.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vereyon.Web;

namespace FindRoommate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUserValidator<AppUser> userValidator;
        private readonly IPasswordValidator<AppUser> passwordValidator;
        private readonly IPasswordHasher<AppUser> passwordHasher;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IUserProfileRepository profileRepository;
        private readonly IAdvertRepository advertRepository;
        private readonly IFlashMessage flashMessage;

        public AccountController(
            UserManager<AppUser> userManager,
            IUserValidator<AppUser> userValidator,
            IPasswordValidator<AppUser> passwordValidator,
            IPasswordHasher<AppUser> passwordHasher,
            SignInManager<AppUser> signInManager,
            IUserProfileRepository profileRepository,
            IAdvertRepository advertRepository,
            IFlashMessage flashMessage)
        {
            this.userManager = userManager;
            this.userValidator = userValidator;
            this.passwordValidator = passwordValidator;
            this.passwordHasher = passwordHasher;
            this.signInManager = signInManager;
            this.profileRepository = profileRepository;
            this.advertRepository = advertRepository;
            this.flashMessage = flashMessage;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login () =>
            View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel details)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        UserProfile profile = profileRepository.UserProfiles.FirstOrDefault(p => p.AppUserId == user.Id);
                        flashMessage.Confirmation("Zostałeś zalogowany");
                        if (profile == null)
                            return RedirectToAction(nameof(UserProfileController.Create), nameof(UserProfileController).Replace("Controller", ""));
                        else 
                            return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
                    }
                }

                ModelState.AddModelError(nameof(LoginUserViewModel.Email), "Nieprawidłowy adres email lub hasło");
            }

            return View(details);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            flashMessage.Confirmation("Zostałeś wylogowany");
            return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Create() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    flashMessage.Confirmation("Konto zostało utworzone");
                    return RedirectToAction(nameof(AccountController.Login), nameof(AccountController).Replace("Controller", ""));
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        flashMessage.Confirmation("Konto zostało zedytowane");
                        return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika");
            }

            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                Advert advert = advertRepository.Adverts.FirstOrDefault(a => a.AppUserId == userId);
                UserProfile profile = profileRepository.UserProfiles.FirstOrDefault(p => p.AppUserId == userId);
                if (advert != null)
                {
                    return View("DeleteError", "Proszę najpierw usunąć swoje ogłoszenia");
                }
                else if (profile != null)
                {
                    return View("DeleteError", "Proszę najpierw usunąć swój profil");
                }
                else
                {                                    
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {                        
                        await signInManager.SignOutAsync();
                        flashMessage.Confirmation("Konto zostało usunięte");
                        return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                return View("DeleteError", "Nie znaleziono użytkownika");
            }

            return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
        }

        public void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}