using NPoco;
using System.Collections.Generic;
using System.Linq;

namespace OchUploader.DataAccess
{
    /// <summary>
    /// Repository which caches/stages all changes except inserts before executing them
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CachedRepositoryBase<T> : IRepository<T>
    {

        #region Protected variables
        protected readonly SimpleDbCache<T> _cache = new SimpleDbCache<T>();

        protected IDatabase _dataBase;
        // Each Model has a [TableName]-Attribute. This is read here.
        protected string _tableName;
        #endregion

        #region Ctor
        public CachedRepositoryBase(IDatabase db)
        {
            _dataBase = db;
            _tableName = _dataBase.PocoDataFactory.TableInfoForType(typeof(T)).TableName;
            _dataBase.OpenSharedConnection();
        }
        #endregion

        #region Public Methods 
        public object Add(T instance)
        {
            _dataBase.Insert(instance);
            return instance;
        }
        public void Update(T instance)
        {
            _cache.Update(instance);
        }

        public void Remove(T instance)
        {
            _cache.Remove(instance);
        }
        public void RemoveById(object id)
        {
            var instance = GetById(id);
            _cache.Remove(instance);
        }

        public T GetById(object id)
        {
            return _dataBase.SingleById<T>(id);
        }
        public IEnumerable<T> GetAll()
        {
            var objects = _dataBase.Fetch<T>("SELECT * FROM " + _tableName).ToList();
            return objects;
        }
        public IQueryable<T> QueryAll()
        {
            var objects = _dataBase.Query<T>("SELECT * FROM " + _tableName).AsQueryable<T>();
            return objects;
        }
        public bool Contains(object id)
        {
            return _dataBase.Exists<T>(id);
        }

        /// <summary>
        /// Writes the cached changes to the database, effectively saving all
        /// </summary>
        public void Save()
        {
            using (var transaction = _dataBase.GetTransaction())
            {
                foreach (var addObj in _cache.ObjectsToAdd)
                    _dataBase.Insert(addObj);
                foreach (var updateObj in _cache.ObjectsToUpdate)
                    _dataBase.Update(updateObj);
                foreach (var deleteObj in _cache.ObjectsToRemove)
                    _dataBase.Delete<T>(deleteObj);
                transaction.Complete();
            }
            ClearCache();
        }

        public virtual void ClearCache()
        {
            _cache.Clear();
        }

        #endregion

    }
}
