# Migration guide from Sieve to Strainer version 3.0.0

To reach version 3.0.0 SieveRevised met heavy refactoring. Whole bunch of new interfaces and services was created, a lot of code was split into smaller pieces in order to achieve finer granularity. No worries - all this without a change in framework's core functionality. The main goal of refactoring was to create a more friendly, universal and easier to maintain, test and extend framework.

## Sieve Processor - introducing Sieve Context

In older version of Sieve in order to create a custom `SieveProcessor` you would have to pass couple of services:

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
```

Now, with `ISieveContext` wrapping all of that, all you need to do is call base constructor passing the context:

```cs
public class ApplicationSieveProcessor : SieveProcessor
{
    public ApplicationSieveProcessor(ISieveContext context) : base(context)
    {

    }
}
```


