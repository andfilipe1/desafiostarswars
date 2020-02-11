using MongoDB.Driver;
using System;

namespace StarWars.Infra.Interfaces
{
    public interface IConnectMongo : IDisposable
    {
        IMongoCollection<T> Collection<T>(string collectionName);
    }
}
