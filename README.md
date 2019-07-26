# Strainer

Strainer is a simple, clean, and extensible framework based on .NET Standard that enables **sorting, filtering, and pagination functionality**.

**Note:** This project is a port of [Sieve](https://github.com/Biarity/Sieve) with its original author [Biarity](https://github.com/Biarity).

## Packages

Strainer is divided into following [NuGet](https://docs.microsoft.com/nuget/what-is-nuget) packages:

| Name | Framework | Version |
| --- | --- | --- |
| Strainer | .NET Standard 2.0 | [![Nuget](https://img.shields.io/nuget/v/Fluorite.Strainer.svg)](https://www.nuget.org/packages/Fluorite.Strainer/)
| Strainer.AspNetCore | .NET Standard 2.0 | [![Nuget](https://img.shields.io/nuget/v/Fluorite.Strainer.AspNetCore.svg)](https://www.nuget.org/packages/Fluorite.Strainer.AspNetCore/)

## Installation

Using .NET Core CLI:

```bash
dotnet add package Fluorite.Strainer.AspNetCore
```

Using NuGet Package Manager Console:

```bash
Install-Package Fluorite.Strainer.AspNetCore
```

## Usage in ASP.NET Core

### Add required services

In `Startup` inject Strainer services while specifying the implementation of [`IStrainerProcessor`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Services/IStrainerProcessor.cs). For starters, you can use the default implementation - [`StrainerProcessor`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Services/StrainerProcessor.cs).

```cs
services.AddStrainer<StrainerProcessor>();
```

While adding Strainer, you can configure it with available [options](#configure-strainer).

### Tell Strainer which properties to use

Strainer will only sort/filter by properties that have applied [`[Strainer]`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Attributes/StrainerAttribute.cs) attribute on them. 

In order to mark a property as **filterable** apply the `[Strainer]` attribute with `CanFilter` property set to `true`:

```cs
[Strainer(CanFilter = true)]
public int Id { get; set; }
```

Similarly **sortable** property:

```cs
[Strainer(CanSort = true)]
public int Id { get; set; }
```

For **filterable** and **sortable** property, combine both:

```cs
[Strainer(CanFilter = true, CanSort = true)]
public int Id { get; set; }
```

Set a custom display name:

```cs
[Strainer(CanFilter = true, CanSort = true, DisplayName = "identifier")]
public int Id { get; set; }
```

Alternatively, you can use [Fluent API](#fluent-api) to do the same. This is especially useful if you don't want to use attributes or have multiple APIs.

### Use Strainer to filter/sort/paginate

Calling `Apply()` will filter, sort and/or paginate the source `IQueryable` depending on model configuration.

```cs
private readonly ApplicationDbContext _dbContext;
private readonly IStrainerProcessor _strainerProcessor;

public PostsController(IStrainerProcessor strainerProcessor, ApplicationDbContext dbContext)
{
    _dbContext = dbContext;
    _strainerProcessor = strainerProcessor;
}

[HttpGet]
public JsonResult GetPosts(StrainerModel strainerModel) 
{
    var result = _strainerProcessor.Apply(strainerModel, _dbContext.Posts); 

    return Json(result);
}
```

You can also explicitly specify if only filtering, sorting, and/or pagination should be applied via optional `bool` arguments:

```cs
var result = _strainerProcessor.Apply(
    strainerModel,
    source,
    applyFiltering: true,
    applySorting: true,
    applyPagination: false);
```

or just call only one desired processing method:

```
var result = _strainerProcessor.ApplyPagination(strainerModel, source);
```

## Fluent API

You can use Fluent API instead of attributes to mark properties and even more. Setup custom `StrainerProcessor` that overrides `MapProperties()`, for example:

```cs
public class ApplicationStrainerProcessor : StrainerProcessor
{
    public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
    {

    }

    protected override void MapProperties(IStrainerPropertyMapper mapper)
    {
        mapper.Property<Post>(p => p.Title)
            .CanSort()
            .CanFilter()
            .HasDisplayName("CustomTitleName");
    }
}
```

Fluent API - as opposed to attributes - allows you to:

 - add [custom filter operators](#custom-filter-operators);
 - add [custom filter methods](#custom-filter-methods);
 - add [custom sort methods](#custom-sort-methods).

## Custom methods

In order to add custom sort/filter methods, override appropriate mapping method in your Strainer processor.

### Custom filter methods

```cs
public class ApplicationStrainerProcessor : StrainerProcessor
{
    public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
    {

    }

    protected override void MapCustomFilterMethods(ICustomFilterMethodMapper mapper)
    {
        mapper.CustomMethod<Post>(nameof(IsNew))
            .WithFunction(IsNew);
    }

    private IQueryable<Post> IsNew(ICustomFilterMethodContext<Post> context)
        => context.Source.Where(p => p.LikeCount < 100 && p.CommentCount < 5);
}
```

### Custom sort methods

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

    private IOrderedQueryable<Post> Popularity(ICustomSortMethodContext<Post> context)
    {
        return context.IsSubsequent
            ? (context.Source as IOrderedQueryable<Post>).ThenBy(p => p.LikeCount)
            : context.Source.OrderBy(p => p.LikeCount)
                .ThenBy(p => p.CommentCount)
                .ThenBy(p => p.DateCreated);
    }
}
```

Notice how conditional ordering is being performed depending on whether context's `IsSubsequent` property have `true` value. That's because Strainer supports sorting by multiple properties with no exception for custom sorting.

## Configure Strainer

Strainer comes with following [`options`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/StrainerOptions.cs):

| Name | Type | Default value | Description |
| --- | --- | --- | --- |
| CaseSensitive | `bool` | false | A `bool` value indictating whether Strainer should operatre in case sensitive mode. This affects for example the way of comparing value of incoming filters with names of properties marked as filterable. |
| DefaultPageNumber | `int` | 1 | Default page number. |
| DefaultPageSize | `int` | 10 | Default page size. |
| MaxPageSize | `int` | 50 | Maximum page number. |
| ThrowExceptions | `bool` | false | A `bool` value indictating whether Strainer should throw [StrainerExceptions](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Exceptions/StrainerException.cs) and the like. |

Use the [ASP.NET Core options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) to tell Strainer where to look for configuration. For example:

```cs
services.AddStrainer<StrainerProcessor>(Configuration.GetSection("Strainer"));
```

Then you can add Strainer configuration to `appsetting.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
   "Strainer": {
    "DefaultPageSize": 20
  }
}
```

Strainer can be also configured via [`Action`](https://docs.microsoft.com/dotnet/api/system.action-1):

```cs
services.AddStrainer<StrainerProcessor>(options => options.DefaultPageSize = 20);
```

## Sample request query

Below you can fine a **HTTP GET** request that includes a sort/filter/page query:

```curl
GET /GetPosts?sorts=LikeCount,CommentCount,-created&filters=LikeCount>10,Title@=awesome title&page=1&pageSize=10
```

That request will be translated by Strainer to:

| Name | Value | Meaning |
| --- | --- | --- |
| sorts | LikeCount,CommentCount,-created | Sort ascendingly by likes count, then ascendingly by comment count and then descendingly by creation date. |
| filters | LikeCount>10,Title@=awesome title | Filter to posts with more then 10 likes and with title containing the phrase _"awesome title"_. |
| page | 1 | Select the first page of resulted collection. |
| pageSize | 10 | Split the resulted collection into a 10-element pages. |

## Strainer model

Default Strainer model contains 4 properties:

### Sorts

* `Sorts` should be a comma-delimited list of property names to sort by. Order of properties **does matter**. Strainer by default sorts ascendingly. Adding a dash prefix (`-`) before the property name switches to sorting descendingly.

### Filters

* `filters` is a comma-delimited list of `{Name}{Operator}{Value}` where
    * `{Name}` is the name of a property with the Strainer attribute or the name of a custom filter method for TEntity
        * You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if `LikeCount` or `CommentCount` is `>10`
    * `{Operator}` is one of the [Operators](#operators)
    * `{Value}` is the value to use for filtering
        * You can also have multiple values (for OR logic) by using a pipe delimiter, eg. `Title@=new|hot` will return posts with titles that contain the text "`new`" or "`hot`"

### Page

* `page` is the number of page to return

### PageSize

* `pageSize` is the number of items returned per page 

#### Notes

* You can use backslashes (`\`) to escape commas and pipes (`|`) within value fields to enable conditional filtering;
* You can have spaces anywhere except **within** `{Name}` or `{Operator}` fields;
* If you need to look at the data before applying pagination (eg. get total count), use the optional paramters on `Apply` to defer pagination or explicitly call `ApplyPagination()` after manually counting resulted collection (an [example](https://github.com/Biarity/Sieve/issues/34));
* Here's a good example on how to work with [enumerables](https://github.com/Biarity/Sieve/issues/2);
* Another example on [how to do OR logic using pipes (`|`)](https://github.com/Biarity/Sieve/issues/8).

## Nested objects

Strainer supports filtering and sorting on nested objects' properties. Mark the property using the [Fluent API](#fluent-api). Marking via attributes is not currently supported.

For example, using `Post` and `User` models:

```cs
public class Post {
    public User Creator { get; set; }
}

public class User {
    public string Name { get; set; }
}
```

in order to `Post.User` to be filterable, override `MapProperties` in your custom Strainer processor and provide expression leading to property:

```cs
protected override void MapProperties(IStrainerPropertyMapper mapper)
{
    mapper.Property<Post>(p => p.Creator.Name)
        .CanFilter();
}
```

With such configuration, requests with `Filters` set to `User.Name==John_Doe` with tell Strainer to filter to posts with creator name being exactly _"John Doe"_.

Notice how property name is not just `Name` but it's constructed using full property path resulting in `User.Name`.

### Creating your own DSL

You can replace this DSL with your own (eg. use JSON instead) by implementing an [IStrainerModel](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/IStrainerModel.cs). You can use the default [StrainerModel](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/StrainerModel.cs) for reference.

## Filter operators

Strainer comes with following filter operators:

| Operator   | Meaning                                      |
|------------|----------------------------------------------|
| `==`       | Equals                                       |
| `!=`       | Not equals                                   |
| `>`        | Greater than                                 |
| `<`        | Less than                                    |
| `>=`       | Greater than or equal to                     |
| `<=`       | Less than or equal to                        |
| `@=`       | Contains                                     |
| `_=`       | Starts with                                  |
| `!@=`      | Does not contain                             |
| `!_=`      | Does not start with                          |
| `@=*`      | Contains _(case-insensitive)_                |
| `_=*`      | Starts with _(case-insensitive)_             |
| `==*`      | Equals _(case-insensitive)_                  |
| `!@=*`     | Does not contain _(case-insensitive)_        |
| `!_=*`     | Does not start with _(case-insensitive)_     |

### Custom Filter Operators

Same manner as adding properties you can add new filter operators. Override a mapping method in a custom Strainer processor:

```cs
public class ApplicationStrainerProcessor : StrainerProcessor
{
    public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
    {

    }

    protected override void MapFilterOperators(IFilterOperatorMapper mapper)
    {
        mapper.AddOperator(symbol: "!=*")
            .HasName("not equal to (case insensitive)")
            .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue))
            .IsCaseInsensitive();
    }
}
```

## Strainer's exceptions

Strainer will silently fail unless [`ThrowExceptions`](#configure-strainer) in the configuration is set to `true`. Following kinds of custom exceptions can be thrown:

* `StrainerMethodNotFoundException` with a `MethodName`
* `StrainerException` which encapsulates any other exception types in its `InnerException`

It is recommended that you write exception-handling middleware to globally handle Strainer's exceptions when using it with ASP.NET Core.

## Examples

You can find an example project incorporating most Strainer concepts in [Strainer.ExampleWebApi](https://gitlab.com/fluorite/strainer/tree/master/src/Strainer.ExampleWebApi).

## Migrating from Sieve to Strainer

A lot happened between Sieve v2* and Strainer v3*. Read the full migration guide [here](https://gitlab.com/fluorite/strainer/blob/master/docs/migration-guide-from-sieve-to-strainer.md).

## License & Contributing

Strainer is licensed under Apache 2.0. Any contributions highly appreciated!
