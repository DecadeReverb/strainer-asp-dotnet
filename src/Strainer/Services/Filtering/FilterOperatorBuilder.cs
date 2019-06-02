//using System;
//using Fluorite.Strainer.Models;

//namespace Fluorite.Strainer.Services.Filtering
//{
//    public class FilterOperatorBuilder : IFilterOperatorBuilder
//    {
//        private readonly FilterOperator _filterOperator;

//        public FilterOperatorBuilder()
//        {
//            _filterOperator = new FilterOperator();
//        }

//        public IFilterOperator Build() => _filterOperator;

//        public IFilterOperatorBuilder Default()
//        {
//            _filterOperator.IsDefault = true;

//            return this;
//        }

//        public IFilterOperatorBuilder HasName(string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//            {
//                throw new ArgumentException(
//                    $"{nameof(name)} cannot be null, empty " +
//                    $"or contain only whitespace characaters.",
//                    nameof(name));
//            }

//            _filterOperator.Name = name;

//            return this;
//        }

//        public IFilterOperatorBuilder Operator(string @operator)
//        {
//            if (string.IsNullOrWhiteSpace(@operator))
//            {
//                throw new ArgumentException(
//                    $"{nameof(@operator)} cannot be null, empty " +
//                    $"or contain only whitespace characaters.",
//                    nameof(@operator));
//            }

//            _filterOperator.Operator = @operator;

//            return this;
//        }
//    }
//}
