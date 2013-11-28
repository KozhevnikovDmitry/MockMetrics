using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class TypeEaterTests
    {
        [Test]
        public void EatCastType_DynamicAsStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var typeUsage = Mock.Of<IDynamicTypeUsage>();
            var helper = Mock.Of<EatExpressionHelper>();
            var typeEater = new TypeEater(helper);

            // Act
            var kind = typeEater.EatCastType(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatCastType_PredefinedAsStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var typeUsage = Mock.Of<IPredefinedTypeUsage>();;
            var helper = Mock.Of<EatExpressionHelper>();
            var typeEater = new TypeEater(helper);

            // Act
            var kind = typeEater.EatCastType(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatCastType_UserTypeAsTargetTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == true);
            var typeUsage = Mock.Of<IUserTypeUsage>();
            var classType = Mock.Of<ITypeElement>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
            var typeEater = new TypeEater(helper);

            // Act
            var kind = typeEater.EatCastType(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Target);
        }

        [Test]
        public void EatCastType_UserTypeAsMockTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == true);
            var typeUsage = Mock.Of<IUserTypeUsage>();
            var classType = Mock.Of<ITypeElement>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
            var typeEater = new TypeEater(helper);

            // Act
            var kind = typeEater.EatCastType(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatCastType_UserTypeAsStubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == false);
            var typeUsage = Mock.Of<IUserTypeUsage>();
            var classType = Mock.Of<ITypeElement>(t => t.Module.Name == "ModuleName");
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
            var typeEater = new TypeEater(helper);

            // Act
            var kind = typeEater.EatCastType(snapshot, typeUsage);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void EatVariableType_TargetTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == true);
            var type = Mock.Of<IType>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetTypeClass(type) == Mock.Of<ITypeElement>(e => e.Module.Name == "ModuleName"));
            var typeEater = new TypeEater(helper);

            // Act
            var kind = typeEater.EatVariableType(snapshot, type);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Target);
        }

        [Test]
        public void EatVariableType_MockTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == true);
            var type = Mock.Of<IType>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetTypeClass(type) == Mock.Of<ITypeElement>(e => e.Module.Name == "ModuleName"));
            var typeEater = new TypeEater(helper);

            // Act
            var kind = typeEater.EatVariableType(snapshot, type);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Mock);
        }

        [Test]
        public void EatVariableType_StubCandidateTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == false);
            var type = Mock.Of<IType>();
            var helper = Mock.Of<EatExpressionHelper>(t => t.GetTypeClass(type) == Mock.Of<ITypeElement>(e => e.Module.Name == "ModuleName"));
            var typeEater = new TypeEater(helper);

            // Act
            var kind = typeEater.EatVariableType(snapshot, type);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void NullEaterTest()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TypeEater(null));
        }
        
        [Test]
        public void EatCastType_NullSnapshotTest()
        {
            // Arrange
            var typeEater = new TypeEater(Mock.Of<EatExpressionHelper>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => typeEater.EatCastType(null, Mock.Of<ITypeUsage>()));
        }

        [Test]
        public void EatCastType_NullTypeUsageTest()
        {
            // Arrange
            var typeEater = new TypeEater(Mock.Of<EatExpressionHelper>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => typeEater.EatCastType(Mock.Of<ISnapshot>(), null));
        }

        [Test]
        public void EatVariableType_NullSnapshotTest()
        {
            // Arrange
            var typeEater = new TypeEater(Mock.Of<EatExpressionHelper>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => typeEater.EatVariableType(null, Mock.Of<IType>()));
        }

        [Test]
        public void EatVariableType_NullTypeTest()
        {
            // Arrange
            var typeEater = new TypeEater(Mock.Of<EatExpressionHelper>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => typeEater.EatVariableType(Mock.Of<ISnapshot>(), null));
        }
    }
}
