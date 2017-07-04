using System.Collections.Generic;
using System.Linq;

namespace OchUploader.DataAccess
{
    public interface IRepository<T>
    {
        object Add(T instance);
        void Update(T instance);
        void Remove(T instance);
        void RemoveById(object id);
        bool Contains(object id);
        T GetById(object id);
        IEnumerable<T> GetAll();
        IQueryable<T> QueryAll();

        void Save();
        void ClearCache();
    }
}
