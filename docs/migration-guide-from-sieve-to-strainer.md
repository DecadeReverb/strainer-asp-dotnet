# Migration guide from Sieve to Strainer

To reach version 3.0.0 Strainer met heavy refactoring. Whole bunch of new interfaces and services was created, a lot of code was split into smaller pieces in order to achieve finer granularity. No worries - all this without a change in framework's core functionality. The main goal of this process was to make the framework easier to maintain, test and extend.

### New name Sieve => Strainer

As this project was forked, to avoid ambiguity a new name was established - **"Strainer"**.

Replace your `Sieve` models and services with `Strainer` (run find and replace with `Ctrl` + `H` in VS):

```cs
public class PostsController : Controller
{
    private readonly ISieveProcessor _sieveProcessor;
    private readonly ApplicationDbContext _dbContext;

    public PostsController(ISieveProcessor sieveProcessor,
        ApplicationDbContext dbContext)
    {
        _sieveProcessor = sieveProcessor;
        _dbContext = dbContext;
    }

    [HttpGet]
    public JsonResult GetAll(SieveModel sieveModel)
    {
        var result = _dbContext.Posts.AsNoTracking();
        result = _sieveProcessor.Apply(sieveModel, result);

        return Json(result.ToList());
    }
```

change to:

```cs
public class PostsController : Controller
{
    private readonly IStrainerProcessor _strainerProcessor;
    private readonly ApplicationDbContext _dbContext;

    public PostsController(IStrainerProcessor strainerProcessor,
        ApplicationDbContext dbContext)
    {
        _strainerProcessor = strainerProcessor;
        _dbContext = dbContext;
    }

    [HttpGet]
    public JsonResult GetAll(StrainerModel strainerModel)
    {
        var result = _dbContext.Posts.AsNoTracking();
        result = _strainerProcessor.Apply(strainerModel, result);

        return Json(result.ToList());
    }
```

### New namespace

Project rebranding came with new namespace. Replace your `Sieve` usings:

```cs
using Sieve.Services;
```

with `Strainer`:

```cs
using Strainer.Services;
```

### New and simpler SieveProcessor - StrainerProcessor

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

With Strainer an `IStrainerContext` was introduced to wrap all important services. Eventually, all you need to do is call the base constructor while passing the context:

```cs
public class ApplicationStrainerProcessor : StrainerProcessor
{
    public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
    {

    }
}
```

### ...more will come.
