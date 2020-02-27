using FindRoommate.Infrastructure;
using FindRoommate.Models.Advert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vereyon.Web;
using FindRoommate.ViewModels.Advert;
using FindRoommate.Models;
using System.Collections.Generic;
using AutoMapper;

namespace FindRoommate.Controllers
{
    public class AdvertController : Controller
    {
        private readonly IAdvertRepository advertRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IFlashMessage flashMessage;
        private readonly IMapper mapper;
        int PageSize = 8;

        public AdvertController(
            IAdvertRepository advertRepository, 
            UserManager<AppUser> userManager,
            IFlashMessage flashMessage,
            IMapper mapper)
        {
            this.advertRepository = advertRepository;
            this.userManager = userManager;
            this.flashMessage = flashMessage;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        public ViewResult List(string category, int productPage = 1)
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(new AdvertListViewModel {
                    Adverts = advertRepository.Adverts.Include(a => a.AppUser)
                        .Where(a => category == null || a.Category == category)
                        .OrderBy(a => a.AdvertId)
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize),
                    AdvertPagingInfo = new AdvertPagingInfoViewModel
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = category == null ? 
                            advertRepository.Adverts.Count() : 
                            advertRepository.Adverts.Where(a => a.Category == category).Count()
                    },
                    CurrentCategory = category
                });
        }

        [AllowAnonymous]
        public ViewResult Details(int advertId)
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);            
            return View(advertRepository.Adverts.Include(a => a.AppUser).Include(a => a.AdvertImages).FirstOrDefault(a => a.AdvertId == advertId));        
        }

        [HttpGet]
        [Authorize]
        public ViewResult Create() => View(new AdvertCreateViewModel());

        [HttpPost]
        [Authorize]
        public IActionResult Create(AdvertCreateViewModel advertViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                advertRepository.CreateAdvert(advertViewModel, userId);

                flashMessage.Confirmation("Ogłoszenie zostało dodane");
                return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
            }
            else
            {
                return View(advertViewModel);
            }
        }

        [HttpGet]
        [Authorize]
        public ViewResult Edit(int advertId)
        {
            Advert advert = advertRepository.Adverts.FirstOrDefault(a => a.AdvertId == advertId);
            AdvertEditViewModel advertViewModel = mapper.Map<AdvertEditViewModel>(advert);
            return View(advertViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(AdvertEditViewModel advertViewModel)
        {
            if (ModelState.IsValid)
            {
                advertRepository.EditAdvert(advertViewModel);    

                flashMessage.Confirmation("Ogłoszenie zostało zedytowane");
                return RedirectToAction(nameof(AdvertController.Details), nameof(AdvertController).Replace("Controller", ""), new { advertId = advertViewModel.AdvertId });
            }
            else
            {
                return View(advertViewModel);
            }
        }

        [Authorize]
        public IActionResult Delete(int advertId)
        {
            advertRepository.DeleteAdvert(advertId);

            flashMessage.Confirmation("Ogłoszenie zostało usunięte");
            return RedirectToAction(nameof(AdvertController.List), nameof(AdvertController).Replace("Controller", ""));
        }
    }
}
