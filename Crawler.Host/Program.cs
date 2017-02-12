using System;
using Crawler.Core;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using Crawler.Core.Model;

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
        List<MovieTagModel> movieTagModelList= host.GetDouBanMovieAllTags("https://movie.douban.com/tag/");
        foreach(var t in movieTagModelList){
            Console.WriteLine(t.TagName);
        }
        for(int i=0;i<movieTagModelList.Count;i++){
            var nextPageUrl=movieTagModelList[i].Url;
            List<MovieInfoModel> infoList=new  List<MovieInfoModel>();
            while(nextPageUrl!=null&&nextPageUrl.Length>0){
                var list= host.GetMovieInfo(nextPageUrl,out nextPageUrl);
                infoList.AddRange(list);
            }
            
        }
        }

       
    }
}
