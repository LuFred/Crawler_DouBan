using System;

namespace Crawler.Core.Model
{
    public class MovieInfoModel{
       
        /// <summary>
        /// 分类
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name{get;set;}
        /// <summary>
        ///封面
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        public string DetailUrl{get;set;}
        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public string Rating { get; set; }
    } 
}