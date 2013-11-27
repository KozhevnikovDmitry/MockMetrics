using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.MoqStub
{
    public interface IMoqStubOptionsEater
    {
        void EatStubOptions(ISnapshot snapshot, ICSharpExpression options);
    }

    public class MoqStubOptionsEater : IMoqStubOptionsEater, ICSharpNodeEater
    {
        private readonly IEater _eater;
        private readonly IMoqStubOptionTargetEater _moqStubOptionTargetEater;

        public MoqStubOptionsEater(IEater eater, IMoqStubOptionTargetEater moqStubOptionTargetEater)
        {
            _eater = eater;
            _moqStubOptionTargetEater = moqStubOptionTargetEater;
        }

        public virtual void EatStubOptions(ISnapshot snapshot, ICSharpExpression options)
        {
            if (options is IConditionalAndExpression)
            {
                EatStubOptions(snapshot, (options as IConditionalAndExpression).LeftOperand);
                EatStubOptions(snapshot, (options as IConditionalAndExpression).RightOperand);
                return;
            }

            // Option target (method or property) can be positioned only in the left side of equality expression
            if (options is IEqualityExpression)
            {
                EatStubOptions(snapshot, (options as IEqualityExpression).LeftOperand);
                _eater.Eat(snapshot, (options as IEqualityExpression).RightOperand);
                return;
            }

            if (options is IInvocationExpression)
            {
                var optType = _moqStubOptionTargetEater.EatOption(snapshot, options as IInvocationExpression);
                snapshot.AddFakeOption(options, optType);
                return;
            }

            if (options is IReferenceExpression)
            {
                var optType = _moqStubOptionTargetEater.EatOption(snapshot, options as IReferenceExpression);
                snapshot.AddFakeOption(options, optType);
                return;
            }

            throw new MoqStubOptionWrongTypeException(this, options);
        }
    }

}