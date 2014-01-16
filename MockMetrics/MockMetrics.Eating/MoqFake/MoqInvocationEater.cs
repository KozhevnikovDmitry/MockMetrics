using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqFake
{
    public interface IMoqInvocationEater
    {
        Variable Eat(ISnapshot snapshot, IInvocationExpression expression);

        bool ContainsFakeOptions(IInvocationExpression expression);
    }


    // TODO : cover by unit tests
    public class MoqInvocationEater : IMoqInvocationEater, ICSharpNodeEater
    {
        private readonly IMoqFakeOptionEater _moqFakeOptionEater;
        private readonly EatExpressionHelper _eatExpressionHelper;
        private readonly MoqSyntaxHelper _moqSyntaxHelper;
        private readonly IInvocationStuffEater _invocationStuffEater;

        public MoqInvocationEater([NotNull] IMoqFakeOptionEater moqFakeOptionEater,
                                  [NotNull] EatExpressionHelper eatExpressionHelper,
                                  [NotNull] MoqSyntaxHelper moqSyntaxHelper,
                                  [NotNull] IInvocationStuffEater invocationStuffEater)
        {
            if (moqFakeOptionEater == null) throw new ArgumentNullException("moqFakeOptionEater");
            if (moqSyntaxHelper == null) throw new ArgumentNullException("moqSyntaxHelper");
            if (invocationStuffEater == null) throw new ArgumentNullException("invocationStuffEater");

            _moqFakeOptionEater = moqFakeOptionEater;
            _eatExpressionHelper = eatExpressionHelper;
            _moqSyntaxHelper = moqSyntaxHelper;
            _invocationStuffEater = invocationStuffEater;
        }

        public Variable Eat(ISnapshot snapshot, IInvocationExpression expression)
        {
            _invocationStuffEater.Eat(snapshot, expression);

            Variable result = ChooseMethod(snapshot, expression) ?? Variable.Library;

            if (_eatExpressionHelper.IsStandaloneExpression(expression))
            {
                snapshot.AddVariable(expression, result);
            }

            return result;
        }

        private Variable? ChooseMethod(ISnapshot snapshot, IInvocationExpression expression)
        {
            var moqInvoke = _moqSyntaxHelper.GetMoqInvokeType(expression);

            switch (moqInvoke)
            {
                case MoqInvoke.None:
                    {
                        return Variable.None;
                    }
                case MoqInvoke.Mock:
                    {
                        AddMock(snapshot, expression);
                        return Variable.Mock;
                    }
                case MoqInvoke.FakeProperty:
                    {
                        snapshot.AddFakeOption(expression, FakeOption.Property);
                        return Variable.None;
                    }
                case MoqInvoke.FakeCallback:
                    {
                        snapshot.AddFakeOption(expression, FakeOption.CallBack);
                        return Variable.None;
                    }
                case MoqInvoke.FakeException:
                    {
                        snapshot.AddFakeOption(expression, FakeOption.Exception);
                        return Variable.None;
                    }
                case MoqInvoke.FakeWithOptions:
                    {
                        _moqFakeOptionEater.Eat(snapshot, expression);
                        return Variable.None;
                    }
                case MoqInvoke.StubWithOptions:
                    {
                        _moqFakeOptionEater.Eat(snapshot, expression);
                        return Variable.Stub;
                    }
                case MoqInvoke.FakeWithoutOptions:
                    {
                        return Variable.Stub;
                    }
            }

            return null;
        }

        public bool ContainsFakeOptions(IInvocationExpression expression)
        {
            var invokedName = _eatExpressionHelper.GetInvokedElementName(expression);
            return invokedName.StartsWith("Method:Moq");
        }

        private void AddMock(ISnapshot snapshot, IInvocationExpression expression)
        {
            if (!expression.Arguments.IsSingle())
            {
                throw new NotSingleMockGetInvacotaionArgumentException(this, expression);
            }

            var mockRef = expression.Arguments.Single().Value as IReferenceExpression;
            if (mockRef != null)
                snapshot.AddVariable(mockRef, Variable.Mock);
        }
    }
}