using StarWars.Infra.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Infra.Connection
{
    public class ConfigurationMongo : IConfigurationMongo
    {
        public ConfigurationMongo(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection("StarWarsDB");
            ConnectionString = section["ConnectionStrings"];
            Database = section["Database"];
        }

        public string ConnectionString { get; private set; }

        public string Database { get; private set; }
    }
}
