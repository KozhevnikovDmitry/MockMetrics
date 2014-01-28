using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GU.BL.Extensions
{
    public static class RecursiveExtensions
    {
        /// <summary>
        /// Получение всех потомков узла в дереве
        /// </summary>
        /// <typeparam name="T">тип узла дерева</typeparam>
        /// <param name="node">исходный узел дерева</param>
        /// <param name="childrenSelector">функция получения непосредственных потомков узла</param>
        /// <param name="includeNode">включать ли в результат исходный узел</param>
        /// <example>
        /// <c>node.Descendants(t => t.Children.OrderBy(tt => tt.Name))</c>
        /// <c>"c:\\".Descendants(Directory.EnumerateDirectories)</c>
        /// </example>
        public static IEnumerable<T> Descendants<T>(this T node, Func<T, IEnumerable<T>> childrenSelector, bool includeNode = false) where T : class
        {
            if (node == null) throw new ArgumentNullException("node");
            if (childrenSelector == null) throw new ArgumentNullException("childrenSelector");
            
            var stack = new Stack<T>();

            if (includeNode)
                stack.Push(node);
            else
            {
                var children = childrenSelector(node);
                if (children != null)
                    foreach (var child in children.Reverse())
                        stack.Push(child);
            }

            while (stack.Count > 0)
            {
                var curNode = stack.Pop();
                yield return curNode;

                var children = childrenSelector(curNode);
                if (children != null)
                    // reverse - чтобы сохранить изначальный порядок
                    foreach (var child in children.Reverse())
                        stack.Push(child);
            }
        }

        /// <summary>
        /// Получение всех предков узла в дереве
        /// </summary>
        /// <typeparam name="T">тип узла дерева</typeparam>
        /// <param name="node">исходный узел дерева</param>
        /// <param name="parent">функция получения родительского узла</param>
        /// <param name="includeNode">включать ли в результат исходный узел</param>
        public static IEnumerable<T> Ancestors<T>(this T node, Func<T, T> parent, bool includeNode) where T : class
        {
            if (node == null) throw new ArgumentNullException("node");
            if (parent == null) throw new ArgumentNullException("parent"); 
            
            var curNode = includeNode ? node : parent(node);
            while (curNode != null)
            {
                yield return curNode;
                curNode = parent(curNode);
            }
        }

        /// <summary>
        /// Получение корневого предка узла в дереве
        /// </summary>
        /// <typeparam name="T">тип узла дерева</typeparam>
        /// <param name="node">исходный узел дерева</param>
        /// <param name="parent">функция получения родительского узла</param>
        /// <returns></returns>
        public static T Root<T>(this T node, Func<T, T> parent) where T:class
        {
            return Ancestors(node, parent, true).Last();
        }
    }
}
