using System.Collections.Generic;

namespace OchUploader.Service.Persistence
{
    /// <summary>
    /// Defines the basic CRUD-methods each service has to implement
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDbService<T>
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Update(T instance);
        void Remove(T instance);
        void Save();
        void Rollback();
        void AddToDb(T instance);
    }
}
