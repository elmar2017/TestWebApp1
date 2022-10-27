using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace TestWebApp.Extensions
{
    public class ForSystemtimeSqlExpressionFactory : SqlExpressionFactory
    {
        public ForSystemtimeSqlExpressionFactory(SqlExpressionFactoryDependencies dependencies) : base(dependencies)
        {
        }

        public override SelectExpression Select(IEntityType entityType)
        {
            var forSystemtimeTableExpression = new ForSystemtimeTableExpression(
                entityType.GetTableName(),
                entityType.GetSchema(),
                entityType.GetTableName().ToLower().Substring(0, 1));
            var selectContructor = typeof(SelectExpression).GetConstructor(BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new Type[] { typeof(IEntityType), typeof(TableExpressionBase) }, null);
            var select = (SelectExpression)selectContructor.Invoke(new object[] { entityType, forSystemtimeTableExpression });

            var privateInitializer = typeof(SqlExpressionFactory).GetMethod("AddConditions", BindingFlags.NonPublic | BindingFlags.Instance);
            privateInitializer.Invoke(this, new object[] { select, entityType, null });

            return select;

        }
    }
}
