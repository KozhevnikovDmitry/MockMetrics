using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.VariableDeclaration;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class VariableInitializerEaterTests
    {
        [Test]
        public void EatArrayInitializerTest()
        {
            // Arrange
            var arrayInit = Mock.Of<IArrayInitializer>();
            Mock.Get(arrayInit).Setup(t => t.ElementInitializers)
                .Returns(new TreeNodeCollection<IVariableInitializer>(new IVariableInitializer[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var initializerEater = new VariableInitializerEater(eater, Mock.Of<IMetricHelper>());

            // Act
            var metrics = initializerEater.Eat(snapshot, arrayInit);

            // Assert
            Assert.AreEqual(metrics.Scope, Scope.Local);
            Assert.AreEqual(metrics.Variable, Variable.Data);
        }

        [Test]
        public void EatArrayItemsRecoursivlyTests()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IExpressionInitializer>(t => t.Value == expression);
            var arrayInit = Mock.Of<IArrayInitializer>();
            Mock.Get(arrayInit).Setup(t => t.ElementInitializers)
                .Returns(new TreeNodeCollection<IVariableInitializer>(new[] { itemInitializer }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            eater.Setup(t => t.Eat(snapshot, expression)).Returns(Metrics.Create());
            var initializerEater = new VariableInitializerEater(eater.Object, Mock.Of<IMetricHelper>());

            // Act
            initializerEater.Eat(snapshot, arrayInit);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression), Times.Once());
        }

        [Test]
        public void EatExpressionInitializerTest()
        {
            // Arrange
            var initial = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IExpressionInitializer>(t => t.Value == initial);
            var snapshot = Mock.Of<ISnapshot>();
            var initialMetrics = Metrics.Create();
            var eater = Mock.Of<IEater>();
            Mock.Get(eater).Setup(t => t.Eat(snapshot, initial)).Returns(initialMetrics);
            var helper = Mock.Of<IMetricHelper>();
            var initializerEater = new VariableInitializerEater(eater, helper);

            // Act
            var result = initializerEater.Eat(snapshot, itemInitializer);

            // Assert
            Assert.AreEqual(result, initialMetrics);
            Assert.AreEqual(result.Scope, Scope.Local);
        }

        [Test]
        public void EatUnsafeCodeFixedPointerInitializerTest()
        { 
            // Arrange
            var initial = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IUnsafeCodeFixedPointerInitializer>(t => t.Value == initial);
            var snapshot = Mock.Of<ISnapshot>();
            var initialMetrics = Metrics.Create();
            var eater = Mock.Of<IEater>();
            Mock.Get(eater).Setup(t => t.Eat(snapshot, initial)).Returns(initialMetrics);
            var helper = Mock.Of<IMetricHelper>();
            var initializerEater = new VariableInitializerEater(eater, helper);

            // Act
            var result = initializerEater.Eat(snapshot, itemInitializer);

            // Assert
            Assert.AreEqual(result, initialMetrics);
            Assert.AreEqual(result.Scope, Scope.Local);
        }

        [Test]
        public void EatUnsafeCodeStackAllocInitializerTest()
        {
            // Arrange
            var initial = Mock.Of<ICSharpExpression>();
            var itemInitializer = Mock.Of<IUnsafeCodeStackAllocInitializer>(t => t.DimExpr == initial);
            var snapshot = Mock.Of<ISnapshot>();
            var initialMetrics = Metrics.Create();
            var eater = Mock.Of<IEater>();
            Mock.Get(eater).Setup(t => t.Eat(snapshot, initial)).Returns(initialMetrics);
            var helper = Mock.Of<IMetricHelper>();
            var initializerEater = new VariableInitializerEater(eater, helper);

            // Act
            var result = initializerEater.Eat(snapshot, itemInitializer);

            // Assert
            Assert.AreEqual(result, initialMetrics);
            Assert.AreEqual(result.Scope, Scope.Local);
        }

        [Test]
        public void NullSnapshotTest()
        {
            // Arrange
            var initializerEater = new VariableInitializerEater(Mock.Of<IEater>(), Mock.Of<IMetricHelper>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => initializerEater.Eat(null, Mock.Of<IExpressionInitializer>()));
        }

        [Test]
        public void NullInitializerTest()
        {
            // Arrange
            var initializerEater = new VariableInitializerEater(Mock.Of<IEater>(), Mock.Of<IMetricHelper>());
            
            // Assert
            Assert.Throws<ArgumentNullException>(() => initializerEater.Eat(Mock.Of<ISnapshot>(), null));
        }
    }
}