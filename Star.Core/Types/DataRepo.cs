using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Star.Core.Types
{
    [BsonIgnoreExtraElements]
    public class DataRepo
    {
        public BsonObjectId Id;
        public string Environment;
        public string TargetCountry;
        public string TargetLanguage;
        public string TestDataKey;
        public BsonDocument TestInputData;
    }
}
