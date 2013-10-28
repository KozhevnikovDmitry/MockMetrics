using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public class UnitTestEater
    {
        public Snapshot EatUnitTest(IMethodDeclaration unitTest)
        {
            // result snapshot
            var snapshot = new Snapshot(unitTest);

            // all parameters are stubs
            foreach (ICSharpParameterDeclaration cSharpParameterDeclaration in unitTest.ParameterDeclarations)
            {
                snapshot.Stubs.Add(cSharpParameterDeclaration);
            }

            Eater.Eat(snapshot, unitTest, unitTest.Body);

            new PostEater().PostEat(snapshot);

            return snapshot;
        }
    }
}