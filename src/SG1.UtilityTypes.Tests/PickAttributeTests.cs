using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class PickAttributeTests
    {

        [Test]
        public void PickAttributeTest()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Pick(typeof(Model1), new string[] { ""FirstName"" })]
    public partial class Model1Partial { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Partial
    {
        public string FirstName { get; set; } = default!;
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

    }
}