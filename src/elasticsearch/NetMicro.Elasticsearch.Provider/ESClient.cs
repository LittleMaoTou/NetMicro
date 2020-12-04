using Elasticsearch.Net;
using Nest;

namespace NetMicro.Elasticsearch
{
    public class ESClient : ElasticClient
    {
        public ESClient(IConnectionPool pool)
            : base(new ConnectionSettings(pool))
        {
        }
    }
}
