using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
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

        public static Guid PublishTestResults(TestResultsMetaData metaData, string dbName, string collectionName)
        {
            var bsonWriter = new BsonDocumentWriter(new BsonDocument(), BsonDocumentWriterSettings.Defaults);
            BsonSerializer.Serialize(bsonWriter, metaData);
            var bsonTestResults = bsonWriter.Document.AsBsonDocument;

            var collection = new MongoClient(ApplicationSettings.MongoDBConnectionString)
                .GetDatabase(dbName)
                .GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(bsonTestResults);
            return bsonTestResults["_id"].AsGuid;
        }

        public static ObjectId StoreFile(string dbName, string file, FileBucketType bucketType, Dictionary<string, string> metaData)
        {
            var bucket = new GridFSBucket(new MongoClient(ApplicationSettings.MongoDBConnectionString)
                .GetDatabase(dbName));
            var fSUploadOptions = new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "ContentType", bucketType.ToString() }
                }
            };
            fSUploadOptions.Metadata.AddRange(metaData);

            var stream = new FileStream(file, FileMode.Open);
            var id = bucket.UploadFromStream(file, stream, fSUploadOptions);
            stream.Close();
            return id;
        }

        public static ObjectId StoreFile(string dbName, string datumName, string datum, FileBucketType bucketType, Dictionary<string, string> metaData)
        {
            var bucket = new GridFSBucket(new MongoClient(ApplicationSettings.MongoDBConnectionString)
                .GetDatabase(dbName));
            var fSUploadOptions = new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "ContentType", bucketType.ToString() }
                }
            };
            fSUploadOptions.Metadata.AddRange(metaData);

            var id = bucket.UploadFromBytes(datumName, Encoding.ASCII.GetBytes(datum ?? ""), fSUploadOptions);
            return id;
        }

        public enum FileBucketType
        {
            Html,
            Screenshots,
            Videos
        }

        internal static void AddTestIds(string dbName, List<BsonObjectId> files, Guid resultId)
        {
            var collection = new MongoClient(ApplicationSettings.MongoDBConnectionString)
                .GetDatabase(dbName)
                .GetCollection<FsRepo>("fs.files");
            var update = Builders<FsRepo>.Update.Set("metadata.resultId", new BsonBinaryData(resultId, GuidRepresentation.Standard));
            foreach (var fileId in files)
            {
                collection.UpdateOne(d => d._id == fileId, update);
            }
        }

        #endregion
    }
}