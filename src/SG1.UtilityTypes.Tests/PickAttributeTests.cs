using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class PickAttributeTests
    {

        [Test]
        public void PickedModel1Test()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Pick(typeof(Model1), new string[] { ""FirstName"" })]
    public partial class PickedModel1 { }
}";

            string expectedOutput = @"using System;

#nullable enable
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class PickedModel1
    {
        public string FirstName { get; set; } = default!;
    };
}
#nullable restore
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void PickedModel3()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Pick(typeof(Model3), new string[] { ""FamilyMembers"" })]
    public partial class PickedModel3 { }
}";

            string expectedOutput = @"using System;

#nullable enable
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class PickedModel3
    {
        public System.Collections.Generic.List<string>? FamilyMembers { get; set; }
    };
}
#nullable restore
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

#nullable enable
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class ReadonlyAndPickCombination
    {
        public string FirstName { get; }
    };
}
#nullable restore
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void PickedModel1WithAttributesTest()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Pick(typeof(Model1), new string[] { ""FirstName"" }, IncludeAttributes = true)]
    public partial class PickedModel1 { }
}";

            string expectedOutput = @"using System;

#nullable enable
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class PickedModel1
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(123)]
        public string FirstName { get; set; } = default!;
    };
}
#nullable restore
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }
    }
}