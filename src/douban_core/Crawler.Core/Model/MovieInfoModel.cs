using System;

namespace Crawler.Core.Model
{
    public class MovieInfoModel{
       
        /// <summary>
        /// ����
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Name{get;set;}
        /// <summary>
        ///����
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// �����ַ
        /// </summary>
        public string DetailUrl{get;set;}
        /// <summary>
        /// ����
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Rating { get; set; }
    } 
}