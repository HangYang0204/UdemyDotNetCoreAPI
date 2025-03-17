using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //POST : api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadReuqestDto reqest)
        {
            ValidateFileUpload(reqest);
            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = reqest.File,
                    FileExtension = Path.GetExtension(reqest.File.FileName),
                    FileName = reqest.FileName,
                    FileDescription = reqest.FileDescription,
                };

                //Upload to local
                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadReuqestDto requst)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(requst.File.FileName)))
                ModelState.AddModelError("file", "Unsupported fle extension");

            if (requst.File.Length > 10485760)
                ModelState.AddModelError("file", "File Size is more than 10MB");

        }
    }
}
