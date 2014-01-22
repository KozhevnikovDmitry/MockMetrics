using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using MockMetrics.Eating.MoqFake;

namespace MockMetrics.Eating.Helpers
{
    // TODO : cover by unit tests
    public class MoqSyntaxHelper
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public MoqSyntaxHelper([NotNull] EatExpressionHelper eatExpressionHelper)
        {
            if (eatExpressionHelper == null) throw new ArgumentNullException("eatExpressionHelper");
            _eatExpressionHelper = eatExpressionHelper;
        }

        private string[] ICallback = { "Callback" };
        private string[] IThrows = { "Throws" };
        private string[] MockMock = { "Get" };
        private string[] MockFakeProperty = { "SetupGet", "SetupSet", "VerifyGet", "VerifySet", "SetupProperty", "SetupAllProperties" };
        private string[] MockFakeWithOptions = { "Setup", "Verify" };
        private string[] MockStubWithOptions = { "Of" };
        private string[] ItFakeWithoutOptions = { "IsAny", "IsIn", "IsInRange", "IsNotIn", "IsRegex", "IsNotNull" };
        private string[] ItStubWithOptions = { "Is" };


        public virtual MoqInvoke? GetMoqInvokeType([NotNull] IInvocationExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            
            var clrElement = _eatExpressionHelper.GetInvokedElement(expression) as IClrDeclaredElement;
            if (clrElement == null)
            {
                return MoqInvoke.None;
            } 
            
            var module = clrElement.Module;
            if (module.Name != "Moq")
            {
                return MoqInvoke.None;
            }
            var type = clrElement.GetContainingType();
            if (type == null)
            {
                return MoqInvoke.None;
            }

            var elementName = clrElement.ShortName;
            var typeName = type.GetClrName().FullName;

            if (module.Name == "Moq")
            {
                if (typeName == "Moq.Mock" || typeName == "Moq.Mock`1")
                {
                    if (MockMock.Contains(elementName))
                    {
                        return MoqInvoke.Mock;
                    }

                    if (MockFakeProperty.Contains(elementName))
                    {
                        return MoqInvoke.FakeProperty;
                    }

                    if (MockFakeWithOptions.Contains(elementName))
                    {
                        return MoqInvoke.FakeWithOptions;
                    }

                    if (MockStubWithOptions.Contains(elementName))
                    {
                        return MoqInvoke.StubWithOptions;
                    }
                }

                if (typeName == "Moq.Language.IThrows")
                {
                    if (IThrows.Contains(elementName))
                    {
                        return MoqInvoke.FakeException;
                    }
                }

                if (typeName == "Moq.Language.ICallback`2" || typeName == "Moq.Language.ICallback")
                {
                    if (ICallback.Contains(elementName))
                    {
                        return MoqInvoke.FakeCallback;
                    }
                }

                if (typeName == "Moq.It")
                {
                    if (ItFakeWithoutOptions.Contains(elementName))
                    {
                        return MoqInvoke.FakeWithoutOptions;
                    }

                    if (ItStubWithOptions.Contains(elementName))
                    {
                        return MoqInvoke.StubWithOptions;
                    }
                }
            }

            return MoqInvoke.None;
        }
    }
}