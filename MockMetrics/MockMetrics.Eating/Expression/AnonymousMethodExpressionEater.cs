﻿using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class AnonymousMethodExpressionEater : ExpressionEater<IAnonymousMethodExpression>
    {
        public AnonymousMethodExpressionEater(IEater eater) : base(eater)
        {
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IAnonymousMethodExpression expression)
        {
            foreach (var anonymousMethodParameterDeclaration in expression.ParameterDeclarations)
            {
                Eater.Eat(snapshot, anonymousMethodParameterDeclaration);
            }

            Eater.Eat(snapshot, expression.Body);

            return ExpressionKind.StubCandidate;
        }
    }
}
