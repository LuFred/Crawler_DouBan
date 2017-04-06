using System;
using System.IO;
using System.Net.Http;
using AngleSharp.Parser.Html;

namespace ConsoleApplication
{
    public class Program
    {
        public static HttpClient _httpClient;
        public static void Main(string[] args)
        {
            string[] PAGE_INDEX_LIST={"1","2","3","4","5","6","7","8","9","10"};
            string PROXY_SITE_URL="http://www.kuaidaili.com/proxylist/{0}/";
            string ipListFile=@"..\ip_file";
            string currentUrl="";
           _httpClient=new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.82 Safari/537.36");         
            _httpClient.DefaultRequestHeaders.Add("Cookie", "_ydclearance=70bd563ca06f5b3babf59e95-e031-40f0-88f1-e2a9d5283997-1491479694");
           // _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");


            for (int i=0;i<PAGE_INDEX_LIST.Length;i++){
            currentUrl=string.Format(PROXY_SITE_URL,PAGE_INDEX_LIST[i]);
           string[] ipList= GetIP(currentUrl);
           WriteFile(ipListFile,ipList);
          }
          Console.WriteLine("Done");
          Console.ReadLine();
      
        }
        //解析http://www.kuaidaili.com/proxylist/1网站的html页面，筛选出ip地址和端口
        public static string[] GetIP(string url){
             var htmlContent=GetHtml(url);             
            var document=new HtmlParser().Parse(htmlContent);
            var trSelector="#index_free_list tbody tr";
            var trListCells=document.QuerySelectorAll(trSelector);
            string[] ipList=new string[trListCells.Length];
            for(int i=0;i<trListCells.Length;i++)
            {
              var ip=  trListCells[i].QuerySelectorAll("td")[0].InnerHtml;
              var port= trListCells[i].QuerySelectorAll("td")[1].InnerHtml;
             ipList[i]=ip+":"+port;
            }
            return ipList;
        }
        //读取html文件
        public static string GetHtml(string url){
        
            var httpResponseMessage=_httpClient.GetAsync(url).Result;
           return httpResponseMessage.Content.ReadAsStringAsync().Result;

        }

        //写文件
        public static void WriteFile(string filePath,string[] strList){
           if(!File.Exists(filePath)){
               File.Create(filePath);
           }
           using(FileStream fs=new FileStream(filePath,FileMode.Open))
           {
              fs.Seek(0,SeekOrigin.End);
               for(int i=0;i<strList.Length;i++)
               {
                //获得字节数组
                byte[] data = System.Text.Encoding.UTF8.GetBytes(strList[i]+"\n"); 
                   fs.Write(data,0,data.Length);
               }
               fs.Flush();
           }
        }
    }

}
