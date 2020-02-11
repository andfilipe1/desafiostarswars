using StarWars.Helpers.CustomAttributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace StarWars.Model
{
    [MongoCollectionName("planetas")]
    public class Planeta
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public int IdPlaneta { get; set; }
        public string Nome { get; set; }
        public string Clima { get; set; }
        public string Terreno { get; set; }
        public int TotalFilmes { get; set; }
    }

    public class PlanetSW
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<PlanetSWResult> results { get; set; }
    }

    public class PlanetSWResult
    {
        public string name { get; set; }
        public string rotation_period { get; set; }
        public string orbital_period { get; set; }
        public string diameter { get; set; }
        public string climate { get; set; }
        public string gravity { get; set; }
        public string terrain { get; set; }
        public string surface_water { get; set; }
        public string population { get; set; }
        public List<string> residents { get; set; }
        public List<string> films { get; set; }
        public DateTime created { get; set; }
        public DateTime edited { get; set; }
        public string url { get; set; }
    }
}
