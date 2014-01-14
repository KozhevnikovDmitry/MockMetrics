﻿using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AnonymousObjectCreationExpressionEaterTests
    {
        [Test]
        public void EatMemberDeclarationsTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var memberDeclaration = Mock.Of<IAnonymousMemberDeclaration>(t => t.Expression == expression);
            var anonymousObjectCreationExpression = Mock.Of<IAnonymousObjectCreationExpression>();
            Mock.Get(anonymousObjectCreationExpression).Setup(t => t.AnonymousInitializer.MemberInitializers)
                .Returns(new TreeNodeCollection<IAnonymousMemberDeclaration>(new[] { memberDeclaration }));
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == Variable.None);
            var anonymousObjectCreationExpressionEater = new AnonymousObjectCreationExpressionEater(eater);

            // Act
            anonymousObjectCreationExpressionEater.Eat(snapshot.Object, anonymousObjectCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddVariable(memberDeclaration, Variable.None), Times.Once());
        }

        [Test]
        public void ReturnStubCandidateTest()
        {
            var anonymousObjectCreationExpression = Mock.Of<IAnonymousObjectCreationExpression>();
            Mock.Get(anonymousObjectCreationExpression).Setup(t => t.AnonymousInitializer.MemberInitializers)
                .Returns(new TreeNodeCollection<IAnonymousMemberDeclaration>(new IAnonymousMemberDeclaration[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var anonymousObjectCreationExpressionEater = new AnonymousObjectCreationExpressionEater(eater);

            // Act
            var metrics = anonymousObjectCreationExpressionEater.Eat(snapshot, anonymousObjectCreationExpression);

            // Assert
            Assert.AreEqual(metrics, Variable.Library);

        } 
    }
}