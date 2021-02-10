using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class OmitAttributeTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OmitAttributeTest()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Omit(typeof(Model1), new string[] { ""FirstName"" })]
    public partial class Model1Partial { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Partial
    {
        public string LastName { get; set; } = default!;
        public string? Email { get; set; }
        public int Age { get; set; } = default!;
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }
    }
}