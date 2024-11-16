namespace API.Helpers
{
    public interface IMediaHandler
    {
        public string UploadImage(IFormFile image);
        public void RemoveImage(string imgPath);

     


    }
}
