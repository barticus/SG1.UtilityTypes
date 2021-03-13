using NUnit.Framework;

namespace SG1.UtilityTypes.Tests
{
    public class ImplementsAttributeTests
    {

        [Test]
        public void ImplementsModel1()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Implements(typeof(Model1))]
    public partial class ImplementsModel1 { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class ImplementsModel1
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
        public void ImplementsModel3()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Implements(typeof(Model3))]
    public partial class ImplementsModel3 { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class ImplementsModel3
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
        public void InterfaceTestDefault()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Implements(typeof(IModel1))]
    public partial class InterfaceTestDefault { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class InterfaceTestDefault
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
        public void InterfaceTestReadonlyAttribute()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [
        SG1.UtilityTypes.Implements(typeof(IModel1)),
        SG1.UtilityTypes.Readonly(typeof(IModel1))
    ]
    public partial class InterfaceTestReadonlyAttribute { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class InterfaceTestReadonlyAttribute
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string? Email { get; }
        public int Age { get; }
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void InterfaceTestReadonlyProperty()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Implements(typeof(IModel1), IsReadonly = true)]
    public partial class InterfaceTestReadonlyProperty { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class InterfaceTestReadonlyProperty
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string? Email { get; }
        public int Age { get; }
    };
}
";
            string output = TestUtilities.GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void InheritedPropertiesTest()
        {
            string source = @"
namespace SG1.UtilityTypes.Tests.SampleClasses
{
    [SG1.UtilityTypes.Implements(typeof(Model1), IsReadonly = true, IncludeProperties = new string[] { ""FirstName"" })]
    public partial class InheritedPropertiesTest { }
}";

            string expectedOutput = @"using System;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public partial class InheritedPropertiesTest
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