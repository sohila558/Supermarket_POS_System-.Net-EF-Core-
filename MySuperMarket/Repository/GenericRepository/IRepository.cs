namespace MySuperMarket.Repository.GenericRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllASync();
        Task<T> GetByIdASync(int id);
        Task AddASync(T model);
        Task UpdateASync(T model);
        Task DeleteASync(T model);
    }
}
