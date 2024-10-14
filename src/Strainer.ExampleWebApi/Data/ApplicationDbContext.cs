using Fluorite.Strainer.ExampleWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluorite.Strainer.ExampleWebApi.Data;

/// <summary>
/// Represents a session with the database and can be used to query
/// and save instances of entities.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/>
    /// class using the specified options.
    /// </summary>
    /// <param name="options">
    /// The options for this context.
    /// </param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    /// <summary>
    /// Gets or sets <see cref="DbSet{TEntity}"/> of <see cref="Comment"/>s.
    /// </summary>
    public DbSet<Comment> Comments { get; set; }

    /// <summary>
    /// Gets or sets <see cref="DbSet{TEntity}"/> of <see cref="Post"/>s.
    /// </summary>
    public DbSet<Post> Posts { get; set; }

    /// <summary>
    /// Configures the model that was discovered by convention from
    /// the entity types exposed in <see cref="DbSet{TEntity}"/> properties
    /// on derived context.
    /// </summary>
    /// <param name="builder">
    /// The builder being used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Post>().ToTable(nameof(Post));

        builder.Entity<Comment>().ToTable(nameof(Comment));
    }
}
