using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class PickAttributeTests
    {

        [Test]
        public void Test1()
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

        [Test]
        public void Test2()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Pick(typeof(Model3), new string[] { ""FamilyMembers"" })]
    public partial class Model3Picked { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model3Picked
    {
        public System.Collections.Generic.List<string>? FamilyMembers { get; set; }
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void ReadonlyAndPickCombination()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [
        SG1.UtilityTypes.Readonly(typeof(Model1)),
        SG1.UtilityTypes.Pick(typeof(Model1), ""FirstName"")
    ]
    public partial class ReadonlyAndPickCombination { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class ReadonlyAndPickCombination
    {
        public string FirstName { get; }
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

    }
}