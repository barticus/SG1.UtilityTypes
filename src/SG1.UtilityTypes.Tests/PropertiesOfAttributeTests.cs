using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class PropertiesOfAttributeTests
    {

        [Test]
        public void Test1()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.PropertiesOf(typeof(Model1))]
    public partial class Model1Partial { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Partial
    {
        public string FirstName { get; set; } = default!;
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
        public void Test2()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.PropertiesOf(typeof(Model3))]
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
        public void Test3()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [
        SG1.UtilityTypes.Readonly(typeof(Model1)),
        SG1.UtilityTypes.PropertiesOf(typeof(Model1))
    ]
    public partial class Model1Picked { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Picked
    {
        public string FirstName { get; } = default!;
        public string LastName { get; } = default!;
        public string? Email { get; }
        public int Age { get; } = default!;
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

    }
}