using System;
using System.Text.Json;
using ConsoleApp.Models;
using SG1.UtilityTypes;

namespace ConsoleApp
{

    [Partial(typeof(Model1))]
    public partial class Model1Partial { }

    [Pick(typeof(Model1), "FirstName")]
    public partial class Model1Picked { }

    [Omit(typeof(Model1), "FirstName")]
    public partial class Model1Omit { }

    [
        Readonly(typeof(Model1)),
        Pick(typeof(Model1), "FirstName")
    // TODO: Fix this combo
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

    [
        Partial(typeof(Model1)),
        Partial(typeof(Model2)),
        Partial(typeof(Model3)),
    ]
    public partial class Model1And2And3
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            WriteType(typeof(Model1Partial));
            WriteType(typeof(Model1Picked));
            WriteType(typeof(Model1Readonly));
            WriteType(typeof(Model1And2));
            WriteType(typeof(Model1Omit));
            WriteType(typeof(Model1And2And3));

            var parsed = JsonSerializer.Deserialize<Model1And2And3>("{}");
            Console.WriteLine(JsonSerializer.Serialize(parsed, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        static void WriteType(Type type)
        {
            Console.WriteLine($"Properties on {type.Name}:");
            foreach (var property in type.GetProperties())
            {
                Console.WriteLine($"\t{property.Name} of type {property.PropertyType.Name}");
            }
        }
    }
}
