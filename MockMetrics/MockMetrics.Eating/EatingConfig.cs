using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HaveBox;
using HaveBox.Configuration;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using MockMetrics.Eating.VariableDeclaration;

namespace MockMetrics.Eating
{
    public class EatingConfig : Config
    {
        public EatingConfig(Assembly assembly, IContainer container)
        {
            For<IContainer>().Use(() => container);
            For<IEater>().Use<Eater>().AsSingleton();
            For<EatExpressionHelper>().Use<EatExpressionHelper>().AsSingleton();
            For<ExpressionKindHelper>().Use<ExpressionKindHelper>().AsSingleton();
            For<ITypeUsageEater>().Use<TypeUsageEater>().AsSingleton();
            For<IVariableInitializerEater>().Use<VariableInitializerEater>().AsSingleton();
            For<UnitTestEater>().Use<UnitTestEater>().AsSingleton();
            assembly.GetTypes()
                    .Where(type => type.GetInterfaces().Contains(typeof(IExpressionEater)) && !type.IsInterface && !type.IsAbstract)
                    .Each(type => type.GetInterfaces().Each(interfaze => For(interfaze).Use(type).AsSingleton()));

            assembly.GetTypes()
                    .Where(type => type.GetInterfaces().Contains(typeof(IStatementEater)) && !type.IsInterface && !type.IsAbstract)
                    .Each(type => type.GetInterfaces().Each(interfaze => For(interfaze).Use(type).AsSingleton()));

            assembly.GetTypes()
                    .Where(type => type.GetInterfaces().Contains(typeof(IVariableDeclarationEater)) && !type.IsInterface && !type.IsAbstract)
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