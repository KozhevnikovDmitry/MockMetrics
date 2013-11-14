using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class TypeUsageEaterTests
    {
        [Test]
        public void EatDynamicAsStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var typeUsage = Mock.Of<IDynamicTypeUsage>();
            var helper = Mock.Of<EatExpressionHelper>();
            var defaultExpressionEater = new TypeUsageEater(helper);

            // Act
            var kind = defaultExpressionEater.Eat(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatPredefinedAsStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var typeUsage = Mock.Of<IPredefinedTypeUsage>();;
            var helper = Mock.Of<EatExpressionHelper>();
            var defaultExpressionEater = new TypeUsageEater(helper);

            // Act
            var kind = defaultExpressionEater.Eat(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatUserTypeAsTargetTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == true);
            var typeUsage = Mock.Of<IUserTypeUsage>();
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
            var defaultExpressionEater = new TypeUsageEater(helper);

            // Act
            var kind = defaultExpressionEater.Eat(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Target);
        }

        [Test]
        public void EatUserTypeAsMockTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == true);
            var typeUsage = Mock.Of<IUserTypeUsage>();
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
            var defaultExpressionEater = new TypeUsageEater(helper);

            // Act
            var kind = defaultExpressionEater.Eat(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatUserTypeAsStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == false);
            var typeUsage = Mock.Of<IUserTypeUsage>();
            var classType = Mock.Of<IClass>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
            var defaultExpressionEater = new TypeUsageEater(helper);

            // Act
            var kind = defaultExpressionEater.Eat(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }
    }
}
