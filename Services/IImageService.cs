using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.Services
{
    public interface IImageService
    {
        void UploadImage(IFormFile file);
    }
}
