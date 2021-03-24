using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Star.Core.Types;

namespace Star.Core.Utilities
{
    public class TestDataProvider
    {
        #region Public methods

        /// <summary>
        /// Fetch a document from the MongoDB server, deserialize it and return it to the calling test.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        /// <param name="branch"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static T GetAll<T>(string dbName, string collectionName, string testDataKey)
        {
            DataRepo doc;

#if DEBUG
            dbName += "-Sandbox";
#endif
            var collection = new MongoClient(ApplicationSettings.MongoDBConnectionString)
                .GetDatabase(dbName)
                .GetCollection<DataRepo>(collectionName);
            try
            {
                doc = collection.Find(d => d.Environment == ApplicationSettings.TargetEnvironment &&
                    d.TargetCountry == ApplicationSettings.TargetCountry &&
                    d.TargetLanguage == ApplicationSettings.TargetLanguage &&
                    d.TestDataKey == testDataKey).First();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
            return BsonSerializer.Deserialize<T>(doc.TestInputData);
        }

        /// <summary>
        /// Deserialize a BSON document and return it to the calling test.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static T GetAll<T>(BsonDocument doc)
        {
            return BsonSerializer.Deserialize<T>(doc);
        }

        #endregion
    }
}