
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //POST: api/image/upload
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                // Map dto to domain model
                var image = new Image
                {
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    File = request.File
                };  

                await imageRepository.UploadImageAsync(image);
                return Ok(image);

            }

           return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var maxFileSizeInBytes = 10485760;
            if(allowedExtensions.Contains(Path.GetExtension(request.File.FileName)) == false)
            {
               ModelState.AddModelError("FileExtension", "Invalid file extension");
            }
            if (request.File.Length > maxFileSizeInBytes)
            {
                ModelState.AddModelError("FileSizeInBytes", "File size is too large, max 10mb");
            }

        }

    }
}
