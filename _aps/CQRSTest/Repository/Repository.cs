using CQRSTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CQRSTest.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ProductContext context;

        public Repository(ProductContext context)
        {
            this.context = context;
        }

        public async Task<List<T>> ReadAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<(List<T>, int)> ReadAllFilterAsync(int skip, int take)
        {
            var all = context.Set<T>();
            var relevant = await all.Skip(skip).Take(take).ToListAsync();
            var total = all.Count();

            (List<T>, int) result = (relevant, total);

            return result;
        }

        public async Task<T> ReadAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }             

            context.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }                

            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }                

            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
