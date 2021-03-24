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
        public void CreateDinner()
        {
            var data = TestDataProvider.GetAll<DinnerModel>("STAR", "ProDinner", "Dinner");

            ProDinner
                .NavigateToDinners()
                .CreateDinner(data)
                .VerifyDinnerAdded(data);
        }
    }
}
