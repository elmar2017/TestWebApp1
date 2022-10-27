using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace TestWebApp.Extensions
{
    public static class SqlServerForSystemtimeQueryableExtensions
    {
        public static readonly MethodInfo AsOfMethodInfo
          = typeof(SqlServerForSystemtimeQueryableExtensions).GetTypeInfo().GetDeclaredMethod(nameof(AsOf));

        public static readonly MethodInfo BetweenMethodInfo
          = typeof(SqlServerForSystemtimeQueryableExtensions).GetTypeInfo().GetDeclaredMethod(nameof(Between));

        public static readonly MethodInfo ForSystemtimeAllMethodInfo
          = typeof(SqlServerForSystemtimeQueryableExtensions).GetTypeInfo().GetDeclaredMethod(nameof(ForSystemtimeAll));


        /// <summary>
        /// Configure a query to constrain all temporal tables to a specific time
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> AsOf<TEntity>(this IQueryable<TEntity> source, DateTime date) where TEntity : class
        {
            return
              source.Provider is EntityQueryProvider
                ? source.Provider.CreateQuery<TEntity>(
                  Expression.Call(
                    instance: null,
                    method: AsOfMethodInfo.MakeGenericMethod(typeof(TEntity)),
                    arg0: source.Expression,
                    arg1: Expression.Constant(date)))
                : source;
        }

        public static IQueryable<TEntity> Between<TEntity>(this IQueryable<TEntity> source, DateTime firstDate, DateTime secondDate) where TEntity : class
        {
            return
              source.Provider is EntityQueryProvider
                ? source.Provider.CreateQuery<TEntity>(
                  Expression.Call(
                    instance: null,
                    method: BetweenMethodInfo.MakeGenericMethod(typeof(TEntity)),
                    arg0: source.Expression,
                    arg1: Expression.Constant(firstDate),
                    arg2: Expression.Constant(secondDate)))
                : source;
        }

        public static IQueryable<TEntity> ForSystemtimeAll<TEntity>(this IQueryable<TEntity> source) where TEntity : class
        {
            return
              source.Provider is EntityQueryProvider
                ? source.Provider.CreateQuery<TEntity>(
                  Expression.Call(
                    instance: null,
                    method: ForSystemtimeAllMethodInfo.MakeGenericMethod(typeof(TEntity)),
                    source.Expression
                    ))
                : source;
        }
    }
}
