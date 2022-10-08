using ZeroFramework.DeviceCenter.Domain.Specifications.Exceptions;

namespace ZeroFramework.DeviceCenter.Domain.Specifications.Evaluators
{
    public class OrderEvaluator : IEvaluator, IInMemoryEvaluator
    {
        private OrderEvaluator() { }

        public static OrderEvaluator Instance { get; } = new OrderEvaluator();

        public bool IsCriteriaEvaluator { get; } = false;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.OrderExpressions != null)
            {
                if (specification.OrderExpressions.Where(x => x.OrderType == OrderTypeEnum.OrderBy ||
                                                            x.OrderType == OrderTypeEnum.OrderByDescending).Count() > 1)
                {
                    throw new DuplicateOrderChainException();
                }

                IOrderedQueryable<T>? orderedQuery = null;
                foreach (var (KeySelector, OrderType) in specification.OrderExpressions)
                {
                    if (OrderType == OrderTypeEnum.OrderBy)
                    {
                        orderedQuery = query.OrderBy(KeySelector);
                    }
                    else if (OrderType == OrderTypeEnum.OrderByDescending)
                    {
                        orderedQuery = query.OrderByDescending(KeySelector);
                    }
                    else if (OrderType == OrderTypeEnum.ThenBy)
                    {
                        orderedQuery = orderedQuery?.ThenBy(KeySelector);
                    }
                    else if (OrderType == OrderTypeEnum.ThenByDescending)
                    {
                        orderedQuery = orderedQuery?.ThenByDescending(KeySelector);
                    }
                }

                if (orderedQuery != null)
                {
                    query = orderedQuery;
                }
            }

            return query;
        }

        public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
        {
            if (specification.OrderExpressions != null)
            {
                if (specification.OrderExpressions.Where(x => x.OrderType == OrderTypeEnum.OrderBy ||
                                                            x.OrderType == OrderTypeEnum.OrderByDescending).Count() > 1)
                {
                    throw new DuplicateOrderChainException();
                }

                IOrderedEnumerable<T>? orderedQuery = null;
                foreach (var (KeySelector, OrderType) in specification.OrderExpressions)
                {
                    if (OrderType == OrderTypeEnum.OrderBy)
                    {
                        orderedQuery = query.OrderBy(KeySelector.Compile());
                    }
                    else if (OrderType == OrderTypeEnum.OrderByDescending)
                    {
                        orderedQuery = query.OrderByDescending(KeySelector.Compile());
                    }
                    else if (OrderType == OrderTypeEnum.ThenBy)
                    {
                        orderedQuery = orderedQuery?.ThenBy(KeySelector.Compile());
                    }
                    else if (OrderType == OrderTypeEnum.ThenByDescending)
                    {
                        orderedQuery = orderedQuery?.ThenByDescending(KeySelector.Compile());
                    }
                }

                if (orderedQuery != null)
                {
                    query = orderedQuery;
                }
            }

            return query;
        }
    }
}