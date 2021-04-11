using NUnit.Framework;
using Star.Core.Utilities;
using Star.Models;

namespace Star.Tests
{
    [Parallelizable(scope: ParallelScope.All)]
    [TestFixture]
    public class DinnerPageTests : BaseStarTests
    {
        [Test]
        [Description("Verifies that a new dinner can be correctly created.")]
        [Property("TestCaseIds", 1)]
        [Property("TestCaseIds", 2)]
        [Property("TestCaseIds", 3)]
        [Category("Create")]
        public void CreateDinner()
        {
            var data = TestDataProvider.GetAll<DinnerModel>("STAR", "ProDinner", "Dinner");
            CaptureTestInputData(data);

            ProDinner
                .NavigateToDinners()
                .CreateDinner(data)
                .VerifyDinnerAdded(data);
        }
    }
}
