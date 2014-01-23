using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating
{
    public interface INodeEater
    {
        Type NodeType { get; }
    }

    public abstract class NodeEater<T> :INodeEater, ICSharpNodeEater where T : ICSharpTreeNode
    {
        protected Variable EatNode([NotNull] ISnapshot snapshot, [NotNull] ICSharpTreeNode node, Func<ISnapshot, T, Variable> eat)
        {
            var result = Variable.None;
            Wrapper(snapshot, node, (s, n) =>
            {
                result = eat(s, n);
            });

            return result;
        }

        protected void EatNode([NotNull] ISnapshot snapshot, [NotNull] ICSharpTreeNode node, Action<ISnapshot, T> eat)
        {
            Wrapper(snapshot, node, eat);
        }

        private void Wrapper([NotNull] ISnapshot snapshot, [NotNull] ICSharpTreeNode node, Action<ISnapshot, T> action)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");

            if (node == null)
                throw new ArgumentNullException("node");

            try
            {
                if (node is T)
                {
                    action(snapshot, (T)node);
                    return;
                }

                throw new UnexpectedTypeOfNodeToEatException(typeof(T), this, node);
            }

#if DEBUG
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EatingException("Unexpected exception", ex, this, node);
            }
#else
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EatingException("Unexpected exception", ex, this, node);
            }
#endif
        }

        public Type NodeType { get { return typeof (T); } }
    }
}