using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Core.Model
{
    public class ProxyIPModel
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public int UseCount { get; set; }
        public bool IsUsed { get; set; }
       
    }
}
