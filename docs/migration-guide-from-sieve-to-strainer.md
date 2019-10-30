# Migration guide from Sieve to Strainer

During porting from Sieve to Strainer, the project met heavy refactoring. Whole bunch of new interfaces and services was introduced, a lot of code was split into smaller pieces. The goal was to achieve finer granularity. No worries - all this without a change in framework's core functionality. Code which is more granular, it's also easier to maintain, test and extend.

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

Strainer introduces new structure of adding such methods by overriding appropriate method in your processor and there adding the method using more comfortable fluent-like API.

For example, code adding the same method from the example above in Strainer would look like this:

```cs
public class ApplicationStrainerProcessor : StrainerProcessor
{
    public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
    {

    }

    protected override void MapCustomSortMethods(ICustomSortMethodMapper mapper)
    {
        mapper.CustomMethod<Post>(nameof(Popularity))
            .WithFunction(Popularity);
    }

    private IQueryable<Post> Popularity(ICustomSortMethodContext<Post> context)
    {
        return context.IsSubsequent
            ? (context.Source as IOrderedQueryable<Post>).ThenBy(p => p.LikeCount)
            : context.Source.OrderBy(p => p.LikeCount)
                .ThenBy(p => p.CommentCount)
                .ThenBy(p => p.DateCreated);
    }
}
```

Some explantation of what is going on above. In `MapCustomSortMethods()` a custom sorting method for `Post` entity is added with a name _"Popularity"_. Then a transforming function is specified. Although `WithFunction()` method requires an argument of `Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>>`, you can easily provide a whole method which returns an `IQueryable` and has an `ICustomSortMethodContext` parameter. In that way you can seperete your sorting logic in a dedicated method which can be made private.

`MapCustomSortMethods()` will be called on `StrainerProcessor` initialization, alongside with other similar methods like `MapCustomFilterMethods()`.

### ...more will come.
