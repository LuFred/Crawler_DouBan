using Crawler.Core.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Crawler.Host.App_Start
{
    public class MongoDBMapConfig
    {

        public static void Register()
        {
            BsonClassMap.RegisterClassMap<MovieTagModel>(cm =>
            {
                cm.MapMember(c => c.TagName).SetElementName("name");
                cm.MapMember(c => c.Url).SetElementName("type");
               
            });
            BsonClassMap.RegisterClassMap<MovieInfoModel>(cm =>
            {
                cm.MapMember(c => c.Name).SetElementName("name");
                cm.MapMember(c => c.Type).SetElementName("type");
                cm.MapMember(c => c.Cover).SetElementName("cover");
                cm.MapMember(c => c.DetailUrl).SetElementName("detailUrl");
                cm.MapMember(c => c.Introduction).SetElementName("introduction");
                cm.MapMember(c => c.Rating).SetElementName("rating");
                //cm.MapExtraElementsMember(c => c.Id);
              //  cm.MapIdMember(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
            });
        }
    }
}
