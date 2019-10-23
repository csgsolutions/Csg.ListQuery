using Csg.ListQuery.Server;
using Csg.ListQuery.Server;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Csg.ListQuery.JsonApi.Abstractions
{
    public class JsonApiListResponse<TAttributes> : ListResponse<JsonApiRecord<TAttributes>>
    {
        [JsonProperty("data")]
        public override IEnumerable<JsonApiRecord<TAttributes>> Data { get => base.Data; set => base.Data = value; }
        
        [JsonProperty("links")]
        public override ListResponseLinks Links { get => base.Links; set => base.Links = value; }

        [JsonProperty("meta")]
        public override ListResponseMeta Meta { get => base.Meta; set => base.Meta = value; }
    }
}
