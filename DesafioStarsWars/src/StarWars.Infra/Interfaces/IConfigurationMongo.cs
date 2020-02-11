namespace StarWars.Infra.Interfaces
{
    public interface IConfigurationMongo
    {
        string ConnectionString { get; }
        string Database { get; }
    }
}
