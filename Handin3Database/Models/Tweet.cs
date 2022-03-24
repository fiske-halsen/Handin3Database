using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handin3Database.Models
{
    [BsonIgnoreExtraElements]
    public class Tweet
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("created_at")]
        public string Created_at { get; set; }
        [BsonElement("text")]
        public string Text { get; set; }
        [BsonElement("source")]
        public string Source { get; set; }
    }
}
