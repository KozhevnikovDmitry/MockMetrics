using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Statement
{
    public class DeclarationStatementEater : StatementEater<IDeclarationStatement>
    {
        public DeclarationStatementEater(IEater eater)
            : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, IDeclarationStatement statement)
        {
            foreach (ILocalConstantDeclaration localConstantDeclaration in statement.ConstantDeclarationsEnumerable)
            {
                snapshot.Stubs.Add(localConstantDeclaration);
            }

            foreach (ILocalVariableDeclaration localVariableDeclaration in statement.VariableDeclarationsEnumerable)
            {
                EatLocalVariable(snapshot, localVariableDeclaration);
            }
        }


        private void EatLocalVariable(ISnapshot snapshot,
            ILocalVariableDeclaration declaration)
        {
            ExpressionKind kind = EatVariableInitializer(snapshot, declaration.Initial);
            snapshot.AddTreeNode(kind, declaration);
        }

        private ExpressionKind EatVariableInitializer(ISnapshot snapshot,
            IVariableInitializer initializer)
        {
            if (initializer is IExpressionInitializer)
            {
                return EatExpressionVariableInitializer(snapshot, initializer as IExpressionInitializer);
            }

            if (initializer is IArrayInitializer)
            {
                foreach (IVariableInitializer variableInitializer in (initializer as IArrayInitializer).ElementInitializers)
                {
                    ExpressionKind kind = EatVariableInitializer(snapshot, variableInitializer);

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

        private ExpressionKind EatExpressionVariableInitializer(ISnapshot snapshot,
            IExpressionInitializer initializer)
        {
            return Eater.Eat(snapshot, initializer.Value);
        }
    }
}