namespace Crawler.Core.Model
{
    /*
    *电影标签分类
    */
    public class MovieTagModel
    {
        public MovieTagModel()
        {
            CurrentCrawlUrl = Url;
        }
        /// <summary>
        /// 标签名
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 当前正在抓取
        /// </summary>
        public bool IsCrawling { get; set; }
        /// <summary>
        /// 访问地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 数据抓取完成状态
        /// </summary>
        public bool CrawlDone { get; set; }
        /// <summary>
        /// 当前抓取url
        /// </summary>
        public string CurrentCrawlUrl { get; set; }
    }
}