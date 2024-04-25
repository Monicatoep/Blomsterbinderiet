using Microsoft.AspNetCore.Hosting;

namespace Blomsterbinderiet.Service
{
    public class Tools
    {
        private IWebHostEnvironment WebHostEnvironment { get; }

        public async Task<byte[]> ConvertToByteArray(IFormFile temp)
        {
            var filePath = Path.Combine(Path.Combine(WebHostEnvironment.WebRootPath, "images"), temp.FileName);

            using (FileStream fs = System.IO.File.Create(filePath))
            {
                temp.CopyTo(fs);
            }
            byte[] temp2 = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return temp2;
        }
    }
}
