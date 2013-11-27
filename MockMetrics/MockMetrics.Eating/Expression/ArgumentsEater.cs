using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public interface IArgumentsEater
    {
        void Eat(ISnapshot snapshot, TreeNodeCollection<ICSharpArgument> arguements);
    }

    public class ArgumentsEater : IArgumentsEater, ICSharpNodeEater
    {
        private readonly IEater _eater;

        public ArgumentsEater(IEater eater)
        {
            _eater = eater;
        }

        public void Eat([NotNull] ISnapshot snapshot, TreeNodeCollection<ICSharpArgument> arguements)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            foreach (ICSharpArgument arg in arguements)
            {
                _eater.Eat(snapshot, arg.Value);
            }
        }
    }
}
