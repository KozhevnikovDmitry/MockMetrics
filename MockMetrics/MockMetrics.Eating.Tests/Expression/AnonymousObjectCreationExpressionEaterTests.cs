using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AnonymousObjectCreationExpressionEaterTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void EatMemberDeclarations_TranslateInnerEatTest(bool innerEat)
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var memberDeclaration = Mock.Of<IAnonymousMemberDeclaration>(t => t.Expression == expression);
            var anonymousObjectCreationExpression = Mock.Of<IAnonymousObjectCreationExpression>();
            Mock.Get(anonymousObjectCreationExpression).Setup(t => t.AnonymousInitializer.MemberInitializers)
                .Returns(new TreeNodeCollection<IAnonymousMemberDeclaration>(new[] { memberDeclaration }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var anonymousObjectCreationExpressionEater = new AnonymousObjectCreationExpressionEater(eater.Object);

            // Act
            anonymousObjectCreationExpressionEater.Eat(snapshot, anonymousObjectCreationExpression, innerEat);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression, innerEat));
        }

        [Test]
        public void AddMemberDeclarationsToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var memberDeclaration = Mock.Of<IAnonymousMemberDeclaration>(t => t.Expression == expression);
            var anonymousObjectCreationExpression = Mock.Of<IAnonymousObjectCreationExpression>();
            Mock.Get(anonymousObjectCreationExpression).Setup(t => t.AnonymousInitializer.MemberInitializers)
                .Returns(new TreeNodeCollection<IAnonymousMemberDeclaration>(new[] { memberDeclaration }));
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression, false) == ExpressionKind.Stub);
            var anonymousObjectCreationExpressionEater = new AnonymousObjectCreationExpressionEater(eater);

            // Act
            anonymousObjectCreationExpressionEater.Eat(snapshot.Object, anonymousObjectCreationExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, memberDeclaration));
        }

        [Test]
        public void ReturnStubCandidateTest()
        {
            var anonymousObjectCreationExpression = Mock.Of<IAnonymousObjectCreationExpression>();
            Mock.Get(anonymousObjectCreationExpression).Setup(t => t.AnonymousInitializer.MemberInitializers)
                .Returns(new TreeNodeCollection<IAnonymousMemberDeclaration>(new IAnonymousMemberDeclaration[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var anonymousObjectCreationExpressionEater = new AnonymousObjectCreationExpressionEater(eater);

            // Act
            var kind = anonymousObjectCreationExpressionEater.Eat(snapshot, anonymousObjectCreationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);

        } 
    }
}