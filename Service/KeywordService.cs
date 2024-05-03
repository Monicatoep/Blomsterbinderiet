namespace Blomsterbinderiet.Service
{
    public class KeywordService
    {
        protected DbGenericService<Models.Keyword> DbService { get; set; }

        public async Task<IEnumerable<Models.Keyword>> GetAllDataTrackingAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Models.Keyword>> GetAllDataAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Models.Keyword> GetDataByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
