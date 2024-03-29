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

#nullable enable
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class Model1Partial
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
    };
}
#nullable restore
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void ModelWithCommentsTest()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{

    public class ModelWithComments
    {
        /// <summary>
        /// The first name
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// The last name
        /// </summary>
        public string LastName { get; set; } = default!;
    }

    [Partial(typeof(ModelWithComments))]
    public partial class ModelWithCommentsPartial { }
}";

            string expectedOutput = @"using System;

#nullable enable
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class ModelWithCommentsPartial
    {
        /// <summary>
        /// The first name
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// The last name
        /// </summary>
        public string? LastName { get; set; }
    };
}
#nullable restore
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void PartialAttributeWithNamedArgumentsOverrideTest()
        {
            string source = @"using SG1.UtilityTypes;

namespace SampleNamespace
{
    [Partial(typeof(SG1.UtilityTypes.Tests.SampleClasses.Model1), NullableType = typeof(Microsoft.CodeAnalysis.Optional<object>), WrapAlreadyNullTypes = true)]
    public partial class Model1Partial { }
}";
            string expectedOutput = @"using System;

#nullable enable
namespace SampleNamespace
{
    public partial class Model1Partial
    {
        public Microsoft.CodeAnalysis.Optional<string> FirstName { get; set; }
        public Microsoft.CodeAnalysis.Optional<string> LastName { get; set; }
        public Microsoft.CodeAnalysis.Optional<string?> Email { get; set; }
        public Microsoft.CodeAnalysis.Optional<int> Age { get; set; }
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