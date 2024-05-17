using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DeleteKeywordModel : PageModel
    {
        private KeywordService KeywordService { get; set; }

        [BindProperty]
        public Models.Keyword Keyword { get; set; }

        public DeleteKeywordModel(KeywordService keywordService)
        {
            KeywordService = keywordService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Keyword = await KeywordService.GetKeywordByIdAsync(id);
            if (User == null)
            {
                return RedirectToPage("/NotFound");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await KeywordService.DeleteKeywordAsync(id);
            return RedirectToPage("/Admin/GetAllKeywords");
        }
    }
}
