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

namespace Crawler.Core
{
    public class Client
    {
        private HttpClient _httpClient;
        private string ipListFile = @"..\..\ip_file";
        public Client() {
            _httpClient = GetHttpClient(true);
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
            var config = Configuration.Default.WithDefaultLoader();          
            var document =BrowsingContext.New(config).OpenAsync(url).Result;
            var cellSelector = ".tagCol tr td a";
            var cells = document.QuerySelectorAll(cellSelector);
            List<MovieTagModel> movieTagModelList=new List<MovieTagModel>();
            foreach (var item in cells)
            {
                var anchorElement = (IHtmlAnchorElement)item;
                movieTagModelList.Add(new MovieTagModel(){
                    TagName=anchorElement.InnerHtml,
                    Url= document.BaseUrl.Origin+WebUtility.UrlDecode(anchorElement.PathName)
                });
          }
          return movieTagModelList;
        
        }
        public  List<MovieInfoModel> GetMovieInfo(string url,out string nextUrl){
            Console.WriteLine("url="+url);
            var htmlContent=GetHtml(url);
            var document=new HtmlParser().Parse(htmlContent);
            var movieListSelector=".article .item .nbg";
            var movieListCells=document.QuerySelectorAll(movieListSelector);
        Console.WriteLine("ct="+movieListCells.Count());
            List<MovieInfoModel> movieInfoModelList=new List<MovieInfoModel>();
             foreach (var item in movieListCells)
            {
                var anchorElement = (IHtmlAnchorElement)item;
                movieInfoModelList.Add(new MovieInfoModel(){
                    MovieName=anchorElement.Title,
                    MovieDetailUrl= WebUtility.UrlDecode(anchorElement.Href)
                });
                Console.WriteLine("name="+anchorElement.Title+";  url="+WebUtility.UrlDecode(anchorElement.Href));
          }
          var nextPageUrlSelector=".article .paginator .next a";
        var nextLinkDom=  document.QuerySelectorAll(nextPageUrlSelector).LastOrDefault();
            Console.WriteLine((IHtmlAnchorElement)nextLinkDom);
          nextUrl=nextLinkDom!=null?((IHtmlAnchorElement)nextLinkDom).Href:"";
          Console.WriteLine("next page="+nextUrl);
          return movieInfoModelList;
        

        }

        private string GetHtml(string url)
        {            
           var httpResponseMessage=_httpClient.GetAsync(url).Result;
            if (!httpResponseMessage.StatusCode.Equals(HttpStatusCode.OK))
            {
               
            }
           return httpResponseMessage.Content.ReadAsStringAsync().Result;
        }
        private HttpClient GetHttpClient(bool useProxy=false)
        {
           var ipstring = FSHelper.Read(ipListFile);
            string[] ipList = ipstring.Split('\n');
            HttpClientHandler config=null;
            if (useProxy)
            {
                config = new HttpClientHandler
                {
                    UseProxy = true,
                    Proxy = new CProxy(ipList[0].Split(':')[0], Convert.ToInt32(ipList[0].Split(':')[1]))
                };
            }
            var httpClient = (config == null ? new HttpClient(): new HttpClient(config));
            return httpClient;
        }

    }
}
