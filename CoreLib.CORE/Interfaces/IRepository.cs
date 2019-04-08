#region

using System.Linq;
using System.Threading.Tasks;

#endregion

namespace CoreLib.CORE.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get();
        Task<T> GetByIdAsync(int id);
        void Create(T item);
        void Update(T item);
        Task UpdateSinglePropertyAsync(int id, string propertyName, object propertyValue);
        Task DeleteAsync(int id);
    }
}