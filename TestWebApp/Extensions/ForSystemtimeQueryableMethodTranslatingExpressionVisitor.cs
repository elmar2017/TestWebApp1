using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace TestWebApp.Extensions
{
    internal class ForSystemtimeQueryableMethodTranslatingExpressionVisitor : RelationalQueryableMethodTranslatingExpressionVisitor
    {
        private QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
        private RelationalQueryableMethodTranslatingExpressionVisitorDependencies _relationalDependencies;

        private ParameterExpression _firstDateParameter;

        public ForSystemtimeQueryableMethodTranslatingExpressionVisitor([NotNull] ForSystemtimeQueryableMethodTranslatingExpressionVisitor parentVisitor) : base(parentVisitor)
        {
            _firstDateParameter = parentVisitor._firstDateParameter;
        }

        public ForSystemtimeQueryableMethodTranslatingExpressionVisitor(QueryableMethodTranslatingExpressionVisitorDependencies dependencies, RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies, QueryCompilationContext queryCompilationContext,
            ParameterExpression asOfDateParameter = null) : base(dependencies, relationalDependencies, queryCompilationContext )
        {
            _dependencies = dependencies;
            _relationalDependencies = relationalDependencies;
            _firstDateParameter = asOfDateParameter;
        }

        protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor()
            => new ForSystemtimeQueryableMethodTranslatingExpressionVisitor(this);

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.DeclaringType == typeof(SqlServerForSystemtimeQueryableExtensions))
            {
                var result = Visit(methodCallExpression.Arguments[0]);
                var shapedExpression = result as ShapedQueryExpression;
                if (shapedExpression != null)
                {
                    switch (methodCallExpression.Method.Name)
                    {
                        case nameof(SqlServerForSystemtimeQueryableExtensions.ForSystemtimeAll):
                            shapedExpression.TrySetIsAll();
                            return result;
                        case nameof(SqlServerForSystemtimeQueryableExtensions.AsOf):
                            // capture the date parameter for use in 'FOR SYSTEM_TIME AS OF '
                            var parameter = Visit(methodCallExpression.Arguments[1]) as ParameterExpression;
                            shapedExpression.TrySetAsOfDateParameter(parameter);
                            return result;
                        case nameof(SqlServerForSystemtimeQueryableExtensions.Between):
                            // capture the both date parameters for use in 'FOR SYSTEM_TIME BETWEEN '
                            var param1 = Visit(methodCallExpression.Arguments[1]) as ParameterExpression;
                            var param2 = Visit(methodCallExpression.Arguments[2]) as ParameterExpression;
                            shapedExpression.TrySetBetweenDateParameters(param1, param2);
                            return result;
                    }
                }
            }

            return base.VisitMethodCall(methodCallExpression);
        }
    }
}
