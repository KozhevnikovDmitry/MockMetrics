using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public class UnitTestEater
    {
        private readonly Eater _eater;

        public UnitTestEater(Eater eater)
        {
            _eater = eater;
        }

        public Snapshot EatUnitTest(IMethodDeclaration unitTest)
        {
            // result snapshot
            var snapshot = new Snapshot(unitTest);

            // all parameters are stubs
            foreach (ICSharpParameterDeclaration cSharpParameterDeclaration in unitTest.ParameterDeclarations)
            {
                snapshot.Stubs.Add(cSharpParameterDeclaration);
            }

            _eater.Eat(snapshot, unitTest.Body);

            new PostEater().PostEat(snapshot);

            return snapshot;
        }
    }
}