namespace Fluorite.Strainer.Services.Configuration;

public interface IStrainerModuleTypeValidator
{
    ICollection<Type> GetValidModuleTypes(IReadOnlyCollection<Type> types);
}