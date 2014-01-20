﻿using JetBrains.ReSharper.Psi.CSharp.Tree;
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
            var snapshot = new Snapshot(unitTest);
            
            foreach (var parameterDeclaration in unitTest.ParameterDeclarations)
            {
                _eater.Eat(snapshot, parameterDeclaration);
            }

            _eater.EatedNodes.Clear();
            _eater.Eat(snapshot, unitTest.Body);

            return snapshot;
        }
    }
}