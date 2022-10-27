using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace TestWebApp.Extensions
{
    public class ForSystemtimeTableExpression : TableExpressionBase
    {
        public string Name { get; }
        public string Schema { get; }

        public bool IsAll { get; set; }

        /// <summary>
        /// Parameter used to constrain a query to a specific temporal period in 'FOR SYSTEM_TIME AS OF <see cref="FromDate"/>' or as the first date in 'FOR SYSTEM_TIME BETWEEN <see cref="FromDate"/> AND <see cref="ToDate"/>'.
        /// </summary>
        public ParameterExpression FromDate { get; set; }

        /// <summary>
        /// Second parameter used to constrain a query to a specific temporal period in 'FOR SYSTEM_TIME BETWEEN <see cref="FromDate"/> AND <see cref="ToDate"/>'.
        /// </summary>
        public ParameterExpression ToDate { get; set; }

        public ForSystemtimeTableExpression(string name, string schema, string alias) : base(alias)
        {
            Name = name;
            Schema = schema;
        }

        protected override void Print(ExpressionPrinter expressionPrinter)
        {
            if (!string.IsNullOrEmpty(Schema))
            {
                expressionPrinter.Append(Schema).Append(".");
            }

            expressionPrinter.Append(Name).Append(" AS ").Append(Alias);
        }

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Name, Schema, IsAll, FromDate?.GetHashCode(), ToDate?.GetHashCode());
    }
}
