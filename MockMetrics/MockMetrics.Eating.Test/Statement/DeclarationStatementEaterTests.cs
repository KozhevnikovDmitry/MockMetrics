using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Test.Statement
{
    [TestFixture]
    public class DeclarationStatementEaterTests
    {
        [Test]
        public void AddConstantsToSnapshotAsStubsTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var constantDeclaration = Mock.Of<ILocalConstantDeclaration>();
            var declarationStatement = Mock.Of<IDeclarationStatement>();
            Mock.Get(declarationStatement)
               .Setup(t => t.ConstantDeclarations)
               .Returns(new TreeNodeCollection<ILocalConstantDeclaration>(new[] { constantDeclaration }));
            Mock.Get(declarationStatement)
               .Setup(t => t.VariableDeclarations)
               .Returns(new TreeNodeCollection<ILocalVariableDeclaration>(new ILocalVariableDeclaration[0]));
          
            var eater = new Mock<IEater>();
            var declarationStatementEater = new DeclarationStatementEater(eater.Object);

            // Act
            declarationStatementEater.Eat(snapshot, declarationStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, constantDeclaration), Times.Once);
        }

        [Test]
        public void EatLocalVariablesTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var localVariableDeclaration = Mock.Of<ILocalVariableDeclaration>();
            var declarationStatement = Mock.Of<IDeclarationStatement>();
            Mock.Get(declarationStatement)
               .Setup(t => t.ConstantDeclarations)
               .Returns(new TreeNodeCollection<ILocalConstantDeclaration>(new ILocalConstantDeclaration[0]));
            Mock.Get(declarationStatement)
               .Setup(t => t.VariableDeclarations)
               .Returns(new TreeNodeCollection<ILocalVariableDeclaration>(new[] { localVariableDeclaration }));

            var eater = new Mock<IEater>();
            var declarationStatementEater = new DeclarationStatementEater(eater.Object);

            // Act
            declarationStatementEater.Eat(snapshot, declarationStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, localVariableDeclaration), Times.Once);
        }
    }
}