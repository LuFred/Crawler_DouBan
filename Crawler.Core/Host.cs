using System;
using Crawler.Core.Model;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace Crawler.Core
{
    public class Client
    {
        
        public async Task<SearchSubjectModelList>  SearchSubject(string tag, int start, int limit)
        {
            var baseAddress = "https://movie.douban.com/j/search_subjects";
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            var url = $"/j/search_subjects?tag={tag}&page_start={start}&page_limit={limit}";
            var response = await client.GetAsync(url);
            var respStrData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(respStrData);
            var resp = JsonConvert.DeserializeObject<SearchSubjectModelList>(respStrData);
            foreach(var item in resp.subjects){
                Console.WriteLine($"film={item.Title},rate={item.Rate},Url={item.Url}");
            }
      
            return resp;

        }
    }
}
