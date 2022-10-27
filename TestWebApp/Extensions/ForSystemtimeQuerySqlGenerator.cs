using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace TestWebApp.Extensions
{
    internal class ForSystemtimeQuerySqlGenerator : SqlServerQuerySqlGenerator
    {
        const string FORSYSTEMTIME_PARAMETER_PREFIX = "__for_system_time_";
        private ISqlGenerationHelper _sqlGenerationHelper;
        private IRelationalCommandBuilder _commandbuilder;

        public ForSystemtimeQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies)
            : base(new QuerySqlGeneratorDependencies(new RelationalCommandBuilderFactoryDecorator(dependencies.RelationalCommandBuilderFactory), dependencies.SqlGenerationHelper)
                  )
        {
            _sqlGenerationHelper = dependencies.SqlGenerationHelper;
            _commandbuilder = this.Dependencies.RelationalCommandBuilderFactory.Create();
        }

        protected override Expression VisitExtension(Expression extensionExpression)
        {
            switch (extensionExpression)
            {
                case ForSystemtimeTableExpression expression:
                    return VisitForSystemtimeTable(expression);
            }

            return base.VisitExtension(extensionExpression);
        }

        private Expression VisitForSystemtimeTable(ForSystemtimeTableExpression expression)
        {
            _commandbuilder.Append(_sqlGenerationHelper.DelimitIdentifier(expression.Name, expression.Schema));

            if (expression.IsAll)
            {
                _commandbuilder.Append(" FOR SYSTEM_TIME ALL ");
            }
            else if (expression.FromDate != null)
            {
                var fromDateParamName = $"{FORSYSTEMTIME_PARAMETER_PREFIX}{expression.FromDate.Name}";
                Sql.Append($" FOR SYSTEM_TIME {(expression.ToDate == null ? "AS OF" : "BETWEEN")} @{fromDateParamName}");

                if (!_commandbuilder.Parameters.Any(x => x.InvariantName == expression.FromDate.Name))
                    _commandbuilder.AddParameter(expression.FromDate.Name, fromDateParamName);

                if (expression.ToDate != null)
                {
                    var toDateParamName = $"{FORSYSTEMTIME_PARAMETER_PREFIX}{expression.ToDate.Name}";
                    Sql.Append($" AND @{toDateParamName}");

                    if (!_commandbuilder.Parameters.Any(x => x.InvariantName == expression.ToDate.Name))
                        _commandbuilder.AddParameter(expression.ToDate.Name, toDateParamName);
                }
            }

            _commandbuilder
                .Append(AliasSeparator)
                .Append(_sqlGenerationHelper.DelimitIdentifier(expression.Alias));

            return expression;
        }
    }
}
