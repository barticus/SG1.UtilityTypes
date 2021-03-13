# SG1.UtilityTypes

Utility Type C# Source Generators, inspired by Typescript

## Getting Started

This library is available as a [nuget package](https://www.nuget.org/packages/SG1.UtilityTypes/)

```bash
dotnet add package SG1.UtilityTypes
```

_NOTE_ that this library is a work in progress and you should perform your own testing to confirm it is fit for production.

## Partials

Ever had a case where you have a base model like:

```csharp
public class Profile
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
}
```

And you need to write an identical class that only takes a partial set of data, like so:

```csharp
public partial class ProfileUpdate
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Nullable<int> Age { get; set; }
}
```

Commonly used for updates, etc.

The `Partial` Utility Type can help you!
Instead of manually keeping your update model up to date, you can simply write

```csharp
[Partial(typeof(Profile))]
public partial class ProfileUpdate { }
```

and the source generator will do the rest!

## Pick

Sometimes you need a copy of a class but with only certain properties.

The Pick attribute can be used to transform a given class (let's use `Profile` from above), with the following syntax

```csharp
[Pick(typeof(Profile), nameof(Profile.FirstName))]
public partial class ProfilePickedFirstName { }
```

will produce a generated source of:

```csharp
public partial class ProfilePickedFirstName
{
    public string FirstName { get; set; } = default!;
}
```

where FirstName is the only property included on the new class.

## Omit

Sometimes you need a copy of a class but with a certain property hidden.

The Omit attribute can be used to transform a given class (let's use `Profile` from above), with the following syntax

```csharp
[Omit(typeof(Profile), nameof(Profile.Age))]
public partial class ProfileOmittedAge { }
```

will produce a generated source of:

```csharp
public partial class ProfileOmittedAge
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
```

where Age is no longer a property of the type

## Readonly

Sometimes you need a readonly version of a class so that none of the properties can be updated.

The Readonly attribute can be used to transform a given class (let's use `Profile` from above), with the following syntax

```csharp
[Readonly(typeof(Profile))]
public partial class ProfileReadonly { }
```

will produce a generated source of:

```csharp
public partial class ProfileReadonly
{
    public string FirstName { get; }
    public string LastName { get; }
    public int Age { get; }
}
```

where all property setters have been removed. You will need to write constructors in order to initialise the properties.

## Implements

Sometimes you want to implement a model interface. The ImplementsAttribute can help out with keeping your code DRY.

Given the interface

```csharp
public interface IProfile
{
    string FirstName { get; }
    string LastName { get; }
    int Age { get; set; }
}
```

```csharp
[Implements(typeof(IProfile))]
public partial class ProfileImplementation { }
```

will produce a generated source of:

```csharp
public partial class ProfileImplementation
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set;  } = default!;
    public int Age { get; set; }
}
```

By default, setters will be added. This behaviour can be overridden by adding the `IsReadonly = true` parameter to the attribute.

## PropertiesOf

This attribute will allow you to copy properties from a source type with no other transformation applied.
For example if I had the Profile class above, as well as this class

```csharp
public partial class ExtendedProfile
{
    public string Bio { get; set; } = default!;
    public string Website { get; set; } = default!;
}
```

I could combine the properties of both

```csharp
[
    PropertiesOf(typeof(Profile)),
    PropertiesOf(typeof(ExtendedProfile)),
]
public partial class FullProfile { }
```

will produce a generated source of:

```csharp
public partial class ProfileImplementation
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set;  } = default!;
    public int Age { get; set; }
    public string Bio { get; set; } = default!;
    public string Website { get; set; } = default!;
}
```

## Combine them together

You can easily use these attributes together in order to get greater utility.
For example, if I wanted an update model just for the name fields of the profile above, I can do:

```csharp
[
    Partial(typeof(Profile)),
    Pick(typeof(Profile), nameof(Profile.FirstName), nameof(Profile.LastName))
]
public partial class ProfileNamesUpdateModel { }
```

And I will get

```csharp
public partial class ProfileNamesUpdateModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
```

I can even combine properties from two models together, for example if I had the second class

```csharp
public partial class ExtendedProfile
{
    public string Bio { get; set; } = default!;
    public string Website { get; set; } = default!;
}
```

I can make a unified update model by having:

```csharp
[
    Partial(typeof(Profile)),
    Partial(typeof(ExtendedProfile))
]
public partial class FullProfileUpdate { }
```

which will produce

```csharp
public partial class ProfileNamesUpdateModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public string? Website { get; set; }
}
```

As there may be some conflicting rules, the "latest attribute" will break any ties.

Transformations are grouped by source type before being resolved.

## Common Attribute Parameters

All transform attributes inherit the following parameters which you can use to modify the behaviour of the attribute you're using.

These attributes are:

1. `IsReadonly` This named parameter controls whether setters will be added to properties. If you are setting this to `true`, it is a shorthand for also adding the `[Readonly]` attribute. Setting this to false will add setters to properties that did not have them, such as if you wanted to implement an interface.
2. `IncludeProperties` This named parameter controls the properties that should be included from the Source type. This is a shorthand for also adding the `[Pick]` attribute
3. `ExcludeProperties` This named parameter controls the properties that should be excluded from the Source type. This is a shorthand for also adding the `[Omit]` attribute

## Contributing

Please raise issues using the Issue Template.

Please raise pull requests using the Pull Request Template.

If you have recommendations on improving this library, please get in touch.
