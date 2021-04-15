using System.ComponentModel.DataAnnotations;

namespace SG1.UtilityTypes.Tests.SampleClasses
{
    public interface IModel1
    {
        string FirstName { get; }
        string LastName { get; }
        string? Email { get; }
        int Age { get; }
    }

    public class Model1 : IModel1
    {
        /// <summary>
        /// The first name
        /// </summary>
        [Required, MaxLength(123)]
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// The last name
        /// </summary>
        [Required, MaxLength(321)]
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