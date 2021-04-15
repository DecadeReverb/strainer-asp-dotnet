namespace Fluorite.Strainer.Services.Modules
{
    /// <summary>
    /// Stores Strainer configuration in an encapsulated form of a module.
    /// </summary>
    /// <typeparam name="T">
    /// The type of model for which this module is for.
    /// </typeparam>
    public interface IStrainerModule<T> : IStrainerModule
    {
        /// <summary>
        /// Loads configuration in module.
        /// </summary>
        /// <param name="builder">
        /// The Strainer Module builder.
        /// </param>
        void Load(IStrainerModuleBuilder<T> builder);
    }
}
