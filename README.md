# Strainer

Strainer is a simple, clean, and extensible framework for .NET Core that **adds sorting, filtering, and pagination functionality out of the box**. 
Most common use case would be for serving ASP.NET Core GET queries.

**Note:** This project is a fork from repository named [Sieve](https://github.com/Biarity/Sieve) with its author [Biarity](https://github.com/Biarity).

## Usage for ASP.NET Core

In this example, consider an app with a `Post` entity. 
We'll use Strainer to add sorting, filtering, and pagination capabilities when GET-ing all available posts.

### 1. Add required services

Inject the `StrainerProcessor` service. So in `Startup.cs` add:

```cs
services.AddStrainer<StrainerProcessor>();
```

### 2. Tell Strainer which properties you'd like to sort/filter in your models

Strainer will only sort/filter properties that have the attribute `[Strainer(CanSort = true, CanFilter = true)]` on them (they don't have to be both true).
So for our `Post` entity model example:

```cs
public int Id { get; set; }

[Strainer(CanFilter = true, CanSort = true)]
public string Title { get; set; }

[Strainer(CanFilter = true, CanSort = true)]
public int LikeCount { get; set; }

[Strainer(CanFilter = true, CanSort = true)]
public int CommentCount { get; set; }

[Strainer(CanFilter = true, CanSort = true, Name = "created")]
public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;
```

There is also the `Name` parameter that you can use to have a different name for use by clients.

Alternatively, you can use [Fluent API](#fluent-api) to do the same. This is especially useful if you don't want to use attributes or have multiple APIs. 

### 3. Get sort/filter/page queries by using the Strainer model in your controllers

In the action that handles returning Posts, use `StrainerModel` to get the sort/filter/page query. 
Apply it to your data by injecting `StrainerProcessor` into the controller and using its `Apply<TEntity>` method. So for instance:

```cs
[HttpGet]
public JsonResult GetPosts(StrainerModel strainerModel) 
{
    var result = _dbContext.Posts.AsNoTracking(); // Makes read-only queries faster
    result = _strainerProcessor.Apply(strainerModel, result); // Returns `result` after applying the sort/filter/page query in `StrainerModel` to it
    return Json(result.ToList());
}
```

You can also explicitly specify if only filtering, sorting, and/or pagination should be applied via optional arguments.

### 4. Send a request

[Send a request](#send-a-request)

### Add custom sort/filter methods

If you want to add custom sort/filter methods, add custom implementation holding sort/filter methods that Strainer will search through.

For instance:

```cs
services.AddStrainer<ApplicationStrainerProcessor>()
    .AddCustomFilterMethods<StrainerCustomFilterMethods>()
    .AddCustomSortMethods<StrainerCustomSortMethods>();
```

Where `StrainerCustomSortMethodsOfPosts` for example is:

```cs
public class StrainerCustomSortMethods : IStrainerCustomSortMethods
{
    public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy, bool desc) // The method is given an indicator of weather to use ThenBy(), and if the query is descending 
    {
        var result = useThenBy ?
            ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount) : // ThenBy only works on IOrderedQueryable<TEntity>
            source.OrderBy(p => p.LikeCount)
            .ThenBy(p => p.CommentCount)
            .ThenBy(p => p.DateCreated);

        return result; // Must return modified IQueryable<TEntity>
    }
}
```

And `StrainerCustomFilterMethods`:

```cs
public class StrainerCustomFilterMethods : IStrainerCustomFilterMethods
{
    public IQueryable<Post> IsNew(IQueryable<Post> source, string op, string[] values) // The method is given the {Operator} & {Value}
    {
        var result = source.Where(p => p.LikeCount < 100 &&
                                        p.CommentCount < 5);

        return result; // Must return modified IQueryable<TEntity>
    }
}
```

## Configure Strainer

Use the [ASP.NET Core options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) with `StrainerOptions` to tell Strainer where to look for configuration. For example:

```cs
services.Configure<StrainerOptions>(Configuration.GetSection("Strainer"));
```

Then you can add the configuration:

```json
{
    "Strainer": {
        "CaseSensitive": "boolean: should property names be case-sensitive? Defaults to false",
        "DefaultPageSize": "int number: optional number to fallback to when no page argument is given. Set <=0 to disable paging if no pageSize is specified (default).",
        "MaxPageSize": "int number: maximum allowed page size. Set <=0 to make infinite (default)",
        "ThrowExceptions": "boolean: should Strainer throw exceptions instead of silently failing? Defaults to false"
    }
}
```

## Send a request

With all the above in place, you can now send a GET request that includes a sort/filter/page query. An example:

```curl
GET /GetPosts

?sorts=     LikeCount,CommentCount,-created         // sort by likes, then comments, then descendingly by date created 
&filters=   LikeCount>10, Title@=awesome title,     // filter to posts with more than 10 likes, and a title that contains the phrase "awesome title"
&page=      1                                       // get the first page...
&pageSize=  10                                      // ...which contains 10 posts

```
More formally:
* `sorts` is a comma-delimited ordered list of property names to sort by. Adding a `-` before the name switches to sorting descendingly.
* `filters` is a comma-delimited list of `{Name}{Operator}{Value}` where
    * `{Name}` is the name of a property with the Strainer attribute or the name of a custom filter method for TEntity
        * You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if `LikeCount` or `CommentCount` is `>10`
    * `{Operator}` is one of the [Operators](#operators)
    * `{Value}` is the value to use for filtering
        * You can also have multiple values (for OR logic) by using a pipe delimiter, eg. `Title@=new|hot` will return posts with titles that contain the text "`new`" or "`hot`"
* `page` is the number of page to return
* `pageSize` is the number of items returned per page 

Notes:
* You can use backslashes to escape commas and pipes within value fields
* You can have spaces anywhere except *within* `{Name}` or `{Operator}` fields
* If you need to look at the data before applying pagination (eg. get total count), use the optional paramters on `Apply` to defer pagination (an [example](https://github.com/Biarity/Sieve/issues/34))
* Here's a [good example on how to work with enumerables](https://github.com/Biarity/Sieve/issues/2)
* Another example on [how to do OR logic](https://github.com/Biarity/Sieve/issues/8)

### Nested objects

You can filter/sort on a nested object's property by marking the property using the Fluent API. Marking via attributes not currently supported.

For example, using this object model:

```cs
public class Post {
    public User Creator { get; set; }
}

public class User {
    public string Name { get; set; }
}
```

Mark `Post.User` to be filterable:
```cs
// in MapProperties
mapper.Property<Post>(p => p.Creator.Name)
    .CanFilter();
```

Now you can make requests such as: `filters=User.Name==specific_name`.

### Creating your own DSL

You can replace this DSL with your own (eg. use JSON instead) by implementing an [IStrainerModel](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/IStrainerModel.cs). You can use the default [StrainerModel](https://gitlab.com/fluorite/strainer/blob/master/src/Strainer/Models/StrainerModel.cs) for reference.

### Operators

| Operator   | Meaning                  |
|------------|--------------------------|
| `==`       | Equals                   |
| `!=`       | Not equals               |
| `>`        | Greater than             |
| `<`        | Less than                |
| `>=`       | Greater than or equal to |
| `<=`       | Less than or equal to    |
| `@=`       | Contains                 |
| `_=`       | Starts with              |
| `!@=`      | Does not Contains        |
| `!_=`      | Does not Starts with     |
| `@=*`      | Case-insensitive string Contains |
| `_=*`      | Case-insensitive string Starts with |
| `==*`      | Case-insensitive string Equals |
| `!@=*`     | Case-insensitive string does not Contains |
| `!_=*`     | Case-insensitive string does not Starts with |

### Handle Strainer's exceptions

Strainer will silently fail unless `ThrowExceptions` in the configuration is set to true. 3 kinds of custom exceptions can be thrown:

* `StrainerMethodNotFoundException` with a `MethodName`
* `StrainerIncompatibleMethodException` with a `MethodName`, an `ExpectedType` and an `ActualType`
* `StrainerException` which encapsulates any other exception types in its `InnerException`

It is recommended that you write exception-handling middleware to globally handle Strainer's exceptions when using it with ASP.NET Core.

### Example project

You can find an example project incorporating most Strainer concepts in [Strainer.ExampleWebApi](https://gitlab.com/fluorite/strainer/tree/master/src/Strainer.ExampleWebApi).

## Fluent API

To use the Fluent API instead of attributes in marking properties, setup an alternative `StrainerProcessor` that overrides `MapProperties`. For example:

```cs
public class ApplicationStrainerProcessor : StrainerProcessor
{
    public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
    {

    }

    protected override IStrainerPropertyMapper MapProperties(IStrainerPropertyMapper mapper)
    {
        mapper.Property<Post>(p => p.Title)
            .CanFilter()
            .HasName("a_different_query_name_here");

        mapper.Property<Post>(p => p.CommentCount)
            .CanSort();

        mapper.Property<Post>(p => p.DateCreated)
            .CanSort()
            .CanFilter()
            .HasName("created_on");

        return mapper;
    }
}
```

Now you should inject custom `StrainerProcessor`:

```cs
services.AddStrainer<ApplicationStrainerProcessor>();
```

## Migrating from Sieve to Strainer v3.0.0

A lot happened between Sieve v2* and Strainer v3*. Read the full migration guide [here](https://gitlab.com/fluorite/strainer/blob/master/docs/migration-guide-from-sieve-to-strainer.md).

## License & Contributing

Strainer is licensed under Apache 2.0. Any contributions highly appreciated!
