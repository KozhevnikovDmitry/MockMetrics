﻿using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class ElementAccessExpressionEater : ExpressionEater<IElementAccessExpression>
    {
        private readonly IArgumentsEater _argumentsEater;

        public ElementAccessExpressionEater(IEater eater,
                                            IArgumentsEater argumentsEater)
            : base(eater)
        {
            _argumentsEater = argumentsEater;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IElementAccessExpression expression)
        {
            _argumentsEater.Eat(snapshot, expression.Arguments);

            // TODO : cover by functional tests
            // TODO : what if array of results or targets
            Eater.Eat(snapshot, expression.Operand);

            return ExpressionKind.StubCandidate;
        }
    }
}
