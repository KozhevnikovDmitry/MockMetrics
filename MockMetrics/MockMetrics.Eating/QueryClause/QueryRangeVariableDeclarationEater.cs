using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.QueryClause
{
    public interface IQueryRangeVariableDeclarationEater : ICSharpNodeEater
    {
        Variable Eat(ISnapshot snapshot, IQueryRangeVariableDeclaration queryRangeVariableDeclaration);
    }

    public class QueryRangeVariableDeclarationEater : IQueryRangeVariableDeclarationEater
    {
        private readonly IEater _eater;
        private readonly IMetricHelper _metricHelper;

        public QueryRangeVariableDeclarationEater([NotNull] IEater eater, [NotNull] IMetricHelper metricHelper)
        {
            if (eater == null) throw new ArgumentNullException("eater");
            if (metricHelper == null) throw new ArgumentNullException("metricHelper");
            _eater = eater;
            _metricHelper = metricHelper;
        }

        public Variable Eat(ISnapshot snapshot, [NotNull] IQueryRangeVariableDeclaration queryRangeVariableDeclaration)
        {
            if (queryRangeVariableDeclaration == null) 
                throw new ArgumentNullException("queryRangeVariableDeclaration");
            if (queryRangeVariableDeclaration.DeclaredElement == null)
                throw new ArgumentException("queryRangeVariableDeclaration.DeclaredElement is null");


            var vartype = _metricHelper.MetricsForType(snapshot, queryRangeVariableDeclaration.DeclaredElement.Type);
            snapshot.AddVariable(queryRangeVariableDeclaration, vartype);
            return vartype;
        }
    }
}