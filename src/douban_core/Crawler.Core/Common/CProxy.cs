using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Crawler.Core.Common
{
    public class CProxy:IWebProxy
    {
        public CProxy(string uri) : this(new Uri(uri)) { }
        public CProxy(Uri proxyUri)
        {
            this.ProxyUri = proxyUri;
        }
        public CProxy(string uri,int port)
        {
            var builder = new UriBuilder(uri);
            if (port>0)
            {
                builder.Port = port;
            }
            this.ProxyUri=builder.Uri;
        }
        
        public Uri ProxyUri { get; set; }
        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            
            return this.ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false; /* Proxy all requests */
        }
    }
}
