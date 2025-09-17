using System.Linq.Expressions;


namespace SharedKernel
{
    public class DatabaseProviderConfiguration
    {
        public static SearchProviderFilterExpressionBuilder DefaultSearchProviderFilterExpressionBuilderFunc { private get; set; }
        private static AsyncLocal<SearchProviderFilterExpressionBuilder> searchProviderFilterExpressionBuilderFunc = new AsyncLocal<SearchProviderFilterExpressionBuilder>();

        public delegate Expression SearchProviderFilterExpressionBuilder(Type TEntity, Expression Parameter, FilterByOptions FilterByOptions);
        public static SearchProviderFilterExpressionBuilder SearchProviderFilterExpressionBuilderFunc
        {
            get
            {
                if (searchProviderFilterExpressionBuilderFunc == null || searchProviderFilterExpressionBuilderFunc.Value == null)
                {
                    return DefaultSearchProviderFilterExpressionBuilderFunc ?? SqlSearchProvider.BuildFilterExpression;
                }

                return searchProviderFilterExpressionBuilderFunc.Value;
            }
            set
            {
                if (value == null)
                {
                    searchProviderFilterExpressionBuilderFunc.Value = DefaultSearchProviderFilterExpressionBuilderFunc ?? SqlSearchProvider.BuildFilterExpression;
                }
                else
                {
                    searchProviderFilterExpressionBuilderFunc.Value = value;
                }
            }
        }
    }
}
