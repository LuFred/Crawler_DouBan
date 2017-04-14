using System;
using Crawler.Core.Model;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AngleSharp;
using System.Linq;
using AngleSharp.Parser.Html;
using AngleSharp.Dom.Html;
using System.Net;
using System.Collections.Generic;
using Crawler.Core.Common;
using System.IO;

namespace Crawler.Core
{
    public class Client
    {
        public static long getCount { get; set; }
        private HttpClient _httpClient;
      //  private string ipListFile = @"/src/ip_file";
      private string ipListFile=@"/Users/lujiangbo/Documents/Git/Crawler_DouB"+
"an/src/ip_file";
        public Client() {
            _httpClient = GetHttpClient(false);
        }

        public async Task<SearchSubjectModelList>  SearchSubject(string tag, int start, int limit)
        {
            var baseAddress = "https://movie.douban.com/j/search_subjects";

            _httpClient.BaseAddress = new Uri(baseAddress);
            var url = $"/j/search_subjects?tag={tag}&page_start={start}&page_limit={limit}";
            var response = await _httpClient.GetAsync(url);
            var respStrData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(respStrData);
            var resp = JsonConvert.DeserializeObject<SearchSubjectModelList>(respStrData);
            foreach(var item in resp.subjects){
                Console.WriteLine($"film={item.Title},rate={item.Rate},Url={item.Url}");
            }
      
            return resp;

        }

        public List<MovieTagModel> GetDouBanMovieAllTags(string url) {
           // var config = Configuration.Default.WithDefaultLoader();          
           // var document =BrowsingContext.New(config).OpenAsync(url).Result;
              var htmlContent=GetHtml(url);
            var document=new HtmlParser().Parse(htmlContent);
            var cellSelector = ".tagCol tr td a";
            var cells = document.QuerySelectorAll(cellSelector);
            List<MovieTagModel> movieTagModelList=new List<MovieTagModel>();
            foreach (var item in cells)
            {
                var anchorElement = (IHtmlAnchorElement)item;
                movieTagModelList.Add(new MovieTagModel(){
                    TagName=anchorElement.InnerHtml,
                    Url= WebUtility.UrlDecode(anchorElement.PathName)
                });
          }
          return movieTagModelList;

        }
    
        public  List<MovieInfoModel> GetMovieIntroList(string type,string url,out string nextUrl){
            Console.WriteLine("url="+url);
            var htmlContent=GetHtml(url);
            return DouBanAnalyze.MovieListAnalyze(type,htmlContent, out nextUrl);       
        }
       

        private string GetHtml(string url)
        {          

           var httpResponseMessage=_httpClient.GetAsync(url).Result;
           
            if (!httpResponseMessage.StatusCode.Equals(HttpStatusCode.OK))
            {
                throw new HttpRequestException($"httpStatusCode:{httpResponseMessage.StatusCode}");
            }
            getCount += 1;
            Console.WriteLine($"请求总次数{getCount}");
           return httpResponseMessage.Content.ReadAsStringAsync().Result;
        }

        private HttpClient GetHttpClient(bool useProxy=false)
        {
       string currentDirectory= Directory.GetCurrentDirectory();
           var ipstring = FSHelper.Read(ipListFile);
            string[] ipList = ipstring.Split('\n');
            HttpClientHandler config=null;
            if (useProxy)
            {
                config = new HttpClientHandler
                {
                    UseProxy = true,
                    Proxy = new CProxy("https://27.159.126.93", 8118)
                };
            }
            var httpClient = (config == null ? new HttpClient(): new HttpClient(config));
            return httpClient;
        }

    }
}
