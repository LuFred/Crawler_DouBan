using System;
using Crawler.Core;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using Crawler.Core.Model;
using Crawler.Host.Components.Kafka;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Crawler.Host.Components.Mongodb;
using Crawler.Host.App_Start;
using Newtonsoft.Json;

namespace Crawler.Host
{
    public class Program
    {
        private const string DOUBAN_MOVIE_TAGS_URL = "https://movie.douban.com/tag/";
        private const string MONGODB_CONNECTION = "mongodb://localhost:50017";
        private const string MONGODB_DATABASE = "douban_movie";
        private const string MONGODB_COLLECTION_TAGS = "tags";
        private const string MONGODB_COLLECTION_INTRODUCE = "introduce";
        public static void Main(string[] args)
        {

            Configuration();
            start();
            Console.ReadLine();
        }

        public static void start()
        {

            var host = new Client();
            MongdbHelper mongoHelper = new MongdbHelper(MONGODB_CONNECTION, MONGODB_DATABASE);
            List<MovieTagModel> movieTagModelList = host.GetDouBanMovieAllTags(DOUBAN_MOVIE_TAGS_URL);

            //分类信息持久化
            foreach (var model in movieTagModelList)
            {
                mongoHelper.ReplaceOrCreateOne<MovieTagModel>(MONGODB_COLLECTION_TAGS, t => t.TagName.Equals(model.TagName), model);
            }

            for (int i = 0; i < movieTagModelList.Count; i++)
            {
                var movieTag = movieTagModelList[i];
                var nextPageUrl = movieTag.Url;                
                while (nextPageUrl != null && nextPageUrl.Length > 0)
                {
                    Console.WriteLine($"当前请求地址：{nextPageUrl}");
                    var introList = host.GetMovieIntroList(movieTag.TagName, nextPageUrl, out nextPageUrl);
                    if (introList != null&& introList.Count>0)
                    {
                        foreach (var introModel in introList)
                        {
                            Console.WriteLine($"抓取电影：{introModel.Name}");
                            mongoHelper.ReplaceOrCreateOne<MovieInfoModel>(MONGODB_COLLECTION_INTRODUCE, t => t.Name.Equals(introModel.Name), introModel);
                        }
                    }                
                   
                    Thread.Sleep(3000);
                }

            }
        }
        #region Congfiguration
        private static void Configuration()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // mongodb mapper config
            MongoDBMapConfig.Register();

        }
        #endregion


    }
}
