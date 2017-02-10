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

namespace Crawler.Core
{
    public class Client
    {
        private HttpClient _httpClient;
        public Client() {
            _httpClient = new HttpClient();
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

        public void GetDouBanMovieAllTags(string url) {
            var config = Configuration.Default.WithDefaultLoader();          
            var document =BrowsingContext.New(config).OpenAsync(url).Result;
            var cellSelector = ".tagCol tr td a";
            var cells = document.QuerySelectorAll(cellSelector);
            foreach (var item in cells)
            {
                var anchorElement = (IHtmlAnchorElement)item;
                Console.WriteLine(anchorElement.InnerHtml+" : "+ document.BaseUrl.Origin+WebUtility.UrlDecode(anchorElement.PathName));
            }
        }


        private string GetHtml(string url)
        {            
           var httpResponseMessage=_httpClient.GetAsync(url).Result;
           return httpResponseMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
