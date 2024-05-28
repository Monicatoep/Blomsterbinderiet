using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class GetAllKeywordsModel : PageModel
    { 
        private KeywordService KeywordService { get; set; }
        
        public IEnumerable<Keyword> Keywords { get; set; }

        public GetAllKeywordsModel(KeywordService keywordService)
        {
            KeywordService = keywordService;
        }

        public async Task OnGetAsync()
        {
            Keywords = (await KeywordService.GetAllKeywordsAsync()).OrderBy(k => k.Name).ToList();
        }
    }
}
