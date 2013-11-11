using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HaveBox;
using HaveBox.Configuration;

namespace MockMetrics.Eating
{
    public class EatingConfig : Config
    {
        public EatingConfig(Assembly assembly, IContainer container)
        {
            For<IContainer>().Use(() => container);
            For<Eater>().Use<Eater>().AsSingleton();
            For<UnitTestEater>().Use<UnitTestEater>().AsSingleton();
            assembly.GetTypes()
                    .Where(type => type.GetInterfaces().Contains(typeof(IExpressionEater)) && !type.IsInterface && !type.IsAbstract)
                    .Each(type => type.GetInterfaces().Each(interfaze => For(interfaze).Use(type).AsSingleton()));

            assembly.GetTypes()
                    .Where(type => type.GetInterfaces().Contains(typeof(IStatementEater)) && !type.IsInterface && !type.IsAbstract)
                    .Each(type => type.GetInterfaces().Each(interfaze => For(interfaze).Use(type).AsSingleton()));

        }
    }

    internal static class IEnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}