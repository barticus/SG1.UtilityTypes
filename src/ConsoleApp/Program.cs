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

    [Partial(typeof(Model1))]
    public partial class Model1Partial { }

    class Program
    {
        static void Main(string[] args)
        {
            var partial = new Model1Partial
            {
                FirstName = "test",
                LastName = "tester",
                Email = "testere",
            };
            Console.WriteLine(
                partial.FirstName +
                partial.LastName +
                partial.Email
            );
        }
    }
}
