using System;
using Crawler.Core;
namespace Crawler.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
              
            ttt();
            Console.WriteLine("OK");
            Console.ReadLine();
        }

        public static void ttt()
        {
        Console.WriteLine("tttttt");
    var host = new Client();
     var   r=    host.SearchSubject("热门", 0, 20).Result;
        }

    }
}
