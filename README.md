![Strainer icon](https://i.imgur.com/2wXq8Lu.png)

# Strainer

Strainer is a simple, clean and extensible framework based on .NET Standard that makes **sorting**, **filtering** and **pagination** trivial.

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

Add Strainer services in `Startup`:

```cs
services.AddStrainer();
```

While adding Strainer, you can configure it with available [options](#configure-strainer).

### Tell Strainer which properties to use

Strainer will filter/sort by properties that have applied [`[StrainerProperty]`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Attributes/StrainerPropertyAttribute.cs) attribute on them. 

In order to mark a property as **filterable** and **sortable**, simply apply the `[StrainerProperty]` attribute:

```cs
[StrainerProperty]
public int Id { get; set; }
```

Set a custom display name:

```cs
[StrainerProperty(DisplayName = "identifier")]
public int Id { get; set; }
```

Mark property as sortable, but not filterable:

```cs
[StrainerProperty(IsFilterable = false)]
public int Id { get; set; }
```

##### Object-level attribute

You can also use [`[StrainerObject]`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Attributes/StrainerObjectyAttribute.cs) attribute to set default values on object level (note that you have to provide name for default sorting property):

```cs
[StrainerObject(nameof(Id))]
public class Post
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Title { get; set; }
}
```

Alternatively, you can use [Fluent API](#fluent-api) to do the same. This is especially useful if you don't want to/can't use attributes or have multiple APIs.

### Use Strainer to filter/sort/paginate

In the example below, Strainer processor is injected to a controller. Then, `Apply()` is called causing the source collection to be processed. Strainer processor will filter, sort and/or paginate the source `IQueryable<T>` depending on provided model.

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

Instead of calling `Apply()` from `IStrainerProcess` explicitly, you can also chain an extension method for `IQueryable<T>`:

```cs
var result = _dbContext
    .Posts
    .Where(p => !p.IsDeleted)
    .Apply(strainerModel, _strainerProcessor)
    .ToList();
```

You can explicitly specify whether only filtering, sorting, and/or pagination should be applied via optional `bool` arguments:

```cs
var result = _strainerProcessor.Apply(
    strainerModel,
    source,
    applyFiltering: true,
    applySorting: true,
    applyPagination: false);
```

or just call only particular processing method:

```
var result = _strainerProcessor.ApplyPagination(strainerModel, source);
```

This is especially useful when you want to count the items in the resulting collection before pagination is applied:

```cs
var result = _strainerProcessor.Apply(strainerModel, questions, applyPagination: false);
Request.HttpContext.Response.Headers.Add("X-Total-Count", result.Count().ToString());
result = _strainerProcessor.ApplyPagination(strainerModel, result);
```

## Fluent API

You can use Fluent API instead of attributes to mark properties, objects and even more. Start with adding your own configuration module deriving from `StrainerModule`:

```cs
public class ApplicationStrainerModule : StrainerModule
{
    public override void Load(IStrainerModuleBuilder builder)
    {
        builder.AddProperty<Post>(p => p.Title)
            .IsFilterable()
            .IsSortable();
    }
}
```

Then, notify Strainer to use it in `Startup`:

```cs
services.AddStrainer(new[] { typeof(ApplicationStrainerModule) });
```

Fluent API - as opposed to attributes - allows you to:

 - add [custom filter operators](#custom-filter-operators);
 - add [custom filter methods](#custom-filter-methods);
 - add [custom sort methods](#custom-sort-methods).

## Configure Strainer

Strainer comes with following [`options`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/StrainerOptions.cs):

| Name | Type | Default value | Description |
| --- | --- | --- | --- |
| DefaultPageNumber | `int` | 1 | Default page number. |
| DefaultPageSize | `int` | 10 | Default page size. |
| DefaultSortingWay | `SortingWay` (`Ascending` or `Descending`) | `Ascending` | An enum value used when applying default sorting. |
| IsCaseInsensitiveForValues | `bool` | false | A `bool` value indicating whether Strainer should operate in case-insensitive mode when comparing `string` values. This affects for example the way of comparing filter value with `string` value of an actual property. |
| MaxPageSize | `int` | 50 | Maximum page number. |
| ThrowExceptions | `bool` | false | A `bool` value indicating whether Strainer should throw [StrainerExceptions](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Exceptions/StrainerException.cs) and the like. |

Use the [ASP.NET Core options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) to tell Strainer where to look for configuration. For example:

```cs
services.AddStrainer(Configuration.GetSection("Strainer"));
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
services.AddStrainer(options => options.DefaultPageSize = 20);
```

## Strainer Modules

Strainer builds its configuration from modules (except for attribute-based metadata which is retrieved at runtime). Once created, the configuration cannot be changed and is read-only. Modules provide a way to split the configuration sources into different assemblies. They also make it possible to use the configuration aside from Strainer services like Strainer processor.

Strainer module loads via a builder allowing to add a property, object, additional filter operator and custom methods. Strainer modules has two built-in abstract implementations of `IStrainerModule`, that are ready for use:
 - `StrainerModule`
 - `StrainerModule<T>` - narrowed down module for adding rules only for `T`.

```cs
public class AppStrainerModule : StrainerModule
{
    public override void Load(IStrainerModuleBuilder builder)
    {
        builder.AddProperty<Post>(p => p.Title)
            .IsFilterable()
            .IsSortable();
    }
}
```

`Load()` will be called once on application startup (ASP.NET Core) per every Strainer module discovered based on types or assemblies provided.

### Attributes vs Fluent API

Marking properties with fluent API needs to be explicit. Calling just `AddProperty<T>()` **is not equivalent** with applying `[StrainerProperty]` on a property. `[StrainerProperty]` attribute will mark the property as filterable and sortable **by default** (unless explicitly configured otherwise) while fluent API requires calling `IsFilterable()` and `IsSortable()` right after `AddProperty<T>()`. Same rule applies with object-level attributes and fluent API method.

Equivalent configuration for marking `Title` property as filterable and sortable:

```cs
public class Post
{
    [StrainerProperty]
    public string Title { get; set; }
}
```

```cs
public SampleStrainerModule : StrainerModule<Post>
{
    public override void Load(IStrainerModuleBuilder<Post> builder)
    {
        builder
            .AddProperty(p => p.Title)
            .IsFilterable()
            .IsSortable();
    }
}
```

Equivalent configuration for marking all `Post` properties as filterable and sortable:

```C#
[StrainerObject(nameof(Title))]
public class Post
{
    public string Title { get; set; }
}
```

```C#
public class PostStrainerModule : StrainerModule<Post>
{
    public override void Load(IStrainerModuleBuilder<Post> builder)
    {
        builder
            .AddObject(p => p.Title)
            .IsFilterable()
            .IsSortable();
    }
}
```

## Sample request query

Below you can find a sample **HTTP GET** request that includes a sort/filter/page query:

```curl
GET /GetPosts?sorts=LikeCount,CommentCount,-created&filters=LikeCount>10,Title@=awesome title&page=1&pageSize=10
```

That request will be translated by Strainer to:

| Name | Value | Meaning |
| --- | --- | --- |
| sorts | LikeCount,CommentCount,-created | Sort by like count in ascending order, then by comment count in ascending order and then by creation date in descending order. |
| filters | LikeCount>10,Title@=awesome title | Filter to posts with more than 10 likes and with title containing the phrase _"awesome title"_. |
| page | 1 | Select the first page of resulted collection. |
| pageSize | 10 | Split the resulted collection into 10-element pages. |

## Strainer model

Strainer model is based on four properties:

### Sorts

`Sorts` is a comma-delimited list of property names to sort by. The order of properties **does matter**. Strainer by default sorts in ascending order (you can configure it via [options](#configure-strainer)). Adding a dash prefix (`-`) before the property name switches the sorting way to descending. You can control this behaviour with [custom sorting way formatter](#custom-sorting-way-formatter).

### Filters

`Filters` is a comma-delimited list of `{Name}{Operator}{Value}` where

* `{Name}` is the name of a property with the `StrainerProperty` attribute or the name of a custom filter method;
  * You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, e.g. `(LikeCount|CommentCount)>10` filters to posts where `LikeCount` or `CommentCount` is greater than `10`;
* `{Operator}` is one of the [Operators](#filter-operators);
* `{Value}` is the value to use for filtering
    * You can also have multiple values (for OR logic) by using a pipe delimiter, e.g. `Title@=new|hot` filters to posts with titles that contain the phrase _"new"_ or _"hot"_.

### Page

`Page` is the number of page to return.

### PageSize

`PageSize` is the number of elements returned per page.

#### Notes

* You can use backslashes (`\`) to escape commas and pipes (`|`) within value fields to enable conditional filtering;
* You can have spaces anywhere except **within** `{Name}` or `{Operator}` fields;
* If you need to look at the data before applying pagination (e.g. get total count), use the optional parameters on `Apply` to defer pagination or explicitly call `ApplyPagination()` after manually counting resulted collection (an [example](https://github.com/Biarity/Sieve/issues/34));
* Here's a good example on how to work with [enumerables](https://github.com/Biarity/Sieve/issues/2);
* Another example on [how to do OR logic using pipes (`|`)](https://github.com/Biarity/Sieve/issues/8).

## Creating your own model

You can replace default `StrainerModel` with your own  by implementing [`IStrainerModel`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/IStrainerModel.cs) interface. See [`StrainerModel`](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/StrainerModel.cs) for reference.

## Validation

`StrainerModel` comes with **no initial validation**, so in order to add your own validation rules you should [implement your own model](#creating-your-own-model) or implement a class deriving from `StrainerModel` and then override desired properties. For example:

```cs
public class ValidatedStrainerModel : StrainerModel
{
    [Range(1, 50)]
    public override int? PageSize { get; set; }
}
```

## Nested objects

Strainer supports filtering and sorting on nested objects' properties. Mark the property using the [Fluent API](#fluent-api). Marking via attributes is not currently supported.

For example, using `Post` and `User` models:

```cs
public class Post {
    public User Author { get; set; }
}

public class User {
    public string Name { get; set; }
}
```

In order to filter by post author name, override `Load()` in your custom Strainer Module and provide expression leading to nested property:

```cs
public override void Load(IStrainerModuleBuilder builder)
{
    builder.AddProperty<Post>(p => p.Author.Name)
        .IsFilterable();
}
```

With such configuration, requests with `Filters` set to `Author.Name==John_Doe` will tell Strainer to filter to posts with post author name being exactly _"John Doe"_.

Notice how nested property name is not just `Name` but it's constructed using full property path resulting in `Author.Name` (unless explicitly configured).

## Custom methods

In order to add custom sort or filter methods, override appropriate mapping method in your .

#### Custom filter methods

```cs
public override void Load(IStrainerModuleBuilder builder)
{
    builder.AddCustomFilterMethod<Post>(nameof(IsPopular))
        .HasFunction(IsPopular);
}

private IQueryable<Post> IsPopular(IQueryable<Post> source, string filterOperator)
    => source.Where(p => p.LikeCount < 10);
```

#### Custom sort methods

```cs
public override void Load(IStrainerModuleBuilder builder)
{
    builder.AddCustomSortMethod<Post>(nameof(Popularity))
        .HasFunction(Popularity);
}

private IOrderedQueryable<Post> Popularity(IQueryable<Post> source, bool isDescending, bool isSubsequent)
{
    return isSubsequent
        ? (source as IOrderedQueryable<Post>).ThenByDescending(p => p.LikeCount)
        : source.OrderByDescending(p => p.LikeCount)
            .ThenByDescending(p => p.DateCreated);
}
```

Notice how conditional ordering is being performed depending on whether context's `IsSubsequent` property is `true`. That's because Strainer supports subsequent sorting (by multiple properties) with no exception for custom sorting. You can chain them all together.

## Filter operators

Strainer comes with following filter operators:

| Operator   | Meaning                                      |
|------------|----------------------------------------------|
| `==`       | Equals                                       |
| `!=`       | Does not equal                               |
| `>`        | Greater than                                 |
| `<`        | Less than                                    |
| `>=`       | Greater than or equal to                     |
| `<=`       | Less than or equal to                        |
| `@=`       | Contains                                     |
| `_=`       | Starts with                                  |
| `=_`       | Ends with                                    |
| `!@=`      | Does not contain                             |
| `!_=`      | Does not start with                          |
| `!=_`      | Does not end with                            |
| `@=*`      | Contains _(case-insensitive)_                |
| `_=*`      | Starts with _(case-insensitive)_             |
| `=_*`      | Ends with _(case-insensitive)_               |
| `==*`      | Equals _(case-insensitive)_                  |
| `!=*`      | Does not equal _(case-insensitive)_          |
| `!@=*`     | Does not contain _(case-insensitive)_        |
| `!_=*`     | Does not start with _(case-insensitive)_     |
| `!=_*`     | Does not end with _(case-insensitive)_       |

Case-insensitive operators will force case insensitivity when comparing values even when [`IsCaseInsensitiveForValues`](#configure-strainer) option is set to `false`.

**Note:** even though Strainer supports different case sensitivity modes, whether the case sensitivity will be taken into account when comparing values, depends entirely on the source `IQueryable` provider, which in most scenarios is the database provider.

## Custom filter operators

Same manner as marking properties you can add new filter operators:

```cs
public override void Load(IStrainerModuleBuilder builder)
{
    builder.AddFilterOperator(symbol: "%")
        .HasName("modulo equal zero")
        .HasExpression((context) => Expression.Equal(
            Expression.Modulo(context.PropertyValue, context.FilterValue),
            Expression.Constant(0)));
}
```

## Sorting way formatting

In order to determine sorting way Strainer uses `ISortingWayFormatter` service with its default implementation `DescendingPrefixSortingWayFormatter`. It checks against the presence of a prefix indicating a descending sorting way, specifically a dash `-`. For example:

 - `Name` will be translated to sorting in ascending order.
 - `-Name` will be translated to sorting in descending order.

In order to perform your own sorting way determination and formatting, implement `ISortingWayFormatter` interface (see [DescendingPrefixSortingWayFormatter](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Services/Sorting/DescendingPrefixSortingWayFormatter.cs) for reference).

Then, add your custom formatter in `Startup` **after** adding Strainer:

```
services.AddStrainer();
services.AddScoped<ISortingWayFormatter, CustomSortingWayFormatter>();
```

## Strainer's exceptions

Strainer will silently fail unless [`ThrowExceptions`](#configure-strainer) in the configuration is set to `true`. The following kinds of custom exceptions can be thrown:

* `StrainerMethodNotFoundException` with a `MethodName`
* `StrainerException` which encapsulates any other exception types in its `InnerException`

It is recommended that you write exception-handling middleware to globally handle Strainer's exceptions when using it with ASP.NET Core.

## Examples

You can find an example project incorporating most Strainer concepts in [Strainer.ExampleWebApi](https://gitlab.com/fluorite/strainer/tree/master/src/Strainer.ExampleWebApi).

## Migrating from Sieve to Strainer

A lot happened between Sieve v2* and Strainer v3*. Read the full migration guide [here](https://gitlab.com/fluorite/strainer/blob/master/docs/migration-guide-from-sieve-to-strainer.md).

## License & Contributing

Strainer is licensed under Apache 2.0. Any contributions highly appreciated!

### Acknowledgements

Top README graphic with strainer is based on one made by [Freepik](https://www.freepik.com/) from [www.flaticon.com](https://www.flaticon.com/) and [Visual Studio Icons](https://www.microsoft.com/en-us/download/details.aspx?id=35825).
