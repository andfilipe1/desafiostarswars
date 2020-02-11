using StarWars.Infra.Connection;
using StarWars.Infra.Interfaces;
using StarWars.Model;

namespace StarWars.Infra.Repositories
{
    public class PlanetaRepository : BaseRepository<Planeta>, IPlanetaRepository
    {
        public PlanetaRepository(IConnectMongo connection)
            :base(connection)
        {

        }
    }
}
