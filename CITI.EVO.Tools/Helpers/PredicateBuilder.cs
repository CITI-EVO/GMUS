using System;
using System.Linq.Expressions;

namespace CITI.EVO.Tools.Helpers
{
    public static class PredicateBuilder
    {
        public static Expression<Func<TEntity, bool>> True<TEntity>()
        {
            return f => true;
        }

        public static Expression<Func<TEntity, bool>> False<TEntity>()
        {
            return f => false;
        }

        public static Expression<Func<TEntity, bool>> Or<TEntity>(this Expression<Func<TEntity, bool>> source, Expression<Func<TEntity, bool>> target)
        {
            var invokedExpr = Expression.Invoke(target, source.Parameters);
            var orElseExp = Expression.OrElse(source.Body, invokedExpr);

            var resultExp = Expression.Lambda<Func<TEntity, bool>>(orElseExp, source.Parameters);
            return resultExp;
        }

        public static Expression<Func<TEntity, bool>> And<TEntity>(this Expression<Func<TEntity, bool>> source, Expression<Func<TEntity, bool>> target)
        {
            var invokedExp = Expression.Invoke(target, source.Parameters);
            var orElseExp = Expression.AndAlso(source.Body, invokedExp);

            var resultExp = Expression.Lambda<Func<TEntity, bool>>(orElseExp, source.Parameters);
            return resultExp;
        }
    }
}
