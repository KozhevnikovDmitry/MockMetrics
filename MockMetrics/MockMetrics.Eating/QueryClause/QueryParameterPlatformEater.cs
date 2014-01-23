using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public interface IQueryParameterPlatformEater : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IQueryParameterPlatform queryParameterPlatform);
    }

    public class QueryParameterPlatformEater : IQueryParameterPlatformEater
    {
        private readonly IEater _eater;

        public QueryParameterPlatformEater(IEater eater)
        {
            _eater = eater;
        }

        public Variable Eat(ISnapshot snapshot, IQueryParameterPlatform queryParameterPlatform)
        {
            return _eater.Eat(snapshot, queryParameterPlatform.Value);
        }
    }
}