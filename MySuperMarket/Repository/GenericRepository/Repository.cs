using Microsoft.EntityFrameworkCore;
using MySuperMarket.Data;

namespace MySuperMarket.Repository.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public async Task AddASync(T model)
        {
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteASync(T model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllASync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdASync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateASync(T model)
        {

            _dbSet.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
