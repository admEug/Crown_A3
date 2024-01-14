namespace CQRSTest.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> ReadAllAsync();
        Task<(List<T>, int)> ReadAllFilterAsync(int skip, int take);
        Task<T> ReadAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
