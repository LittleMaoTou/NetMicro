using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace NetMicro.Elasticsearch
{
    public class ESConnectionPool : SniffingConnectionPool
    {
        public ESConnectionPool(IOptions<ESConfig> options)
            : base(options.Value.Uris.Select(uri => new Uri(uri)))
        {
        }
    }
}
