using System.Collections.Generic;
using HaveBox;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests
{
    [TestFixture]
    public class EaterTest
    {
        [Test]
        public void GetInstancesFromContainerTest()
        {
            // Arrange
            var container = new Container();
            container.Configure(config => config.MergeConfig(new EatingConfig(typeof(EatingConfig).Assembly, container)));

            // Act
            var eater = container.GetInstance<IEater>();
            var unitTestEater = container.GetInstance<UnitTestEater>();
            var expressionEaters = container.GetInstance<IEnumerable<IExpressionEater>>();
            var statementEaters = container.GetInstance<IEnumerable<IStatementEater>>();

            // Assert
            Assert.IsInstanceOf<IEater>(eater);
            Assert.IsInstanceOf<UnitTestEater>(unitTestEater);
            Assert.IsNotEmpty(expressionEaters);
            Assert.IsNotEmpty(statementEaters);
        }

        [Test]
        public void GetEatersFromEaterTest()
        {
            // Arrange
            var container = new Container();
            container.Configure(config => config.MergeConfig(new EatingConfig(typeof(EatingConfig).Assembly, container)));

            // Act 
            var eater = container.GetInstance<IEater>() as Eater;

            // Assert 
            Assert.DoesNotThrow(() => eater.GetEater(Mock.Of<IBlock>()));
            Assert.DoesNotThrow(() => eater.GetEater(Mock.Of<IDeclarationStatement>()));
            Assert.DoesNotThrow(() => eater.GetEater(Mock.Of<IExpressionStatement>()));
            Assert.DoesNotThrow(() => eater.GetEater(Mock.Of<IInvocationExpression>()));
            Assert.DoesNotThrow(() => eater.GetEater(Mock.Of<ICSharpLiteralExpression>()));
            Assert.DoesNotThrow(() => eater.GetEater(Mock.Of<IObjectCreationExpression>()));
            Assert.DoesNotThrow(() => eater.GetEater(Mock.Of<IReferenceExpression>()));
        }


        [Test]
        public void GetBinaryEatersFromEaterTest()
        {
            // Arrange
            var container = new Container();
            container.Configure(config => config.MergeConfig(new EatingConfig(typeof(EatingConfig).Assembly, container)));

            // Act 
            var eater = container.GetInstance<IEater>() as Eater;

            // Assert 
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IConditionalAndExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IConditionalOrExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IBitwiseAndExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IBitwiseExclusiveOrExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IBitwiseInclusiveOrExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IEqualityExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IMultiplicativeExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<INullCoalescingExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IRelationalExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IShiftExpression>()));
            Assert.IsInstanceOf<BinaryExpressionEater>(eater.GetEater(Mock.Of<IAdditiveExpression>()));
        }
    }
}
