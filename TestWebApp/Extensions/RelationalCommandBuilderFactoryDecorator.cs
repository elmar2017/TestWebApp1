using Microsoft.EntityFrameworkCore.Storage;

namespace TestWebApp.Extensions
{
    public class RelationalCommandBuilderFactoryDecorator : IRelationalCommandBuilderFactory
    {
        private Lazy<IRelationalCommandBuilder> _builder;

        public RelationalCommandBuilderFactoryDecorator(IRelationalCommandBuilderFactory innerFactory)
        {
            _builder = new Lazy<IRelationalCommandBuilder>(() => innerFactory.Create());
        }

        public IRelationalCommandBuilder Create()
        {
            return _builder.Value;
        }
    }
}
