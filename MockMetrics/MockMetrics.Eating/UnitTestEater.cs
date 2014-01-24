using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;


namespace MockMetrics.Eating
{
    public class UnitTestEater
    {
        private readonly IEater _eater;

        public UnitTestEater(IEater eater)
        {
            _eater = eater;
        }

        public ISnapshot EatUnitTest(IMethodDeclaration unitTest)
        {
            Console.WriteLine(unitTest.NameIdentifier.Name);

            var snapshot = new Snapshot(unitTest);

            if (unitTest.IsValid())
            {
                foreach (var parameterDeclaration in unitTest.ParameterDeclarations)
                {
                    _eater.Eat(snapshot, parameterDeclaration);
                }

                _eater.EatedNodes.Clear();
                _eater.Eat(snapshot, unitTest.Body);
            }

            return snapshot;
        }
    }
}