#nullable enable
namespace ConsoleApp.Models
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
}
#nullable restore