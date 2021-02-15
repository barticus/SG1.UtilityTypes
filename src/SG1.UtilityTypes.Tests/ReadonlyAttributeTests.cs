using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class ReadonlyAttributeTests
    {

        [Test]
        public void ReadonlyAttributeTest()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Readonly(typeof(Model1))]
    public partial class Model1Partial { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Partial
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

        [Test]
        public void Test2()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [
        SG1.UtilityTypes.Partial(typeof(Model1)),
        SG1.UtilityTypes.Readonly(typeof(Model1))
    ]
    public partial class Model1Partial { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Partial
    {
        public string? FirstName { get; }
        public string? LastName { get; }
        public string? Email { get; }
        public Nullable<int> Age { get; }
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
        SG1.UtilityTypes.Partial(typeof(Model1)),
        SG1.UtilityTypes.Readonly(typeof(Model1)),
        SG1.UtilityTypes.Pick(typeof(Model1), ""FirstName"")
    ]
    public partial class Model1Partial { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Partial
    {
        public string? FirstName { get; }
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }
    }
}