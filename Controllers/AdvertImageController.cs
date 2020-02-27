using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindRoommate.Infrastructure;
using FindRoommate.Models.AdvertImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace FindRoommate.Controllers
{
    public class AdvertImageController : Controller
    {        
        private readonly IFlashMessage flashMessage;
        private readonly IAdvertImageRepository advertImageRepository;

        public AdvertImageController(
            IFlashMessage flashMessage,
            IAdvertImageRepository advertImageRepository)
        {
            this.flashMessage = flashMessage;
            this.advertImageRepository = advertImageRepository;
        }

        [HttpGet]
        public ViewResult ManageImages(int advertId)
        {
            ViewBag.AdvertId = advertId;
            var images = advertImageRepository.AdvertImages.Where(a => a.AdvertId == advertId);
            return View(images);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UploadImages(List<IFormFile> files, int advertId)
        {
            if (files != null)
            {
                advertImageRepository.UploadAdvertImages(files, advertId);
                flashMessage.Confirmation("Zdjęcia zostały dodane");
            }

            return RedirectToAction(nameof(AdvertImageController.ManageImages), nameof(AdvertImageController).Replace("Controller", ""), new { advertId = advertId });
        }


        public IActionResult Delete(int advertImageId, int advertId)
        {
            advertImageRepository.DeleteAdvertImage(advertImageId);

            flashMessage.Confirmation("Zdjęcie zostało usunięte");
            return RedirectToAction(nameof(AdvertImageController.ManageImages), nameof(AdvertImageController).Replace("Controller", ""), new { advertId = advertId });
        }
    }
}
