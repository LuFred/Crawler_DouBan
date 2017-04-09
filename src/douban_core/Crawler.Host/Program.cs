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

using Crawler.Host.App_Start;
using Newtonsoft.Json;
using Crawler.Core.Common;
using System.Net;
using Crawler.Core.Common.Mongodb;

namespace Crawler.Host
{
    public class Program
    {
        private const string DOUBAN_MOVIE_TAGS_URL = "https://movie.douban.com/tag/";
        private static Client CoreClient = new Client();

        public static void Main(string[] args)
        {

            Configuration();
            start();
            //test();
            Console.ReadLine();
        }

        public static void start()
        {
            var host = new Client();
           
           var movieTagModelList = CoreClient.GetDouBanMovieAllTags(DOUBAN_MOVIE_TAGS_URL);
            //分类信息持久化
            foreach (var model in movieTagModelList)
            {
                ProcessModel.ReplaceOrCreateTag(model);
            }

            for (int i = 0; i < 1; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetTagMovie), null);
            }           
        }        
        public static void GetTagMovie(Object o) {

          var tagModel=ProcessModel.GetUnCrawlTag();
            var nextPageUrl = tagModel.Url;
            var host = new Client();
            int selectIndex = 0;
            bool errorTag = false;
            while (nextPageUrl != null && nextPageUrl.Length > 0)
            {
                selectIndex += 1;
                if (selectIndex == 5)
                {
                    tagModel.CurrentCrawlUrl = nextPageUrl;
                    ProcessModel.ReplaceOrCreateTag(tagModel);
                }
                try
                {
                    Console.WriteLine($"当前请求地址：{nextPageUrl}");
                    var introList = CoreClient.GetMovieIntroList(tagModel.TagName, nextPageUrl, out nextPageUrl);
                    if (introList != null && introList.Count > 0)
                    {
                        foreach (var introModel in introList)
                        {
                            Console.WriteLine($"线程：{Thread.CurrentThread.ManagedThreadId} 标签：{tagModel.TagName} 电影：{introModel.Name}");
                            ProcessModel.SaveMovieInfo(introModel);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"线程：{Thread.CurrentThread.ManagedThreadId} 标签：{tagModel.TagName}  发成错误:{e.Message}");
                    tagModel.IsCrawling = false;
                    tagModel.CrawlDone = false;
                    tagModel.CurrentCrawlUrl = nextPageUrl;
                    ProcessModel.ReplaceOrCreateTag(tagModel);
                    errorTag = true;
                    throw;
                }               
                if (!errorTag&&nextPageUrl == null|| nextPageUrl.Length<1)
                {

                    tagModel.CrawlDone = true;                                       
                    ProcessModel.ReplaceOrCreateTag(tagModel);
                }              
             
                Thread.Sleep(3000);
            }

        }
        #region Congfiguration
        private static void Configuration()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // mongodb mapper config
           // MongoDBMapConfig.Register();

        }
        #endregion

        public static void test() {
            HttpClientHandler config = null;
            if (true)
            {
                config = new HttpClientHandler
                {
                    UseProxy = true,
                    Proxy = new CProxy("27.159.126.93", 8118)
                };
            }

           
            var _httpClient = (config == null ? new HttpClient() : new HttpClient(config));
            var httpResponseMessage = _httpClient.GetAsync("https://movie.douban.com/tag/%E5%96%9C%E5%89%A7").Result;
            if (!httpResponseMessage.StatusCode.Equals(HttpStatusCode.OK))
            {

            }
           
        }
    }
}
