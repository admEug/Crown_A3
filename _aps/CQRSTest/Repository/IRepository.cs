namespace CQRSTest.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ReadAllAsync();
        Task<T> ReadAsync(int id);
        Task<(IEnumerable<T>, int)> ReadAllFilterAsync(int skip, int take);        
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
