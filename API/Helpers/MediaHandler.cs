namespace API.Helpers
{
    public class MediaHandler : IMediaHandler
    {
        public MediaHandler(IWebHostEnvironment webhost,IConfiguration config)
        {
            Webhost = webhost;
            this.config = config;
        }

        private readonly IWebHostEnvironment Webhost;
        private readonly IConfiguration config;

        public void RemoveImage(string imgPath)
        {
            ///remove the url
            string[] words = imgPath.Split("images/");
            imgPath = words[1];

            string image = Path.Combine(Webhost.WebRootPath, "images", imgPath);
            FileInfo fi = new FileInfo(image);
            if (fi != null)
            {
                System.IO.File.Delete(image);
                fi.Delete();
            }
        }

        public string UploadImage(IFormFile image)
        {
            string uniqueFileName = null;

            if (image != null)
            {
     
                string uploadsFolder = Path.Combine(Webhost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);

                }

                uniqueFileName = $"{config["ApiUrl"]}images/{uniqueFileName}";

            }

            return uniqueFileName;
        }

     
    }
}
