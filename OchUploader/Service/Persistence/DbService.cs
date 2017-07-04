using OchUploader.DataAccess;
using System.Collections.Generic;

namespace OchUploader.Service.Persistence
{
    /// <summary>
    /// Base class providing service methods to encapsulate a repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DbServiceBase<T> : IDbService<T>
    {
        protected IRepository<T> _repo;

        public DbServiceBase(IRepository<T> repo)
        {
            _repo = repo;
        }

        public virtual IEnumerable<T> GetAll()
        {
            // Get All business objects and its dependencies
            return PerformJoins(_repo.GetAll());
        }
        public T GetById(object id)
        {
            return _repo.GetById(id);
        }
        public virtual void Update(T instance)
        {
            _repo.Update(instance);
        }
        public virtual void Remove(T instance)
        {
            _repo.Remove(instance);
        }
        public virtual void AddToDb(T instance)
        {
            if (!_repo.Contains(instance))
                _repo.Add(instance);
        }

        public virtual void Save()
        {
            _repo.Save();
        }
        public void Rollback()
        {
            _repo.ClearCache();
        }

        /// <summary>
        /// Perfoms joins of models. At default, no joins
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        protected virtual IEnumerable<T> PerformJoins(IEnumerable<T> models)
        {
            // Default, perform no joins
            return models;
        }

    }
}
