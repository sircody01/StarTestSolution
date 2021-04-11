using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Star.Core.WebDriver;

namespace Star.Core.Types
{
    public struct TestResultsMetaData
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id;                                 // Will automatically be filled in by MongoDB, do not fill in.
        // Metadata about the executed test. Information comes from attributes applied to the test.
        public string TestName;                         // The fully qualified name of the test.
        [BsonIgnoreIfDefault]
        public string Description;                      // The short description of the test.
        [BsonIgnoreIfDefault]
        public List<int> TestCaseIds;                   // List of source control workitems associated with the test. Should be at least one identifying the test case ID.
        [BsonIgnoreIfDefault]
        public List<string> TestCategories;             // List of test categories associated with the test.

        // Metadata about the executed test. Information comes from application settings
        [BsonIgnoreIfDefault]
        public string Environment;                      // The test environment the test ran in.
        [BsonIgnoreIfDefault]
        public string Country;                          // The 2 letter country code the test run in e.g. US, CA, MX
        [BsonIgnoreIfDefault]
        public string Language;                         // The region the test ran in.

        // Metadata about the executed test. Information captured at runtime.
        [BsonRepresentation(BsonType.String)]
        public WebDriverType Browser;                   // The browser used to run the test.
        [BsonIgnoreIfDefault]
        public string BrowserVersion;                   // The version of the browser used.
        [BsonIgnoreIfDefault]
        public string MachineName;                      // The name of the machine running the test code (not the Selenium grid node).
        [BsonIgnoreIfDefault]
        public string OS;                               // The OS the browser was running on.
        public bool OnCi;                               // Whether or not the test executed on a CICD system.
        [BsonIgnoreIfDefault]
        public object TestInputData;                    // The test data that was used to drive the test
        public DateTime TimeRun;                        // The UTC time the test started execution.
        public double TestDuration;                     // How long the test took to execute in seconds.
        public string Outcome;                          // The outcome of the test, e.g. Passed, Failed, Timed out.
        public string[] Log;                            // The test log

        // Metadata about the executed test. Infomration is captured from the CICD system, if run on a CICD system.
        [BsonIgnoreIfDefault]
        public int CicdBuildId;                         // If the test ran on a CICD system, the ID of the CICD build.
        [BsonIgnoreIfDefault]
        public int CicdPipelineId;                      // If the test ran on a CICD system, the pipeline id.
        [BsonIgnoreIfDefault]
        public int ProjectId;                           // The source control system project ID, if applicable.
        [BsonIgnoreIfDefault]
        public string BranchName;                       // The source control system branch name, if applicable.

        public List<BsonObjectId> Files;                // List of file ID's stored in GridFS that belong with this test result. Files can include screenshots, test logs, recorded videos.
    }
}
