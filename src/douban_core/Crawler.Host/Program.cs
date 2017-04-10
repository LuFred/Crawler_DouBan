using System;
using Crawler.Core;
using System.Text;
using System.Collections.Generic;
using Crawler.Core.Model;
using System.Threading;
using Crawler.Core.Common;
using System.Net;
using System.Net.Http;


namespace Crawler.Host
{
    public class Program
    {
        private const string DOUBAN_MOVIE_TAGS_URL = "https://movie.douban.com/tag/";
        private const string DOUBAN_MOVIE_DOMAIN_URL = "https://movie.douban.com";
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
          long tagCount=ProcessModel.GetTagCount();
          if(tagCount<1)
            {
            var movieTagModelList = CoreClient.GetDouBanMovieAllTags(DOUBAN_MOVIE_TAGS_URL);
                        //分类信息持久化
                        foreach (var model in movieTagModelList)
                        {
                            model.Url=DOUBAN_MOVIE_DOMAIN_URL+model.Url;
                            model.CurrentCrawlUrl=model.Url;
                            ProcessModel.ReplaceOrCreateTag(model);
                        }
            }           

            for (int i = 0; i < 1; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetTagMovie), null);
            }           
        }        
        public static void GetTagMovie(Object o) {

          var tagModel=ProcessModel.GetUnCrawlTag();

            var nextPageUrl = tagModel.CurrentCrawlUrl;
          
            int selectIndex = 0;
            bool errorTag = false;
            while (nextPageUrl != null && nextPageUrl.Length > 0)
            {
                selectIndex += 1;
                if (selectIndex == 5)
                {
                    tagModel.CurrentCrawlUrl = nextPageUrl;
                    ProcessModel.ReplaceOrCreateTag(tagModel);
                    selectIndex=0;
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
                    tagModel=ProcessModel.GetUnCrawlTag();
                    nextPageUrl = tagModel.CurrentCrawlUrl;
                }              
             
                Thread.Sleep(1000);
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
