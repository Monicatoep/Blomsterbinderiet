using Microsoft.AspNetCore.Hosting;
using static Azure.Core.HttpHeader;

namespace Blomsterbinderiet.Service
{
    public class ImageService
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

        //public static async Task<byte[]> ConvertToByteArray2(IFormFile temp)
        //{
           
        //    var filePath = Path.Combine(Path.Combine(WebHostEnvironment.WebRootPath, "images"), temp.FileName);

        //    using (FileStream fs = System.IO.File.Create(filePath))
        //    {
        //        temp.CopyTo(fs);
        //    }
        //    byte[] temp2 = System.IO.File.ReadAllBytes(filePath);
        //    System.IO.File.Delete(filePath);
        //    return temp2;
        //}
    }
}
