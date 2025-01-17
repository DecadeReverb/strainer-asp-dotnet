# Migration guide from Sieve to Strainer

During porting from Sieve to Strainer, the project met heavy refactoring. Whole bunch of new interfaces and services was introduced, a lot of code was split into smaller pieces. The goal was to achieve finer granularity. Framework's core functionality stayed more or less the same. Code which is more granular, it's also easier to maintain, test and extend.

### New project name - "Strainer"

As this project was forked, to avoid ambiguity a new name was established - **"Strainer"**.

Replace your `Sieve` models and services with `Strainer`.

### New namespace - Fluorite.Strainer

New project name comes with new namespace. Replace your `Sieve` usings like:

```cs
using Sieve.Services;
```

with `Fluorite.Strainer`:

```cs
using Fluorite.Strainer.Services;
```

### New attribute name

`SieveAttribute` became `StrainerPropertyAttribute`.

### New and simpler processor

In older versions of Sieve to create a custom `SieveProcessor` you would have to pass couple of services:

```cs
public class ApplicationSieveProcessor : SieveProcessor
{
    public ApplicationSieveProcessor(
        IOptions<SieveOptions> options, 
        ISieveCustomSortMethods customSortMethods, 
        ISieveCustomFilterMethods customFilterMethods) 
        : base(options, customSortMethods, customFilterMethods)
    {

    }
}
```

With Strainer an [`IStrainerContext`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Services/IStrainerContext.cs) was introduced to wrap all important services. Essentially, all you need to do is call the base constructor while passing the context:

```cs
public class ApplicationStrainerProcessor : StrainerProcessor
{
    public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
    {

    }
}
```

### Strainer Modules - division and encapsulation of configuration

Strainer introduces Modules as a way to provide and use configuration from different assemblies. Modules also make it possible to use Strainer configuration aside from Strainer service (e.g. Strainer processor).

Strainer module has configuration methods allowing registration of a property, object, additional filter operator and custom methods.

Module methods should be called from overriden `Load()` method (**not** from module constructor), like below:

```cs
public class AppStrainerModule : StrainerModule<Post>
{
    public override void Load(IStrainerModuleBuilder<Post> builder)
    {
        builder.AddProperty(p => p.Comments.Count)
            .IsFilterable()
            .IsSortable();
    }
}
```

`Load()` will be called only once on first request of resolving Strainer services for all Strainer Modules dicovered.

### Validation attributes removed from sieve model

So far, the `SieveModel` had validation attributes applied on pagination related properties:

```cs
[Range(1, int.MaxValue)]
public int? Page { get; set; }

[Range(1, int.MaxValue)]
public int? PageSize { get; set; }
```

Strainer however, does not create any initial validation requirements and leaves that task entirely to user. Validation attributes has been removed from the `Page` and `PageSize` properties, but at the same time they were made `virtual` to allow being overriden in a derived class:

```cs
public virtual int? Page { get; set; }

public virtual int? PageSize { get; set; }
```

 Eventually you can create a completly new model by implementing [`IStrainerModel`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/IStrainerModel.cs) interface.

### New structure of custom filter and sort methods

In Sieve, custom filter and sort methods were called using reflection and defined in service implementing `ISieveCustomFilterMethods` or `ISieveCustomSortMethod`.

Old custom filter method:

```cs
public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy, bool desc)
{
    var result = useThenBy
            ? ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount)
            : source.OrderBy(p => p.LikeCount)
        .ThenBy(p => p.CommentCount)
        .ThenBy(p => p.DateCreated);

    return result;
}
```

Strainer introduces new structure of adding such methods by calling appropriate method in your module and adding the method using more comfortable fluent-like API.

For example, code adding the same method from the example above in Strainer would look like this:

```cs
public class ApplicationStrainerModule : StrainerModule<Post>
{
    public override void Load(IStrainerModuleBuilder<Post> builder)
    {
        builder.AddCustomSortMethod(b => b
            .HasName("Popularity")
            .HasFunction(p => p.LikeCount)
            .Build());
    }
}
```

In the code snippet above, a custom sorting method for `Post` entity is added with a name _"Popularity"_ and a sorting function.

### Object-level attribute

In Strainer it is possible to configure filtering/sorting options on object-level, instead of configuring every property, one by one. To use that feature, apply `StrainerObject` attribute on a desired class or struct. For example, consider following class:

```cs
[StrainerObject(nameof(Id))]
public class Post
{
	public int Id { get; set; }

	public string Title { get; set; }
}
```

In the example above, marking `Post` class with `StrainerObject` attribute, will tell Strainer that all its properties are filterable and sortable (this can be configured with additional `bool` flags). While applying `StrainerObject` attribute, a name of property is required which will act as a name for fallback sorting property, when during processing no sorting information was discovered, but sorting was still requested.

Furthermore, you can combine `StrainerObject` attribute with `StrainerProperty` attribute which will override object-level defaults. For example:

```cs
[StrainerObject(nameof(Id))]
public class Post
{
	public int Id { get; set; }

	[StrainerProperty(IsSortable = false)]
	public string Title { get; set; }
}
```

Configuration above will tell Strainer that all properties of `Post` class are filterable and sortable, with exception for `Title` property which should not be sortable.

### Object level fluent API

Instead of applying an attribute, you can also mark the whole object in Fluent API in your custom module:

```C#
public class CommentStrainerModule : StrainerModule<Comment>
{
    public override void Load(IStrainerModuleBuilder<Comment> builder)
    {
        builder
            .AddObject(p => p.Id)
            .IsFilterable()
            .IsSortable();
    }
}

```

When doing so, it is required to explicitly specify whether all object properties should be filterable and sortable, same as when adding a property via `AddProperty()`. 

Notice that strongly typed `StrainerModule<T>` has been used in the example above, so there is no need for specifing model type with `AddObject()`.

### ...more will come.
