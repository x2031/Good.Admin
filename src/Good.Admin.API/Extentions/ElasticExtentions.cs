﻿using Elasticsearch.Net;
using Nest;
using Nest.JsonNetSerializer;


namespace Good.Admin.API
{
    public static class ElasticExtentions
    {

        public static Uri CreateUri(string node)
        {
            return new Uri(node);
        }
        public static ConnectionSettings CreateconnentionSettiongs(LogOptions options)
        {
            //List<Uri> uris = new List<Uri>();
            //options.Elasticsearch.Nodes.ForEach(node => uris.Add(new Uri(node)));
            var pool = new SingleNodeConnectionPool(new Uri(options.Elasticsearch.Nodes[0]));
            return new ConnectionSettings(
                connectionPool: pool,
                sourceSerializer: (buildin, settings) => new JsonNetSerializer(buildin, settings, () => new Newtonsoft.Json.JsonSerializerSettings()
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects,
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                }))
                .DisableDirectStreaming()
                .DefaultIndex(options.Elasticsearch.DefaultIndex);
        }
    }

}
