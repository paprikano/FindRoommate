using FindRoommate.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FindRoommate.Services
{
    public class ImageService : IImageService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public ImageService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async void UploadImage(IFormFile file)
        {
            long totalBytes = file.Length;
            string fileName = file.FileName.Trim('"');
            fileName = EnsureFileName(fileName);
            byte[] buffer = new byte[16 * 1024];
            using (FileStream output = File.Create(GetPathAndFileName(fileName)))
            {
                using (Stream input = file.OpenReadStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, buffer.Length);
                        totalBytes += readBytes;
                    }
                }
            }
        }

        private string GetPathAndFileName(string fileName)
        {
            string path = hostingEnvironment.WebRootPath + @"\uploads\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + fileName;
        }

        private string EnsureFileName(string fileName)
        {
            if (fileName.Contains("\\"))
                fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            return fileName;
        }
    }
}
