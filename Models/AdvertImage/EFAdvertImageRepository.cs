using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindRoommate.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace FindRoommate.Models.AdvertImage
{
    public class EFAdvertImageRepository : IAdvertImageRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IImageService imageService;

        public EFAdvertImageRepository(ApplicationDbContext context, IImageService imageService)
        {
            this.context = context;
            this.imageService = imageService;
        }

        public IQueryable<AdvertImage> AdvertImages => context.AdvertImages;

        public void UploadAdvertImages(List<IFormFile> files, int advertId)
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
        }

        public void DeleteAdvertImage(int advertImageId)
        {
            AdvertImage advertImage = AdvertImages.FirstOrDefault(a => a.AdvertImageId == advertImageId);
            if (advertImage != null)
            {
                context.Remove(advertImage);
                context.SaveChanges();
            }
        }        
    }
}
