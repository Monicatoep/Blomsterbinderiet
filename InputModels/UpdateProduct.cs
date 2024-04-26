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

        public UpdateProduct()
        {
        }

        public UpdateProduct(Models.Product product)
        {
            ID = product.ID;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Colour = product.Colour;
            UploadedImage = product.UploadedImage;
        }

        public Models.Product UpdateParameterWithNewValues(Models.Product product)
        {
            product.ID = ID;
            product.Name = Name;
            product.Description = Description;
            product.Price = Price;
            product.Colour = Colour;
            product.UploadedImage = UploadedImage;
            if(UploadedImage != null)
            {
                product.Image = new Service.ImageService().ConvertToByteArrayAsync(UploadedImage).Result;
            }
            return product;
        }

        //public bool Disabled { get; set; }
    }
}
