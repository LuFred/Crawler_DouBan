using System;
using System.IO;
using System.Net.Http;
using AngleSharp.Parser.Html;
using Crawler.Core.Common;
using System.Net;
using System.Text;
using Crawler.Core;
using Crawler.Core.Model;
using System.Threading;

namespace ConsoleApplication
{
    public class Program
    {
        public static HttpClient _httpClient;
        private const string CheckIpUrl = "https://movie.douban.com/tag/";
        private const string PROXY_SITE_URL = "http://www.kuaidaili.com/free/inha/{0}/";
        private const int pageCount = 1574;
        private static int proxyCount = 0;


        public static void Main(string[] args)
        {
            Configuration();

            string currentUrl = "";
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Add("Cookie", "_ydclearance=4859e83632bb2507c4346158-a817-45a5-ba38-b475fcf9afa3-1491750881");
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

            for (int i =1; i <= 30; i++)
            {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(GetIP), i*50);
               
            }
            //for (int i = 1; i <= pageCount; i++)
            //{
            //    currentUrl = string.Format(PROXY_SITE_URL, i);
            //    GetIP(currentUrl);
             
            //}
            Console.WriteLine("Done");
            Console.ReadLine();

        }
        //解析http://www.kuaidaili.com/proxylist/1网站的html页面，筛选出ip地址和端口
        public static void GetIP(object o)
        {
            int LastPage = (int)o;
            string url = "";
            for (int j = LastPage; j >= LastPage - 50; j--)
            {
                
                url = string.Format(PROXY_SITE_URL, j);
                var htmlContent = GetHtml(url);
                var document = new HtmlParser().Parse(htmlContent);
                var trSelector = "#list tbody tr";
                var trListCells = document.QuerySelectorAll(trSelector);
                string[] ipList = new string[trListCells.Length];
                for (int i = 0; i < trListCells.Length; i++)
                {
                    var ip = trListCells[i].QuerySelectorAll("td")[0].InnerHtml;
                    var port = Convert.ToInt32(trListCells[i].QuerySelectorAll("td")[1].InnerHtml);
                    if (CheckProxy(ip, port))
                    {
                        ProcessModel.SaveProxy(new ProxyIPModel()
                        {
                            Ip = ip,
                            Port = port,
                            IsUsed = true,
                            UseCount = 0
                        });
                    }
                }
            }

        }
        //读取html文件
        public static string GetHtml(string url)
        {

            var httpResponseMessage = _httpClient.GetAsync(url).Result;
            return httpResponseMessage.Content.ReadAsStringAsync().Result;

        }

        public static bool CheckProxy(string ip, int port)
        {

            bool result = false;
            HttpClientHandler config = new HttpClientHandler
            {
                UseProxy = true,
                Proxy = new CProxy(ip, port),

            };
            var _httpClient = new HttpClient(config);
            _httpClient.Timeout = TimeSpan.FromMilliseconds(10000);
            int i = 0;
            while (i<3)
            {
              
                try
                {
                    var httpResponseMessage = _httpClient.GetAsync(CheckIpUrl).Result;
                    if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        // Console.WriteLine($"{ip}:{port}可用");
                        result = true;
                        break;
                    }
                }
                catch (Exception e)
                {

                }
                           
                i += 1;
            }
            proxyCount += 1;
            Console.WriteLine($"线程{Thread.CurrentThread.ManagedThreadId}  筛选第{proxyCount}个IP: {ip}:{port} 尝试：{i}次后{(result ? "可用" : "不可用")}");

            return result;
        }

        //写文件
        public static void WriteFile(string filePath, string[] strList)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                fs.Seek(0, SeekOrigin.End);
                for (int i = 0; i < strList.Length; i++)
                {
                    //获得字节数组
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(strList[i] + "\n");
                    fs.Write(data, 0, data.Length);
                }
                fs.Flush();
            }
        }

        #region Congfiguration
        private static void Configuration()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

          
        }
        #endregion
    }

}
