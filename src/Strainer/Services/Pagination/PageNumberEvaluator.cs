using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pagination
{
    public class PageNumberEvaluator : IPageNumberEvaluator
    {
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;

        public PageNumberEvaluator(IStrainerOptionsProvider strainerOptionsProvider)
        {
            _strainerOptionsProvider = strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
        }

        public int Evaluate(IStrainerModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var options = _strainerOptionsProvider.GetStrainerOptions();

            return model.Page ?? options.DefaultPageNumber;
        }
    }
}
