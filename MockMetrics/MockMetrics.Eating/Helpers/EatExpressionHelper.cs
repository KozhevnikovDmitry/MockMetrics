using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util.DataStructures;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Helpers
{
    public class EatExpressionHelper
    {
        public virtual ITypeElement GetUserTypeUsageClass([NotNull] IUserTypeUsage userTypeUsage)
        {
            if (userTypeUsage == null)
                throw new ArgumentNullException("userTypeUsage");

            if (userTypeUsage.ScalarTypeName.Reference.CurrentResolveResult != null)
            {
                return userTypeUsage.ScalarTypeName.Reference.CurrentResolveResult.DeclaredElement as ITypeElement;
            }
            else
            {
                throw new ExpressionHelperException("Null resolved result of type usage", userTypeUsage);
            }
        }

        public virtual ITypeElement GetTypeClass([NotNull] IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsUnknown)
            {
                return new NullTypeElement();
            }

            var scalarType = type.GetScalarType();
            if (scalarType != null)
            {
                var typeElement = scalarType.GetTypeElement();
                if (typeElement != null)
                {
                    return typeElement;
                }
            }

            return new NullTypeElement();
        }

        public virtual IDeclaredElement GetInvokedElement([NotNull] IInvocationExpression invocationExpression)
        {
            var result = invocationExpression.InvocationExpressionReference.CurrentResolveResult;

            if (result != null)
            {
                if (result.DeclaredElement != null)
                {
                    return result.DeclaredElement;
                }
                
                if (result.Result.Candidates.Any())
                {
                    return result.Result.Candidates.First();
                }
            }
            return new NullDeclaredElement();
        }

        public virtual bool IsInternalMethod(IInvocationExpression expression, ISnapshot snapshot)
        {
            var method = GetInvokedElement(expression) as IMethod;
            if (method != null)
            {
                if (method.GetContainingType() ==
                   snapshot.UnitTest.DeclaredElement.GetContainingType())
                {
                    return true;
                }
            }

            return false;
        }

        public virtual IDeclaredElement GetReferenceElement([NotNull] IReferenceExpression referenceExpression)
        {
            if (referenceExpression == null)
                throw new ArgumentNullException("referenceExpression");

            var result = referenceExpression.Reference.CurrentResolveResult;

            if (result != null)
            {
                if (result.DeclaredElement != null)
                {
                    return result.DeclaredElement;
                }

                if (result.Result.Candidates.Any())
                {
                    return result.Result.Candidates.First();
                }
            }

            return new NullDeclaredElement();
        }
        
        public virtual ICSharpDeclaration GetReferenceDeclaration([NotNull] IReferenceExpression referenceExpression)
        {
            if (referenceExpression == null)
                throw new ArgumentNullException("referenceExpression");
            var declaredElement = GetReferenceElement(referenceExpression);

            if (declaredElement is NullDeclaredElement)
            {
                return new NullCsharpDeclaration();
            }

            var declaration = declaredElement as ICSharpDeclaration;

            if (declaration != null)
            {
                return declaration;
            }

            declaration = declaredElement.GetDeclarations().FirstOrDefault() as ICSharpDeclaration;

            if (declaration != null)
            {
                return declaration;
            }

            return new NullCsharpDeclaration();
        }

        public virtual string GetInvokedElementName([NotNull] IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null)
                throw new ArgumentNullException("invocationExpression");
            
            return GetInvokedElement(invocationExpression).ToString();
        }

        public virtual bool IsStandaloneExpression([NotNull] IInvocationExpression expression)
        {
            return IsStandaloneExpr(expression);
        }

        public virtual bool IsStandaloneExpression([NotNull] ICSharpLiteralExpression expression)
        {
            return IsStandaloneExpr(expression);
        }

        public virtual bool IsStandaloneExpression([NotNull] IReferenceExpression expression)
        {
            return IsStandaloneExpr(expression);
        }

        private bool IsStandaloneExpr(ICSharpExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (expression.Parent is IExpressionInitializer ||
                expression.Parent is IMemberInitializer ||
                expression.Parent is IAnonymousMemberDeclaration)
            {
                return false;
            }

            if (expression.Parent is IAssignmentExpression &&
               (expression.Parent as IAssignmentExpression).Dest is IReferenceExpression)
            {
                return false;
            }

            return true;
        }

        public virtual bool IsStandaloneObjectCreationExpression([NotNull] IObjectCreationExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (expression.Parent is IExpressionInitializer ||
                expression.Parent is IAssignmentExpression ||
                expression.Parent is IMemberInitializer ||
                expression.Parent is IAnonymousMemberDeclaration)
            {
                return false;
            }

            return true;
        }

        public virtual IReferenceExpression GetParentReference([NotNull] IReferenceExpression referenceExpression)
        {
            if (referenceExpression == null) throw new ArgumentNullException("referenceExpression");
            return referenceExpression.QualifierExpression as IReferenceExpression;
        }

        public virtual IReferenceExpression GetParentReference([NotNull] IInvocationExpression invocationExpression)
        {
            if (invocationExpression == null) throw new ArgumentNullException("invocationExpression");
            if (invocationExpression.ExtensionQualifier == null)
            {
                return null;
            }

            var mc = invocationExpression.ExtensionQualifier.ManagedConvertible;
            if (mc is ExtensionArgumentInfo)
            {
                return (mc as ExtensionArgumentInfo).Expression as IReferenceExpression;
            }

            return null;
        }

        public virtual bool IsMoqFakeOptionParameter([NotNull] ILambdaParameterDeclaration variableDeclaration)
        {
            if (variableDeclaration == null) throw new ArgumentNullException("variableDeclaration");

            var lambdaExpression = variableDeclaration.Parent.Parent;

            if (lambdaExpression.Parent is ICSharpArgument)
            {
                var arg = lambdaExpression.Parent as ICSharpArgument;
                if (arg.Parent is IArgumentList)
                {
                    var argList = arg.Parent as IArgumentList;

                    if (argList.Parent is IInvocationExpression)
                    {
                        var invocation = argList.Parent as IInvocationExpression;

                        var invokedName = GetInvokedElementName(invocation);

                        return invokedName.StartsWith("Method:Moq");
                    }
                }
            }

            return false;
        }
    }

    public class NullCsharpDeclaration : ICSharpDeclaration
    {
        public IPsiServices GetPsiServices()
        {
            throw new NotImplementedException();
        }

        public IPsiModule GetPsiModule()
        {
            throw new NotImplementedException();
        }

        public IPsiSourceFile GetSourceFile()
        {
            throw new NotImplementedException();
        }

        public ReferenceCollection GetFirstClassReferences()
        {
            throw new NotImplementedException();
        }

        public void ProcessDescendantsForResolve(IRecursiveElementProcessor processor)
        {
            throw new NotImplementedException();
        }

        public T GetContainingNode<T>(bool returnThis = false) where T : ITreeNode
        {
            throw new NotImplementedException();
        }

        public bool Contains(ITreeNode other)
        {
            throw new NotImplementedException();
        }

        public bool IsPhysical()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public bool IsStub()
        {
            throw new NotImplementedException();
        }

        public bool IsFiltered()
        {
            throw new NotImplementedException();
        }

        public DocumentRange GetNavigationRange()
        {
            throw new NotImplementedException();
        }

        public TreeOffset GetTreeStartOffset()
        {
            throw new NotImplementedException();
        }

        public int GetTextLength()
        {
            throw new NotImplementedException();
        }

        public StringBuilder GetText(StringBuilder to)
        {
            throw new NotImplementedException();
        }

        public IBuffer GetTextAsBuffer()
        {
            throw new NotImplementedException();
        }

        public string GetText()
        {
            throw new NotImplementedException();
        }

        public ITreeNode FindNodeAt(TreeTextRange treeTextRange)
        {
            throw new NotImplementedException();
        }

        public ICollection<ITreeNode> FindNodesAt(TreeOffset treeTextOffset)
        {
            throw new NotImplementedException();
        }

        public ITreeNode FindTokenAt(TreeOffset treeTextOffset)
        {
            throw new NotImplementedException();
        }

        public ITreeNode Parent { get; private set; }
        public ITreeNode FirstChild { get; private set; }
        public ITreeNode LastChild { get; private set; }
        public ITreeNode NextSibling { get; private set; }
        public ITreeNode PrevSibling { get; private set; }
        public NodeType NodeType { get; private set; }
        public PsiLanguageType Language { get; private set; }
        public NodeUserData UserData { get; private set; }
        public NodeUserData PersistentUserData { get; private set; }
        public XmlNode GetXMLDoc(bool inherit)
        {
            throw new NotImplementedException();
        }

        public void SetName(string name)
        {
            throw new NotImplementedException();
        }

        public TreeTextRange GetNameRange()
        {
            throw new NotImplementedException();
        }

        public bool IsSynthetic()
        {
            throw new NotImplementedException();
        }

        public IDeclaredElement DeclaredElement { get; private set; }
        public string DeclaredName { get; private set; }
        public void Accept(TreeNodeVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context)
        {
            throw new NotImplementedException();
        }

        public TReturn Accept<TContext, TReturn>(TreeNodeVisitor<TContext, TReturn> visitor, TContext context)
        {
            throw new NotImplementedException();
        }

        public ICSharpNamespaceDeclaration GetContainingNamespaceDeclaration()
        {
            throw new NotImplementedException();
        }

        public ICSharpTypeMemberDeclaration GetContainingTypeMemberDeclaration()
        {
            throw new NotImplementedException();
        }

        public ICSharpTypeDeclaration GetContainingTypeDeclaration()
        {
            throw new NotImplementedException();
        }

        public ICSharpIdentifier NameIdentifier { get; private set; }
    }

    public class NullTypeElement : ITypeElement
    {
        public IPsiServices GetPsiServices()
        {
            throw new NotImplementedException();
        }

        public IList<IDeclaration> GetDeclarations()
        {
            throw new NotImplementedException();
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            throw new NotImplementedException();
        }

        public DeclaredElementType GetElementType()
        {
            throw new NotImplementedException();
        }

        public XmlNode GetXMLDoc(bool inherit)
        {
            throw new NotImplementedException();
        }

        public XmlNode GetXMLDescriptionSummary(bool inherit)
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public bool IsSynthetic()
        {
            throw new NotImplementedException();
        }

        public HybridCollection<IPsiSourceFile> GetSourceFiles()
        {
            throw new NotImplementedException();
        }

        public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
        {
            throw new NotImplementedException();
        }

        public string ShortName { get; private set; }
        public bool CaseSensistiveName { get; private set; }
        public PsiLanguageType PresentationLanguage { get; private set; }
        public ITypeElement GetContainingType()
        {
            throw new NotImplementedException();
        }

        public ITypeMember GetContainingTypeMember()
        {
            throw new NotImplementedException();
        }

        public IPsiModule Module { get; private set; }
        public ISubstitution IdSubstitution { get; private set; }
        public IModuleReferenceResolveContext ResolveContext { get; private set; }
        public IList<ITypeParameter> TypeParameters { get; private set; }
        public IList<IAttributeInstance> GetAttributeInstances(bool inherit)
        {
            throw new NotImplementedException();
        }

        public IList<IAttributeInstance> GetAttributeInstances(IClrTypeName clrName, bool inherit)
        {
            throw new NotImplementedException();
        }

        public bool HasAttributeInstance(IClrTypeName clrName, bool inherit)
        {
            throw new NotImplementedException();
        }

        public IClrTypeName GetClrName()
        {
            throw new NotImplementedException();
        }

        public IList<IDeclaredType> GetSuperTypes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITypeMember> GetMembers()
        {
            throw new NotImplementedException();
        }

        public INamespace GetContainingNamespace()
        {
            throw new NotImplementedException();
        }

        public IPsiSourceFile GetSingleOrDefaultSourceFile()
        {
            throw new NotImplementedException();
        }

        public IList<ITypeElement> NestedTypes { get; private set; }
        public IEnumerable<IConstructor> Constructors { get; private set; }
        public IEnumerable<IOperator> Operators { get; private set; }
        public IEnumerable<IMethod> Methods { get; private set; }
        public IEnumerable<IProperty> Properties { get; private set; }
        public IEnumerable<IEvent> Events { get; private set; }
        public IEnumerable<string> MemberNames { get; private set; }
    }
}
