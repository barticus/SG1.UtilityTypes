using System;
using SG1.UtilityTypes;

namespace ConsoleApp
{
    public class Model1
    {
        /// <summary>
        /// The first name
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// The last name
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
        /// The email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The age
        /// </summary>
        public int Age { get; set; }
    }

    public class Model2
    {
        /// <summary>
        /// The users Bio
        /// </summary>
        public string Bio { get; set; } = default!;

        /// <summary>
        /// The users Bio
        /// </summary>
        public string Website { get; set; } = default!;
    }

    [Partial(typeof(Model1))]
    public partial class Model1Partial { }

    [Pick(typeof(Model1), new string[] { "FirstName" })]
    public partial class Model1Picked { }

    [Omit(typeof(Model1), new string[] { "FirstName" })]
    public partial class Model1Omit { }

    [
        Readonly(typeof(Model1)),
        Pick(typeof(Model1), new string[] { "FirstName" })
    ]
    public partial class Model1Readonly
    {
        public Model1Readonly(
            string firstName
        )
        {
            FirstName = firstName;
        }
    }
    [
        Partial(typeof(Model1)),
        Partial(typeof(Model2)),
    ]
    public partial class Model1And2
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            var partial = new Model1Partial
            {
                FirstName = null,
                LastName = "tester",
                Email = "testere",
            };
            var picked = new Model1Picked
            {
                FirstName = "test"
            };
            var ro = new Model1Readonly("test");
            var model1And2 = new Model1And2
            {
                FirstName = "test",
                LastName = "tester",
                Email = "testere",
                Age = 10,
                Bio = "All about me",
                Website = "https://google.com/me"
            };
            var omitted = new Model1Omit
            {
                LastName = "tester",
                Email = "testere",
                Age = 10
            };
            Console.WriteLine(
                partial.FirstName +
                partial.LastName +
                partial.Email
            );
        }
    }
}
