using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Star.Core.Types
{
    [BsonIgnoreExtraElements]
    public class DataRepo
    {
        public BsonObjectId _id { get; set; }
        public string Environment { get; set; }
        public string TargetCountry { get; set; }
        public string TargetLanguage { get; set; }
        public string TestDataKey { get; set; }
        public BsonDocument TestInputData { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "These names are set by MongoDB")]
    public class FsRepo
    {
        public BsonObjectId _id { get; set; }
        public int length { get; set; }
        public int chunkSize { get; set; }
        public DateTime uploadDate { get; set; }
        public string md5 { get; set; }
        public string filename { get; set; }
        public BsonDocument metadata { get; set; }
    }
}
