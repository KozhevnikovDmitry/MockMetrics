using System;
using HaveBox;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating
{
    public class EatingRoot
    {
        #region Singleton

        private static readonly Lazy<EatingRoot> _instance = new Lazy<EatingRoot>(() => new EatingRoot());

        public static EatingRoot Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private readonly IContainer _container;
        
        #endregion

        private EatingRoot()
        {
            _container = new Container();
            _container.Configure(config => config.MergeConfig(new EatingConfig(GetType().Assembly, _container)));
            
            #region Expressions to eat
            
            // - already implemented
            //*ICSharpLiteralExpression
            //*IObjectCreationExpression

            // - to work with, eat call reference
            //*IInvocationExpression

            //- all binary expressions can be eat the same way- eat operand, return stub
            //*IBinaryExpression
            //*IConditionalAndExpression
            //*IConditionalOrExpression
            //*IBitwiseAndExpression
            //*IBitwiseExclusiveOrExpression
            //*IBitwiseInclusiveOrExpression
            //*IEqualityExpression
            //*IMultiplicativeExpression
            //*INullCoalescingExpression
            //*IRelationalExpression
            //*IShiftExpression
            //*IAdditiveExpression

            // - nothing to eat, return none
            //*IThisExpression
            //*IBaseExpression
            //*IPredefinedTypeExpression
            //*IAnonymousFunctionExpression

            // - simply eat containing expressions and return StubCandidate
            //*IIsExpression
            //*ITypeofExpression
            //*IConditionalTernaryExpression
            //IArrayCreationExpression
            //IAnonymousMethodExpression
            //IAnonymousObjectCreationExpression

            // - simply eat containing expression
            //*IPostfixOperatorExpression
            //*IPrefixOperatorExpression
            //*IAwaitExpression
            //*IUnaryOperatorExpression
            //*IParenthesizedExpression
            
            // - simply eat containing expressions and return none
            //IAssignmentExpression (case: int i; i = 0;)

            // - depends of return type, may be stub, target, mock(after that simple eating)
            //IDefaultExpression
            //ILambdaExpression
            //IReferenceExpression
            //IElementAccessExpression
            //IAsExpression
            //ICastExpression

            
            // - commom expression interfaces, never eat
            //*IUnaryExpression
            //*IPrimaryExpression
            //*IOperatorExpression
            //*IPrimaryExpression
            //*ICreationExpression

            // - no idea yet
            //IQueryExpression

            // - really rare using expressions
            //I__ArglistExpression
            //ICheckedExpression
            //IUncheckedExpression
            //IUnsafeCodePointerAccessExpression
            //IUnsafeCodePointerIndirectionExpression
            //IUnsafeCodeSizeOfExpression
            //IUnsafeCodeAddressOfExpression
            
            #endregion

            #region Statements to eat

            //IDeclarationStatement
            //*IExpressionStatement
            //*IBlock
            //*IIfStatement
            //*IForStatement
            //*IForeachStatement
            //IUsingStatement
            //*IWhileStatement
            //*IDoStatement
            //ITryStatement
            //*ISwitchStatement
            //*ISwitchLabelStatement
            //*IThrowStatement
            //*ILockStatement
            //*IReturnStatement
            //*IYieldStatement
            //*IGotoStatement
            //*IGotoCaseStatement
            //*ILabelStatement
            //*IEmptyStatement
            //*IUncheckedStatement
            //*IUnsafeCodeFixedStatement
            //*IUnsafeCodeUnsafeStatement

            #endregion

            #region VariableDeclarations to eat
            
            //*IAnonymousMethodParameterDeclaration
            //*ICatchVariableDeclaration
            //*IForeachVariableDeclaration
            //*ILambdaParameterDeclaration
            //*ILocalConstantDeclaration
            //*ILocalVariableDeclaration
            //*IRegularParameterDeclaration
            //*IUnsafeCodeFixedPointerDeclaration

            // THINKING ABOUT IT. 
            //IMultipleDeclarationMember ? May be passed through in DeclarationStatementEater
            //ArglistParameterDeclaration ? Really rare to use

            // DO NOT EAT
            //IConstantDeclaration ? (not occurs id methods)
            //IEventDeclaration ? (not occurs id methods)
            //IFieldDeclaration ? (not occurs id methods)
            //IPropertyDeclaration ? (not occurs id methods)

            #endregion
        }
        
        public UnitTestEater GetUnitTestEater()
        {
            return _container.GetInstance<UnitTestEater>();
        }

        //private IStatementEater<T> GetStatementEater<T>() where T : ICSharpStatement
        //{
        //    return Container.GetInstance<IStatementEater<T>>();
        //}

        //private IExpressionEater<T> GetExpressionEater<T>() where T : ICSharpExpression
        //{
        //    return Container.GetInstance<IExpressionEater<T>>();
        //}

        //private ExpressionKind EatExpression(Snapshot snapshot, IMethodDeclaration test,
        //    ICSharpExpression expression)
        //{
        //    #region AnonymousFunction

        //    if (expression is ILambdaExpression)
        //    {
        //        return GetExpressionEater<ILambdaExpression>().Eat(snapshot, test, expression as ILambdaExpression);
        //    }

        //    if (expression is IAnonymousMethodExpression)
        //    {
        //        return GetExpressionEater<IAnonymousMethodExpression>()
        //            .Eat(snapshot, test, expression as IAnonymousMethodExpression);
        //    }

        //    if (expression is IAnonymousFunctionExpression)
        //    {
        //        return GetExpressionEater<IAnonymousFunctionExpression>()
        //            .Eat(snapshot, test, expression as IAnonymousFunctionExpression);
        //    }

        //    #endregion

        //    #region Object Creation

        //    if (expression is IAnonymousObjectCreationExpression)
        //    {
        //        return GetExpressionEater<IAnonymousObjectCreationExpression>()
        //            .Eat(snapshot, test, expression as IAnonymousObjectCreationExpression);
        //    }

        //    if (expression is IArrayCreationExpression)
        //    {
        //        return GetExpressionEater<IArrayCreationExpression>()
        //            .Eat(snapshot, test, expression as IArrayCreationExpression);
        //    }

        //    if (expression is IObjectCreationExpression)
        //    {
        //        return GetExpressionEater<IObjectCreationExpression>()
        //            .Eat(snapshot, test, expression as IObjectCreationExpression);
        //    }

        //    if (expression is ICreationExpression)
        //    {
        //        return GetExpressionEater<ICreationExpression>().Eat(snapshot, test, expression as ICreationExpression);
        //    }

        //    #endregion

        //    #region Binary

        //    if (expression is IBitwiseAndExpression)
        //    {
        //        return GetExpressionEater<IBitwiseAndExpression>()
        //            .Eat(snapshot, test, expression as IBitwiseAndExpression);
        //    }

        //    if (expression is IBitwiseExclusiveOrExpression)
        //    {
        //        return GetExpressionEater<IBitwiseExclusiveOrExpression>()
        //            .Eat(snapshot, test, expression as IBitwiseExclusiveOrExpression);
        //    }

        //    if (expression is IBitwiseInclusiveOrExpression)
        //    {
        //        return GetExpressionEater<IBitwiseInclusiveOrExpression>()
        //            .Eat(snapshot, test, expression as IBitwiseInclusiveOrExpression);
        //    }

        //    if (expression is IConditionalAndExpression)
        //    {
        //        return GetExpressionEater<IConditionalAndExpression>()
        //            .Eat(snapshot, test, expression as IConditionalAndExpression);
        //    }

        //    if (expression is IConditionalOrExpression)
        //    {
        //        return GetExpressionEater<ILambdaExpression>().Eat(snapshot, test, expression as ILambdaExpression);
        //    }

        //    if (expression is IAdditiveExpression)
        //    {
        //        return GetExpressionEater<IAdditiveExpression>().Eat(snapshot, test, expression as IAdditiveExpression);
        //    }

        //    if (expression is IEqualityExpression)
        //    {
        //        return GetExpressionEater<IEqualityExpression>().Eat(snapshot, test, expression as IEqualityExpression);
        //    }

        //    if (expression is IMultiplicativeExpression)
        //    {
        //        return GetExpressionEater<IMultiplicativeExpression>()
        //            .Eat(snapshot, test, expression as IMultiplicativeExpression);
        //    }

        //    if (expression is INullCoalescingExpression)
        //    {
        //        return GetExpressionEater<INullCoalescingExpression>()
        //            .Eat(snapshot, test, expression as INullCoalescingExpression);
        //    }

        //    if (expression is IRelationalExpression)
        //    {
        //        return GetExpressionEater<IRelationalExpression>()
        //            .Eat(snapshot, test, expression as IRelationalExpression);
        //    }

        //    if (expression is IShiftExpression)
        //    {
        //        return GetExpressionEater<IShiftExpression>().Eat(snapshot, test, expression as IShiftExpression);
        //    }

        //    if (expression is IBinaryExpression)
        //    {
        //        return GetExpressionEater<IBinaryExpression>().Eat(snapshot, test, expression as IBinaryExpression);
        //    }

        //    #endregion

        //    #region Operator

        //    if (expression is IAssignmentExpression)
        //    {
        //        return GetExpressionEater<IAssignmentExpression>()
        //            .Eat(snapshot, test, expression as IAssignmentExpression);
        //    }

        //    if (expression is IPostfixOperatorExpression)
        //    {
        //        return GetExpressionEater<IPostfixOperatorExpression>()
        //            .Eat(snapshot, test, expression as IPostfixOperatorExpression);
        //    }

        //    if (expression is IPrefixOperatorExpression)
        //    {
        //        return GetExpressionEater<IPrefixOperatorExpression>()
        //            .Eat(snapshot, test, expression as IPrefixOperatorExpression);
        //    }

        //    if (expression is IUnaryOperatorExpression)
        //    {
        //        return GetExpressionEater<IUnaryOperatorExpression>()
        //            .Eat(snapshot, test, expression as IUnaryOperatorExpression);
        //    }

        //    if (expression is IOperatorExpression)
        //    {
        //        return GetExpressionEater<IOperatorExpression>().Eat(snapshot, test, expression as IOperatorExpression);
        //    }

        //    #endregion

        //    #region Primary

        //    if (expression is IBaseExpression)
        //    {
        //        return GetExpressionEater<IBaseExpression>().Eat(snapshot, test, expression as IBaseExpression);
        //    }

        //    if (expression is IUnsafeCodePointerAccessExpression)
        //    {
        //        return GetExpressionEater<IUnsafeCodePointerAccessExpression>()
        //            .Eat(snapshot, test, expression as IUnsafeCodePointerAccessExpression);
        //    }

        //    if (expression is IUnsafeCodePointerIndirectionExpression)
        //    {
        //        return GetExpressionEater<IUnsafeCodePointerIndirectionExpression>()
        //            .Eat(snapshot, test, expression as IUnsafeCodePointerIndirectionExpression);
        //    }

        //    if (expression is IUnsafeCodeSizeOfExpression)
        //    {
        //        return GetExpressionEater<IUnsafeCodeSizeOfExpression>()
        //            .Eat(snapshot, test, expression as IUnsafeCodeSizeOfExpression);
        //    }

        //    if (expression is I__ArglistExpression)
        //    {
        //        return GetExpressionEater<I__ArglistExpression>()
        //            .Eat(snapshot, test, expression as I__ArglistExpression);
        //    }

        //    if (expression is IParenthesizedExpression)
        //    {
        //        return GetExpressionEater<IParenthesizedExpression>()
        //            .Eat(snapshot, test, expression as IParenthesizedExpression);
        //    }

        //    if (expression is IPredefinedTypeExpression)
        //    {
        //        return GetExpressionEater<IPredefinedTypeExpression>()
        //            .Eat(snapshot, test, expression as IPredefinedTypeExpression);
        //    }

        //    if (expression is ICheckedExpression)
        //    {
        //        return GetExpressionEater<ICheckedExpression>().Eat(snapshot, test, expression as ICheckedExpression);
        //    }

        //    if (expression is IUncheckedExpression)
        //    {
        //        return GetExpressionEater<IUncheckedExpression>()
        //            .Eat(snapshot, test, expression as IUncheckedExpression);
        //    }

        //    if (expression is IReferenceExpression)
        //    {
        //        return GetExpressionEater<IReferenceExpression>()
        //            .Eat(snapshot, test, expression as IReferenceExpression);
        //    }

        //    if (expression is IThisExpression)
        //    {
        //        return GetExpressionEater<IThisExpression>().Eat(snapshot, test, expression as IThisExpression);
        //    }

        //    if (expression is ITypeofExpression)
        //    {
        //        return GetExpressionEater<ITypeofExpression>().Eat(snapshot, test, expression as ITypeofExpression);
        //    }

        //    if (expression is IElementAccessExpression)
        //    {
        //        return GetExpressionEater<IElementAccessExpression>()
        //            .Eat(snapshot, test, expression as IElementAccessExpression);
        //    }

        //    if (expression is IInvocationExpression)
        //    {
        //        return GetExpressionEater<IInvocationExpression>()
        //            .Eat(snapshot, test, expression as IInvocationExpression);
        //    }

        //    if (expression is ICSharpLiteralExpression)
        //    {
        //        return GetExpressionEater<ICSharpLiteralExpression>()
        //            .Eat(snapshot, test, expression as ICSharpLiteralExpression);
        //    }

        //    if (expression is IDefaultExpression)
        //    {
        //        return GetExpressionEater<IDefaultExpression>().Eat(snapshot, test, expression as IDefaultExpression);
        //    }

        //    if (expression is IPrimaryExpression)
        //    {
        //        return GetExpressionEater<IPrimaryExpression>().Eat(snapshot, test, expression as IPrimaryExpression);
        //    }

        //    #endregion

        //    #region Unary

        //    if (expression is IUnsafeCodeAddressOfExpression)
        //    {
        //        return GetExpressionEater<IUnsafeCodeAddressOfExpression>()
        //            .Eat(snapshot, test, expression as IUnsafeCodeAddressOfExpression);
        //    }

        //    if (expression is IAwaitExpression)
        //    {
        //        return GetExpressionEater<IAwaitExpression>().Eat(snapshot, test, expression as IAwaitExpression);
        //    }

        //    if (expression is ICastExpression)
        //    {
        //        return GetExpressionEater<ICastExpression>().Eat(snapshot, test, expression as ICastExpression);
        //    }

        //    if (expression is IUnaryExpression)
        //    {
        //        return GetExpressionEater<IUnaryExpression>().Eat(snapshot, test, expression as IUnaryExpression);
        //    }

        //    #endregion

        //    #region CSharpExpression

        //    if (expression is IIsExpression)
        //    {
        //        return GetExpressionEater<IIsExpression>().Eat(snapshot, test, expression as IIsExpression);
        //    }

        //    if (expression is IAsExpression)
        //    {
        //        return GetExpressionEater<IAsExpression>().Eat(snapshot, test, expression as IAsExpression);
        //    }

        //    if (expression is IConditionalTernaryExpression)
        //    {
        //        return GetExpressionEater<IConditionalTernaryExpression>()
        //            .Eat(snapshot, test, expression as IConditionalTernaryExpression);
        //    }

        //    if (expression is IQueryExpression)
        //    {
        //        return GetExpressionEater<IQueryExpression>().Eat(snapshot, test, expression as IQueryExpression);
        //    }

        //    #endregion

        //    throw new NotSupportedException();
        //}

        //private void EatStatement(Snapshot snapshot, IMethodDeclaration test, ICSharpStatement statement)
        //{
        //    if (statement is IDeclarationStatement)
        //    {
        //        GetStatementEater<IDeclarationStatement>().Eat(snapshot, test, statement as IDeclarationStatement);
        //        return;
        //    }

        //    if (statement is IExpressionStatement)
        //    {
        //        GetStatementEater<IExpressionStatement>().Eat(snapshot, test, statement as IExpressionStatement);
        //        return;
        //    }

        //    if (statement is IBlock)
        //    {
        //        GetStatementEater<IBlock>().Eat(snapshot, test, statement as IBlock);
        //        return;
        //    }

        //    if (statement is IIfStatement)
        //    {
        //        GetStatementEater<IIfStatement>().Eat(snapshot, test, statement as IIfStatement);
        //        return;
        //    }

        //    if (statement is IForStatement)
        //    {
        //        GetStatementEater<IForStatement>().Eat(snapshot, test, statement as IForStatement);
        //        return;
        //    }

        //    if (statement is IForeachStatement)
        //    {
        //        GetStatementEater<IForeachStatement>().Eat(snapshot, test, statement as IForeachStatement);
        //        return;
        //    }

        //    if (statement is IUsingStatement)
        //    {
        //        GetStatementEater<IUsingStatement>().Eat(snapshot, test, statement as IUsingStatement);
        //        return;
        //    }

        //    if (statement is IWhileStatement)
        //    {
        //        GetStatementEater<IWhileStatement>().Eat(snapshot, test, statement as IWhileStatement);
        //        return;
        //    }

        //    if (statement is IDoStatement)
        //    {
        //        GetStatementEater<IDoStatement>().Eat(snapshot, test, statement as IDoStatement);
        //        return;
        //    }

        //    if (statement is ITryStatement)
        //    {
        //        GetStatementEater<ITryStatement>().Eat(snapshot, test, statement as ITryStatement);
        //        return;
        //    }

        //    if (statement is ISwitchStatement)
        //    {
        //        GetStatementEater<ISwitchStatement>().Eat(snapshot, test, statement as ISwitchStatement);
        //        return;
        //    }

        //    if (statement is ISwitchLabelStatement)
        //    {
        //        GetStatementEater<ISwitchLabelStatement>().Eat(snapshot, test, statement as ISwitchLabelStatement);
        //        return;
        //    }

        //    if (statement is IThrowStatement)
        //    {
        //        GetStatementEater<IThrowStatement>().Eat(snapshot, test, statement as IThrowStatement);
        //        return;
        //    }

        //    if (statement is ILockStatement)
        //    {
        //        GetStatementEater<ILockStatement>().Eat(snapshot, test, statement as ILockStatement);
        //        return;
        //    }

        //    if (statement is IReturnStatement)
        //    {
        //        GetStatementEater<IReturnStatement>().Eat(snapshot, test, statement as IReturnStatement);
        //        return;
        //    }

        //    if (statement is IYieldStatement)
        //    {
        //        GetStatementEater<IYieldStatement>().Eat(snapshot, test, statement as IYieldStatement);
        //        return;
        //    }

        //    if (statement is IGotoStatement)
        //    {
        //        GetStatementEater<IGotoStatement>().Eat(snapshot, test, statement as IGotoStatement);
        //        return;
        //    }

        //    if (statement is IGotoCaseStatement)
        //    {
        //        GetStatementEater<IGotoCaseStatement>().Eat(snapshot, test, statement as IGotoCaseStatement);
        //        return;
        //    }

        //    if (statement is ILabelStatement)
        //    {
        //        GetStatementEater<ILabelStatement>().Eat(snapshot, test, statement as ILabelStatement);
        //        return;
        //    }
        //    if (statement is IEmptyStatement)
        //    {
        //        GetStatementEater<IEmptyStatement>().Eat(snapshot, test, statement as IEmptyStatement);
        //        return;
        //    }

        //    if (statement is IUncheckedStatement)
        //    {
        //        GetStatementEater<IUncheckedStatement>().Eat(snapshot, test, statement as IUncheckedStatement);
        //        return;
        //    }

        //    if (statement is IUnsafeCodeFixedStatement)
        //    {
        //        GetStatementEater<IUnsafeCodeFixedStatement>()
        //            .Eat(snapshot, test, statement as IUnsafeCodeFixedStatement);
        //        return;
        //    }

        //    if (statement is IUnsafeCodeUnsafeStatement)
        //    {
        //        GetStatementEater<IUnsafeCodeUnsafeStatement>()
        //            .Eat(snapshot, test, statement as IUnsafeCodeUnsafeStatement);
        //        return;
        //    }

        //    throw new NotSupportedException();
        //}
    }
}