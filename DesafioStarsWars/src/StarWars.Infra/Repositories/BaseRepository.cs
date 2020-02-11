using StarWars.Helpers.CustomAttributes;
using StarWars.Infra.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace StarWars.Infra.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : class, new()
    {
        protected IConnectMongo _connect { get; private set; }
        protected IMongoCollection<T> _collection { get; private set; }

        public BaseRepository(IConnectMongo connect)
        {
            _connect = connect;
            setCollection(connect);
        }

        #region Internal
        internal void setCollection(IConnectMongo connect)
        {
            MongoCollectionName mongoCollectionName = (MongoCollectionName)typeof(T)
               .GetTypeInfo()
               .GetCustomAttribute(typeof(MongoCollectionName));

            string collectionName = mongoCollectionName != null
                ? mongoCollectionName.TableName
                : typeof(T).Name.ToLower();

            _collection = _connect.Collection<T>(collectionName);

            mongoCollectionName = null;
        }
        #endregion

        #region objectId
        public ObjectId CreateObjectId(string value)
        {
            return ObjectId.Parse(value);
        }

        #endregion

        #region Add
        public T Add(T model)
        {
            _collection.InsertOne(model);
            return model;
        }

        public async Task<T> AddAsync(T model)
        {
            await _collection.InsertOneAsync(model);
            return model;
        }
        #endregion

        #region All
        public IEnumerable<T> All()
        {
            return _collection
               .AsQueryable()
               .AsEnumerable();
        }

        public IEnumerable<T> All(Expression<Func<T, bool>> filter)
        {
            return _collection
                .AsQueryable()
                .Where(filter)
                .AsEnumerable();
        }

        public async Task<IList<T>> AllAsync()
        {
            return await _collection
              .AsQueryable()
              .ToListAsync();
        }

        public async Task<IList<T>> AllAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection
                .AsQueryable()
                .Where(filter)
                .ToListAsync();
        }

        public IEnumerable<T> AllOrderBy<Tkey>(Expression<Func<T, Tkey>> orderBy)
        {
            return _collection
                .AsQueryable()
                .OrderBy(orderBy)
                .AsEnumerable();
        }

        public async Task<IList<T>> AllOrderByAsync<Tkey>(Expression<Func<T, Tkey>> orderBy)
        {
            return await _collection
                .AsQueryable()
                .OrderBy(orderBy)
                .ToListAsync();
        }

        #endregion

        #region Delete
        public bool Delete(Expression<Func<T, bool>> filter)
        {
            return _collection
                .DeleteOne(filter)
                .DeletedCount > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            DeleteResult result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
        #endregion

        #region Find
        public T Find(Expression<Func<T, bool>> filter)
        {
            return _collection
                .Find(filter)
                .FirstOrDefault();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> filter)
        {
            IAsyncCursor<T> result = await _collection
              .FindAsync(filter);
            return result
                .FirstOrDefault();
        }
        #endregion

        #region List
        public IList<T> List<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> filter = null)
        {
            IMongoQueryable<T> query = _collection.AsQueryable();
            if (filter != null)
                return query.Where(filter).OrderBy(orderBy).ToList();
            return query.OrderBy(orderBy).ToList();
        }

        public async Task<IList<T>> ListAsync<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> filter = null)
        {
            IMongoQueryable<T> query = _collection.AsQueryable();
            if (filter != null)
                return await query.Where(filter).OrderBy(orderBy).ToListAsync();
            return await query.OrderBy(orderBy).ToListAsync();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _collection = null;
                    _connect = null;
                }
                disposed = true;
            }
        }
        ~BaseRepository()
        {
            Dispose(false);
        }
        private bool disposed = false;
        #endregion Dispose                    
    }
}
