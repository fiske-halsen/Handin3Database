using Handin3Database.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handin3Database.Repository
{
    public class TweetRepository
    {
        private readonly IConfiguration _configuration;
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Tweet> _tweetCollection;

        public TweetRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new MongoClient(_configuration.GetConnectionString("TweetsMongoDB"));
            _database = _client.GetDatabase("test");
            _tweetCollection = _database.GetCollection<Tweet>("tweets");
        }

        public string Test()
        {
            var connectionString = _configuration.GetConnectionString("TweetsMongoDB");

            MongoClient dbClient = new MongoClient(connectionString);

            var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
            return "";
        }

        public async Task<List<Tweet>> GetTweets()
        {
            return await _tweetCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<List<string>> GetMapReducedTweets()
        {
            string map = @"
                     function() {
                     var tweet = this;
                     var text = tweet.text

                    var hashTags = text.split(' ').filter(v => v.startsWith('#'))
                    for (let i = 0; i < hashTags.length; i++) {
                    emit(hashTags[i], { count: 1 });
                    }
                     }";

            string reduce = @"        
                                function(key, values) {
                                    var result = {count: 0, hashTag: 0 };
                                    values.forEach(function(value){               
                                        result.count += value.count;
                                        result.hashTag = key;
                                    });
                                    return result;
                                }";

            var builder = Builders<Tweet>.Filter;

            var options = new MapReduceOptions<Tweet, BsonDocument>();
            options.OutputOptions = MapReduceOutputOptions.Inline;


            var results = _tweetCollection.MapReduce(map, reduce, options);

            var list = results.ToList().OrderByDescending(x => x[1]).ToList().Take(10);

            return list.Select(e => e.ToJson()).ToList();
        }
    }
}
