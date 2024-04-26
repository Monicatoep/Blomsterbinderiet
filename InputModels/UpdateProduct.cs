using Microsoft.AspNetCore.Mvc;

namespace Blomsterbinderiet.InputModels
{
    [ModelMetadataType(typeof(Models.Product))]
    public class UpdateProduct
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Colour { get; set; }
        public IFormFile? UploadedImage { get; set; }
        public string Colour { get; set; }
        //public bool Disabled { get; set; }
    }
}
