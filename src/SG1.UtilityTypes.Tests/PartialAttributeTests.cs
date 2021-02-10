using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class PartialAttributeTests
    {

        [Test]
        public void PartialAttributeWithDefaultsTest()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [Partial(typeof(Model1))]
    public partial class Model1Partial { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Partial
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public Nullable<int> Age { get; set; }
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void PartialAttributeWithOverrideTest()
        {
            string source = @"using SG1.UtilityTypes;

namespace SampleNamespace
{
    [Partial(typeof(SG1.UtilityTypes.Tests.SampleClasses.Model1), ""Optional"", true)]
    public partial class Model1Partial { }
}";
            string expectedOutput = @"using System;

namespace SampleNamespace
{
    public partial class Model1Partial
    {
        public Optional<string> FirstName { get; set; }
        public Optional<string> LastName { get; set; }
        public Optional<string?> Email { get; set; }
        public Optional<int> Age { get; set; }
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

    }
}