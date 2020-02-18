using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.Infrastructure
{
    public interface IImageService
    {
        void UploadImage(IFormFile file);
    }
}
