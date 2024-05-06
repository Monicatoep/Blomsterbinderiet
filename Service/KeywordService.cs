using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class KeywordService
    {
        public List<Keyword> Keywords { get; set; }
        private DbGenericService<Keyword> KeywordsDbService { get; set; }

        public KeywordService(DbGenericService<Keyword> keywordsDbService)
        {
            KeywordsDbService = keywordsDbService;
            Keywords = keywordsDbService.GetObjectsAsync().Result.ToList();
        }

        public async Task<IEnumerable<Keyword>> GetAllKeywordsAsync()
        {
            return await KeywordsDbService.GetObjectsAsync();
        }

        public async Task AddKeywordAsync(Keyword keyword)
        {
            Keywords.Add(keyword);
            await KeywordsDbService.AddObjectAsync(keyword);
        }
    }
}
