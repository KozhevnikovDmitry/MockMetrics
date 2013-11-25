using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ObjectCreationExpressionEaterTests
    {
        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.VALUE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(args); 
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argsEater = new Mock<IArgumentsEater>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, Mock.Of<EatExpressionHelper>(), argsEater.Object);

            // Act
            objectCreationExpressionEater.Eat(snapshot, objectCreationExpression, false);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }

        [Test]
        public void EatMemberInitializersWithInnerEatTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var memberInitializer = Mock.Of<IMemberInitializer>(t => t.Expression == expression);
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.VALUE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Initializer.InitializerElements)
                .Returns(new TreeNodeCollection<IInitializerElement>(new[] { memberInitializer }));
            var argsEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater.Object, Mock.Of<EatExpressionHelper>(), argsEater);

            // Act
            objectCreationExpressionEater.Eat(snapshot, objectCreationExpression, false);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression, true));
        }

        [Test]
        public void NotAddMemberInitializersToSnapshotTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var memberInitializer = Mock.Of<IMemberInitializer>(t => t.Expression == expression);
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.VALUE_TYPE);
            Mock.Get(objectCreationExpression).Setup(t => t.Initializer.InitializerElements)
                .Returns(new TreeNodeCollection<IInitializerElement>(new[] { memberInitializer }));
            var argsEater = Mock.Of<IArgumentsEater>();
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression, false) == ExpressionKind.Stub);
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, Mock.Of<EatExpressionHelper>(), argsEater);

            // Act
            objectCreationExpressionEater.Eat(snapshot.Object, objectCreationExpression, false);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.Stub, memberInitializer), Times.Never);
        }

        [Test]
        public void EatValueTypeTest()
        {   
            // Arrange
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.VALUE_TYPE);
            var argsEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, Mock.Of<EatExpressionHelper>(), argsEater);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatTargetTest()
        {
            // Arrange
            var classType = Mock.Of<IClass>(t => t.Module.Name == "Project");
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.REFERENCE_TYPE);
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetCreationClass(objectCreationExpression) == classType);
            var argsEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("Project") == true);
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, helper, argsEater);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Target);
        }

        [Test]
        public void EatHandyMockTest()
        {
            // Arrange
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ProjectTest");
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.REFERENCE_TYPE);
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetCreationClass(objectCreationExpression) == classType);
            var argsEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestProject("ProjectTest") == true);
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, helper, argsEater);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatMoqMockTest()
        {
            // Arrange
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ProjectTest");
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.REFERENCE_TYPE);
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetCreationClass(objectCreationExpression) == classType
                                                        && t.GetCreationTypeName(objectCreationExpression) == "Moq.Mock OLOLO");
            var argsEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, helper, argsEater);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatStubWhenNoOneOtherTest()
        {
            // Arrange
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ProjectTest");
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type().Classify == TypeClassification.REFERENCE_TYPE);
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetCreationClass(objectCreationExpression) == classType
                                                        && t.GetCreationTypeName(objectCreationExpression) == "OLOLO");
            var argsEater = Mock.Of<IArgumentsEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var objectCreationEater = new ObjectCreationExpressionEater(eater, helper, argsEater);

            // Act
            var kind = objectCreationEater.Eat(snapshot, objectCreationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }
    }
}