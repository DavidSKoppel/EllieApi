namespace EllieApi.Service.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetById(int id);
        Task<bool> entityExists(int id);
        Task Insert(T obj);
        Task Update(int id, Dictionary<string, object> updates);
        Task Delete(int id);
        Task Save();
    }
}