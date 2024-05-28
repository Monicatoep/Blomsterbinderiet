using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UpdateKeywordModel : PageModel
    {
        private KeywordService KeywordService { get; set; }

        [BindProperty]
        public Keyword Keyword { get; set; }

        public string Confirmation { get; set; }

        public UpdateKeywordModel(KeywordService keywordService)
        {
            KeywordService = keywordService;
        }

        public async Task OnGetAsync(int id)
        {
            Keyword = await KeywordService.GetKeywordByIdAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Confirmation = "Opdatering fejlede";
                return Page();
            }
            await KeywordService.UpdateKeywordAsync(Keyword);
            Confirmation = "Opdaterede produktet";
            return Page();
        }
    }
}
