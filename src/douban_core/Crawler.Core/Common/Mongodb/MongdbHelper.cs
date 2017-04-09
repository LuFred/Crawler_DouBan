using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Crawler.Core.Common.Mongodb
{
    public class MongdbHelper
    {
        private MongoClient _client { get; set; }
        private IMongoDatabase _database { get; set; }
        public MongdbHelper(string conn, string dbname)
        {
            _client = new MongoClient(conn);
            _database = _client.GetDatabase(dbname);
        }
        private IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
        public List<T> Find<T>(string collectionName, Expression<Func<T, bool>> filter)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.Find(filter).ToList();
        }
        public T FindOne<T>(string collectionName, Expression<Func<T, bool>> filter)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.Find(filter).FirstOrDefault();
        }

        public void InsertOne<T>(string collectionName, T model)
        {
            var collection = GetCollection<T>(collectionName);
            collection.InsertOne(model);
        }
        public void ReplaceOne<T>(string collectionName, Expression<Func<T, bool>> filter, T model)
        {
            var collection = GetCollection<T>(collectionName);
            collection.ReplaceOne(filter, model);
        }

        public void ReplaceOrCreateOne<T>(string collectionName, Expression<Func<T, bool>> filter, T model)
        {
            var collection = GetCollection<T>(collectionName);
            T findModel = FindOne<T>(collectionName, filter);
            if (findModel != null)
            {
                ReplaceOne(collectionName, filter, model);
            }
            else
            {
                InsertOne(collectionName, model);
            }

        }

    }
}
