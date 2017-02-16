using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Crawler.Core.Model
{
    public class SearchSubjectModelList{
        public List<SearchSubjectModel> subjects{get;set;}
    }

    [DataContract]
    public class SearchSubjectModel
    {
        [DataMember(Name = "rate")]
        public string Rate { get; set; }
        [DataMember(Name = "is_beetle_subject")]
        public bool IsBeetleSubject { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "cover")]
        public string Cover { get; set; }
        [DataMember(Name = "id")]
        public string Id
        {
            get; set;
        }
        [DataMember(Name = "is_new")]
        public bool IsNew { get; set; }
    }

}