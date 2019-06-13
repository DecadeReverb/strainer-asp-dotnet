using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterExpressionBuilder<TOperator> : IFilterExpressionBuilder<TOperator>
        where TOperator : IFilterOperator
    {
        public FilterExpressionBuilder(IFilterExpressionMapper mapper)
        {
            Mapper = mapper;
        }

        public Func<IFilterExpressionContext, Expression> Expression { get; protected set; }

        protected IFilterExpressionMapper Mapper { get; }

        public IFilterExpressionBuilder<TOperator> HasExpression(Func<IFilterExpressionContext, Expression> expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            UpdateMap();

            return this;
        }

        private void UpdateMap()
        {
            Mapper.Add(typeof(TOperator), Expression);
        }
    }
}
