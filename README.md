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

```charp
[Partial(typeof(Profile))]
public partial class ProfileUpdate { }
```

and the source generator will do the rest!

## Pick

Sometimes you need a copy of a class but with only certain properties.

The Pick attribute can be used to transform a given class (let's use `Profile` from above), with the following syntax

```csharp
[Pick(typeof(Profile), new string[] { nameof(Profile.FirstName) })]
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
[Omit(typeof(Profile), new string[] { nameof(Profile.Age) })]
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
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public int Age { get; }
}
```

where all property setters have been removed. You will need to write constructors in order to initialise the properties.

## Combine them together

You can easily use these attributes together in order to get greater utility.
For example, if I wanted an update model just for the name fields of the profile above, I can do:

```csharp
[
    Partial(typeof(Profile)),
    Pick(typeof(Profile), new string[] { nameof(Profile.FirstName), nameof(Profile.LastName) })
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

## Contributing

Please raise issues using the Issue Template.

Please raise pull requests using the Pull Request Template.

If you have recommendations on improving this library, please get in touch.
