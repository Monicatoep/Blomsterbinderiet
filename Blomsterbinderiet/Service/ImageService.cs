namespace Blomsterbinderiet.Service
{
    public class ImageService
    {
        private IWebHostEnvironment WebHostEnvironment { get; set; }

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public byte[] ConvertToByteArray(IFormFile tempUploadedFile)
        {
            var filePath = Path.Combine(Path.Combine(WebHostEnvironment.WebRootPath, "images"), tempUploadedFile.FileName);

            using (FileStream fs = System.IO.File.Create(filePath))
            {
                tempUploadedFile.CopyTo(fs);
            }
            byte[] temp2 = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return temp2;
        }
    }
}
