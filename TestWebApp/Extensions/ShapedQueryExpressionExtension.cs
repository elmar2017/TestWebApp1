using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace TestWebApp.Extensions
{
    public static class ShapedQueryExpressionExtension
    {
        public static bool TrySetIsAll(this ShapedQueryExpression shapedQueryExpression)
        {
            if (shapedQueryExpression.TryGetTable(out var table))
            {
                table.IsAll = true;
                return true;
            }

            return false;
        }

        public static bool TrySetAsOfDateParameter(this ShapedQueryExpression shapedQueryExpression, ParameterExpression dateParameter)
        {
            if (shapedQueryExpression.TryGetTable(out var table))
            {
                table.FromDate = dateParameter;
                return true;
            }

            return false;
        }

        public static bool TrySetBetweenDateParameters(this ShapedQueryExpression shapedQueryExpression, ParameterExpression fromDateParameter, ParameterExpression toDateParameter)
        {
            if (shapedQueryExpression.TryGetTable(out var table))
            {
                table.FromDate = fromDateParameter;
                table.ToDate = toDateParameter;
                return true;
            }

            return false;
        }

        private static bool TryGetTable(this ShapedQueryExpression shapedQueryExpression, out ForSystemtimeTableExpression table)
        {
            table = null;
            if (shapedQueryExpression.QueryExpression is SelectExpression selectExpression)
            {
                table = selectExpression.Tables.OfType<ForSystemtimeTableExpression>().FirstOrDefault();
                return table != null;
            }
            return false;
        }
    }
}
