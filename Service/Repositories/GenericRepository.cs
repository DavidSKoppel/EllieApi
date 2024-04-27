using EllieApi.Models;
using EllieApi.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EllieApi.Service.Repositories
{
    public class GenericRepository<T, TDBContext> : IGenericRepository<T>
    where T : class
    where TDBContext : ElliedbContext
    {

        protected ElliedbContext _context;

        public GenericRepository(ElliedbContext context)
        {
            this._context = context;
        }

        public async Task Insert(T obj)
        {
            _context.Add(obj);
            await Save();
        }
        public async Task Update(int id, Dictionary<string, object> updates)
        {
            T t = await _context.Set<T>().FindAsync(id);

            foreach (var update in updates)
            {
                var field = t.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(update.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && field.CanWrite)
                {
                    if (update.Value == null)
                    {
                        field.SetValue(t, null);
                    }
                    else
                    {
                        field.SetValue(t, ChangeType(update.Value.ToString(), field.PropertyType));
                    }
                }
            }

            _context.Entry(t).State = EntityState.Modified;
            await Save();
        }
        public async Task Delete(int id)
        {
            var entityToDelete = _context.Set<T>().Find(id);
            _context.Remove(entityToDelete);
            await Save();
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<bool> entityExists(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            if (result != null)
            {
                _context.ChangeTracker.Clear();
                return true;
            }
            else
            {
                _context.ChangeTracker.Clear();
                return false;
            }

        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
    }
}