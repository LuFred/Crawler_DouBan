using System;
using Crawler.Core;
using System.Text;
using System.Net.Http;

namespace Crawler.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


           

            ttt();
            Console.ReadLine();
        }

        public static void ttt()
        {
           
            var host = new Client();
            //var r=host.SearchSubject("热门", 0, 20).Result;
            host.GetDouBanMovieAllTags("https://movie.douban.com/tag/");
        }

       
    }
}
