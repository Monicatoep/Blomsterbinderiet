using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.MockData;
using Blomsterbinderiet.Service;

namespace Blomsterbinderiet.Pages.Product
{
    public class GetAllProductsModel : PageModel
    {
        private DbGenericService<Models.Product> DbService { get; set; }

        public GetAllProductsModel(DbGenericService<Models.Product> dbService)
        {
            DbService = dbService;
        }

        public List<Models.Product> Products { get; private set; } = new List<Models.Product>();

		public void OnGet()
        {
            Products = DbService.GetObjectsAsync().Result.ToList();
        }
    }
}
