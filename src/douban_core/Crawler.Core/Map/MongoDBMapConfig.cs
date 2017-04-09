using Crawler.Core.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Crawler.Core.Map
{
    public class MongoDBMapConfig
    {

        public static void Register()
        {
            BsonClassMap.RegisterClassMap<MovieTagModel>(cm =>
            {
                cm.MapMember(c => c.TagName).SetElementName("name");
                cm.MapMember(c => c.Url).SetElementName("type");
                cm.MapMember(c => c.IsCrawling).SetElementName("is_crawling");
                cm.MapMember(c => c.CrawlDone).SetElementName("crawl_done");
                cm.MapMember(c => c.CurrentCrawlUrl).SetElementName("current_crawl_url");

            });
            BsonClassMap.RegisterClassMap<MovieInfoModel>(cm =>
            {
                cm.MapMember(c => c.Name).SetElementName("name");
                cm.MapMember(c => c.Type).SetElementName("type");
                cm.MapMember(c => c.Cover).SetElementName("cover");
                cm.MapMember(c => c.DetailUrl).SetElementName("detailUrl");
                cm.MapMember(c => c.Introduction).SetElementName("introduction");
                cm.MapMember(c => c.Rating).SetElementName("rating");
               
            });
            BsonClassMap.RegisterClassMap<ProxyIPModel>(cm =>
            {
                cm.MapMember(c => c.Ip).SetElementName("ip");
                cm.MapMember(c => c.Port).SetElementName("port");
                cm.MapMember(c => c.UseCount).SetElementName("use_count");
                cm.MapMember(c => c.IsUsed).SetElementName("is_used");               

            });
        }
    }
}
