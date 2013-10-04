using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eat
{
    public class UnitTestEater 
    {
        public Snapshot EatUnitTest(IMethodDeclaration unitTest)
        {
            var snapshot = new Snapshot(unitTest);
            var regularParameterEater = new ParameterEater();
            foreach (var cSharpParameterDeclaration in unitTest.ParameterDeclarations)
            {
                regularParameterEater.Eat(snapshot, unitTest, cSharpParameterDeclaration);
            }
            Eater.Eat(snapshot, unitTest, unitTest.Body);
            new PostEater().PostEat(snapshot);

            return snapshot;
        }
    }
}
