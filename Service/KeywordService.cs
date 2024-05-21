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
            Keywords = keywordsDbService.GetObjectsAsync().Result.OrderBy(k => k.Name).ToList();
        }

        public async Task<IEnumerable<Keyword>> GetAllKeywordsAsync()
        {
            var keywords = await KeywordsDbService.GetObjectsAsync();
            return keywords.OrderBy(k => k.Name).ToList();
        }

        public async Task AddKeywordAsync(Keyword keyword)
        {
            Keywords.Add(keyword);
            await KeywordsDbService.AddObjectAsync(keyword);
        }

        public async Task<Keyword> DeleteKeywordAsync(int id)
        {
            Keyword keywordToBeDeleted = KeywordsDbService.GetObjectByIdAsync(id).Result;
            Keywords.Remove(keywordToBeDeleted);
            await KeywordsDbService.DeleteObjectAsync(keywordToBeDeleted);
            return keywordToBeDeleted;
        }

        public async Task<Keyword?> GetKeywordByIdAsync(int id)
        {
            return await KeywordsDbService.GetObjectByIdAsync(id);
        }

        public async Task UpdateKeywordAsync(Keyword keyword)
        {
            await KeywordsDbService.UpdateObjectAsync(keyword);
        }
    }
}
