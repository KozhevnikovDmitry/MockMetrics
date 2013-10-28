using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class DeclarationStatementEater : IStatementEater<IDeclarationStatement>
    {
        public void Eat(Snapshot snapshot, IMethodDeclaration unitTest, IDeclarationStatement statement)
        {
            foreach (ILocalConstantDeclaration localConstantDeclaration in statement.ConstantDeclarationsEnumerable)
            {
                snapshot.Stubs.Add(localConstantDeclaration);
            }

            foreach (ILocalVariableDeclaration localVariableDeclaration in statement.VariableDeclarationsEnumerable)
            {
                EatLocalVariable(snapshot, unitTest, localVariableDeclaration);
            }
        }


        private void EatLocalVariable(Snapshot snapshot, IMethodDeclaration unitTest,
            ILocalVariableDeclaration declaration)
        {
            ExpressionKind kind = EatVariableInitializer(snapshot, unitTest, declaration.Initial);
            snapshot.AddTreeNode(kind, declaration);
        }

        private ExpressionKind EatVariableInitializer(Snapshot snapshot,
            IMethodDeclaration unitTest,
            IVariableInitializer initializer)
        {
            if (initializer is IExpressionInitializer)
            {
                return EatExpressionVariableInitializer(snapshot, unitTest, initializer as IExpressionInitializer);
            }

            if (initializer is IArrayInitializer)
            {
                foreach (
                    IVariableInitializer variableInitializer in (initializer as IArrayInitializer).ElementInitializers)
                {
                    ExpressionKind kind = EatVariableInitializer(snapshot, unitTest, variableInitializer);

                    snapshot.AddTreeNode(kind, variableInitializer);
                }

                return ExpressionKind.Stub;
            }


            if (initializer is IUnsafeCodeFixedPointerInitializer)
            {
                throw new NotImplementedException();
            }


            if (initializer is IUnsafeCodeStackAllocInitializer)
            {
                throw new NotImplementedException();
            }

            throw new NotSupportedException();
        }

        private ExpressionKind EatExpressionVariableInitializer(Snapshot snapshot,
            IMethodDeclaration unitTest,
            IExpressionInitializer initializer)
        {
            return Eater.Eat(snapshot, unitTest, initializer.Value);
        }
    }
}