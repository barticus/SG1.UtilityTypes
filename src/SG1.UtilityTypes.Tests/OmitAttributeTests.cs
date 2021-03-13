using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class OmitAttributeTests
    {

        [Test]
        public void Model1OmittedArray()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Omit(typeof(Model1), new string[] { ""FirstName"" })]
    public partial class Model1OmittedArray { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1OmittedArray
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

        [Test]
        public void Model1OmittedParams()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Omit(typeof(Model1), ""FirstName"")]
    public partial class Model1OmittedParams { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1OmittedParams
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

        [Test]
        public void PartialAndOmittedCombination()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [
        SG1.UtilityTypes.Partial(typeof(Model1)),
        SG1.UtilityTypes.Omit(typeof(Model1), ""FirstName"")
    ]
    public partial class PartialAndOmittedCombination { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class PartialAndOmittedCombination
    {
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }
    }
}