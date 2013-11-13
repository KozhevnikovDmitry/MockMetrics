using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public class UnitTestEater
    {
        private readonly IEater _eater;

        public UnitTestEater(IEater eater)
        {
            _eater = eater;
        }

        public Snapshot EatUnitTest(IMethodDeclaration unitTest)
        {
            // result snapshot
            var snapshot = new Snapshot(unitTest);

            // all parameters are stubs
            foreach (var parameterDeclaration in unitTest.ParameterDeclarations)
            {
                _eater.Eat(snapshot, parameterDeclaration);
            }

            _eater.Eat(snapshot, unitTest.Body);

            new PostEater().PostEat(snapshot);

            return snapshot;
        }

        
    }
}