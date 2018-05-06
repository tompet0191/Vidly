using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vidly.Models
{
    [BsonIgnoreExtraElements]
    public class Movie
    {
        public ObjectId id { get; set; }
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime AddedDate { get; set; }
        public int Available { get; set; }
    }
}
