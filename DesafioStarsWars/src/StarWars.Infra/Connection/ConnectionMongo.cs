using StarWars.Infra.Interfaces;
using MongoDB.Driver;
using System;

namespace StarWars.Infra.Connection
{
    public class ConnectionMongo : IConnectMongo, IDisposable
    {
        protected MongoClient Client { get; private set; }
        protected IMongoDatabase DataBase { get; private set; }

        public IMongoCollection<T> Collection<T>(string collectionName)
        {
            return DataBase.GetCollection<T>(collectionName);
        }

        public ConnectionMongo(IConfigurationMongo config)
        {
            Client = new MongoClient(config.ConnectionString);
            DataBase = Client.GetDatabase(config.Database);
        }

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
                    DataBase = null;
                    Client = null;
                }
                disposed = true;
            }
        }
        ~ConnectionMongo()
        {
            Dispose(false);
        }
        private bool disposed = false;
        #endregion Dispose
    }
}

