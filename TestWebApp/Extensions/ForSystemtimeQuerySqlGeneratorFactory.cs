using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Query;

namespace TestWebApp.Extensions
{
    public class ForSystemtimeQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;

        public ForSystemtimeQuerySqlGeneratorFactory([NotNull] QuerySqlGeneratorDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public QuerySqlGenerator Create()
        {
            return new ForSystemtimeQuerySqlGenerator(_dependencies);
        }
    }
}
