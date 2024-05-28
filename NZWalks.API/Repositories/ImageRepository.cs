using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IWebHostEnvironment webHost;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ImageRepository(NZWalksDbContext dbContext, IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.webHost = webHost;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Image> UploadImageAsync(Image image)
        {
            //project solution directory/images/filename.extension
            var localFilePath = Path.Combine(webHost.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            using var stream = new FileStream(localFilePath, FileMode.Create);

            await image.File.CopyToAsync(stream);

            //APIURL/images/filename.extension
            var urlFilePath = $"{httpContextAccessor?.HttpContext?.Request.Scheme}://" +
                $"{httpContextAccessor?.HttpContext?.Request.Host}/Images/{image.FileName}{image.FileExtension}";
            image.FileUrlPath = urlFilePath;

            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }
    }
}
