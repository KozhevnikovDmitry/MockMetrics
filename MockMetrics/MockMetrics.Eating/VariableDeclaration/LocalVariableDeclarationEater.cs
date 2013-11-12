using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class LocalVariableDeclarationEater : VariableDeclarationEater<ILocalVariableDeclaration>
    {
        public LocalVariableDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override void Eat(ISnapshot snapshot, ILocalVariableDeclaration variableDeclaration)
        {
            ExpressionKind kind = EatVariableInitializer(snapshot, variableDeclaration.Initial);
            snapshot.AddTreeNode(kind, variableDeclaration);
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
                foreach (
                    IVariableInitializer variableInitializer in (initializer as IArrayInitializer).ElementInitializers)
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
