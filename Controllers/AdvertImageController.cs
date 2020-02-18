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

        private readonly IImageService imageService;
        private readonly ApplicationDbContext context;
        private readonly IFlashMessage flashMessage;
        private readonly IAdvertImageRepository advertImageRepository;

        public AdvertImageController(
            IImageService imageService, 
            ApplicationDbContext context, 
            IFlashMessage flashMessage,
            IAdvertImageRepository advertImageRepository)
        {
            this.imageService = imageService;
            this.context = context;
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
            foreach (IFormFile file in files)
            {
                imageService.UploadImage(file);
                AdvertImage advertImage = new AdvertImage()
                {
                    ImagePath = file.FileName,
                    AdvertId = advertId
                };

                context.AdvertImages.Add(advertImage);
            }
            context.SaveChanges();

            flashMessage.Confirmation("Zdjęcia zostały dodane");
            return RedirectToAction(nameof(AdvertImageController.ManageImages), nameof(AdvertImageController).Replace("Controller", ""), new { advertId = advertId });
        }


        public IActionResult Delete(int advertImageId, int advertId)
        {
            AdvertImage advertImage = advertImageRepository.AdvertImages.FirstOrDefault(a => a.AdvertImageId == advertImageId);
            if (advertImage != null)
            {
                context.Remove(advertImage);
                context.SaveChanges();

                flashMessage.Confirmation("Zdjęcie zostało usunięte");
            }
            else
            {
                flashMessage.Danger("Zdjęcie nie zostało usunięte");
            }

            return RedirectToAction(nameof(AdvertImageController.ManageImages), nameof(AdvertImageController).Replace("Controller", ""), new { advertId = advertId });
        }
    }
}
