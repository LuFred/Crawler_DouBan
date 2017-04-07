using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Crawler.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Crawler.Core
{
    public class DouBanAnalyze
    {
        /// <summary>
        /// 解析电影列表
        /// </summary>
        /// <param name="type">电影分类</param>
        /// <param name="htmlContent">html</param>
        /// <param name="nextUrl">下一页地址</param>
        /// <returns></returns>
        public static List<MovieInfoModel> MovieListAnalyze(string type,string htmlContent,out string nextUrl) {
            var document = new HtmlParser().Parse(htmlContent);            
            var NBGSelector = ".article .item";                    
            var movieListCells = document.QuerySelectorAll(NBGSelector);        
            List<MovieInfoModel> movieInfoModelList = new List<MovieInfoModel>();
            foreach (var item in movieListCells)
            {
              var infoModel=  new MovieInfoModel();
                infoModel.Type = type;
                #region 标题 封面 详情地址
                IElement NBGElement = item.QuerySelector(".nbg");
                var NBGAnchorElement = (IHtmlAnchorElement)NBGElement;
                var ImgCoverElement = ((IHtmlImageElement)NBGAnchorElement.FirstElementChild);
                infoModel.Name = NBGAnchorElement.Title;
                infoModel.Cover = ImgCoverElement.Source;
                infoModel.DetailUrl = WebUtility.UrlDecode(NBGAnchorElement.Href);
                #endregion
                #region 简介
                IElement IntroElement = item.QuerySelector(".pl");
                infoModel.Introduction = IntroElement.InnerHtml;
                #endregion
                #region  分数
                IElement RatingElement = item.QuerySelector(".rating_nums");
                if (RatingElement!=null)
                {
                    infoModel.Rating = RatingElement.InnerHtml;
                }
                else
                {
                    infoModel.Rating ="0";
                }
               
                #endregion
                movieInfoModelList.Add(infoModel);
            }
            #region 下一页地址
            var nextPageUrlSelector = ".article .paginator .next a";
            var nextLinkDom = document.QuerySelectorAll(nextPageUrlSelector).LastOrDefault();           
            nextUrl = nextLinkDom != null ? ((IHtmlAnchorElement)nextLinkDom).Href : "";
            #endregion
            return movieInfoModelList;
        }
    }
}
