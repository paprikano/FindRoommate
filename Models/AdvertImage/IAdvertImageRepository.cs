using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FindRoommate.Models.AdvertImage
{
    public interface IAdvertImageRepository
    {
        IQueryable<AdvertImage> AdvertImages { get; }

        void UploadAdvertImages(List<IFormFile> files, int advertId);

        void DeleteAdvertImage(int advertImageId);
    }
}
