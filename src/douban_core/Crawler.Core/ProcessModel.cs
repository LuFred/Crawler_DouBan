using Crawler.Core.Common;
using Crawler.Core.Common.Mongodb;
using Crawler.Core.Map;
using Crawler.Core.Model;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Core
{
    public static class ProcessModel
    {
       
        static ProcessModel() {
            MongoDBMapConfig.Register();
        }
        private const string MONGODB_CONNECTION = "mongodb://localhost:50017";
        private const string MONGODB_DATABASE = "douban_movie";
        private const string MONGODB_COLLECTION_TAGS = "tags";
        private const string MONGODB_COLLECTION_INTRODUCE = "introduce";
        private const string PROXY_IP_LIST = "proxy_ip";
        private static readonly MongdbHelper MongoClient = new MongdbHelper(MONGODB_CONNECTION, MONGODB_DATABASE);

        private static object TagSetLocker = new object();
        private static object TagGetLocker = new object();

        public static void SetTagList(List<MovieTagModel> movieTagModelList)
        {
            //if (movieTagModelList == null)
            //{
            //    lock (TagSetLocker)
            //    {
            //        if (movieTagModelList == null)
            //        {
            //            _movieTagModelList = movieTagModelList;
            //        }
            //    }
            //}
        }
        public static long GetTagCount(){
                long count = MongoClient.Count<MovieTagModel>(MONGODB_COLLECTION_TAGS,t=>true);
                return count;
        }
        public static MovieTagModel GetUnCrawlTag()
        {
            MovieTagModel model = null;
            lock (TagGetLocker)
            {

                MovieTagModel tagModel = MongoClient.FindOne<MovieTagModel>(MONGODB_COLLECTION_TAGS, t => !t.IsCrawling && !t.CrawlDone);
                if (tagModel != null)
                {
                    tagModel.IsCrawling = true;
                    MongoClient.ReplaceOne<MovieTagModel>("tags", t => t.TagName.Equals(tagModel.TagName), tagModel);
                }
                model = tagModel;
            }
            return model;
        }

        /// <summary>
        /// 存储标签数据
        /// </summary>
        /// <param name="model">MovieTagModel</param>
        public static void ReplaceOrCreateTag(MovieTagModel model) {
            MongoClient.ReplaceOrCreateOne(MONGODB_COLLECTION_TAGS, t => t.TagName.Equals(model.TagName), model);
        }

        public static void SaveMovieInfo(MovieInfoModel model)
        {
            MongoClient.ReplaceOrCreateOne<MovieInfoModel>(MONGODB_COLLECTION_INTRODUCE, t => t.Name.Equals(model.Name), model);
        }

        public static void SaveProxy(ProxyIPModel model) {
            MongoClient.InsertOne(PROXY_IP_LIST, model);
        }
    }
}
