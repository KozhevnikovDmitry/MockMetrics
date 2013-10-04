using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eat.Statement;

namespace MockMetrics.Eat
{
    public static class Eater
    {
        private static Dictionary<Type, object> _eaters; 

        static Eater()
        {
            _eaters = new Dictionary<Type, object>();
            _eaters[typeof (IBlock)] = new BlockEater();

            //ILambdaExpression
            //IAnonymousMethodExpression
            //IAnonymousFunctionExpression
            //IAnonymousObjectCreationExpression
            //IArrayCreationExpression
            //IObjectCreationExpression
            //ICreationExpression
            //IBitwiseAndExpression
            //IBitwiseExclusiveOrExpression
            //IBitwiseInclusiveOrExpression
            //IConditionalAndExpression
            //IConditionalOrExpression
            //IAdditiveExpression
            //IEqualityExpression
            //IMultiplicativeExpression
            //INullCoalescingExpression
            //IRelationalExpression
            //IShiftExpression
            //IBinaryExpression
            //IAssignmentExpression
            //IPostfixOperatorExpression
            //IPrefixOperatorExpression
            //IUnaryOperatorExpression
            //IOperatorExpression
            //IBaseExpression
            //IUnsafeCodePointerAccessExpression
            //IUnsafeCodePointerIndirectionExpression
            //IUnsafeCodeSizeOfExpression
            //I__ArglistExpression
            //IParenthesizedExpression
            //IPredefinedTypeExpression
            //ICheckedExpression
            //IUncheckedExpression
            //IReferenceExpression
            //IThisExpression
            //ITypeofExpression
            //IElementAccessExpression
            //IInvocationExpression
            //ICSharpLiteralExpression
            //IDefaultExpression
            //IPrimaryExpression
            //IUnsafeCodeAddressOfExpression
            //IAwaitExpression
            //ICastExpression
            //IUnaryExpression
            //IIsExpression
            //IAsExpression
            //IConditionalTernaryExpression
            //IQueryExpression

            //IDeclarationStatement
            //IExpressionStatement
            //IBlock
            //IIfStatement
            //IForStatement
            //IForeachStatement
            //IUsingStatement
            //IWhileStatement
            //IDoStatement
            //ITryStatement
            //ISwitchStatement
            //ISwitchLabelStatement
            //IThrowStatement
            //ILockStatement
            //IReturnStatement
            //IYieldStatement
            //IGotoStatement
            //IGotoCaseStatement
            //ILabelStatement
            //IEmptyStatement
            //IUncheckedStatement
            //IUnsafeCodeFixedStatement
            //IUnsafeCodeUnsafeStatement
        }

        private static IEater<T> GetEater<T>() where T : ICSharpTreeNode
        {
            return _eaters[typeof(T)] as IEater<T>;
        }

        public static Snapshot Eat(Snapshot snapshot, IMethodDeclaration test, ICSharpTreeNode treeNode)
        {
            if (treeNode is ICSharpExpression)
            {
                return EatExpression(snapshot, test, treeNode as ICSharpExpression);
            }

            if (treeNode is ICSharpStatement)
            {
                return EatStatement(snapshot, test, treeNode as ICSharpStatement);
            }

            throw new NotSupportedException();
        }

        private static Snapshot EatExpression(Snapshot snapshot, IMethodDeclaration test, ICSharpExpression expression)
        {
            #region AnonymousFunction

            if (expression is ILambdaExpression)
            {
                return GetEater<ILambdaExpression>().Eat(snapshot, test, expression as ILambdaExpression);
            }

            if (expression is IAnonymousMethodExpression)
            {
                return GetEater<IAnonymousMethodExpression>().Eat(snapshot, test, expression as IAnonymousMethodExpression);
            }

            if (expression is IAnonymousFunctionExpression)
            {
                return GetEater<IAnonymousFunctionExpression>().Eat(snapshot, test, expression as IAnonymousFunctionExpression);
            }

            #endregion


            #region Object Creation

            if (expression is IAnonymousObjectCreationExpression)
            {
                return GetEater<IAnonymousObjectCreationExpression>().Eat(snapshot, test, expression as IAnonymousObjectCreationExpression);
            }

            if (expression is IArrayCreationExpression)
            {
                return GetEater<IArrayCreationExpression>().Eat(snapshot, test, expression as IArrayCreationExpression);
            }

            if (expression is IObjectCreationExpression)
            {
                return GetEater<IObjectCreationExpression>().Eat(snapshot, test, expression as IObjectCreationExpression);
            }

            if (expression is ICreationExpression)
            {
                return GetEater<ICreationExpression>().Eat(snapshot, test, expression as ICreationExpression);
            }

            #endregion


            #region Binary

            if (expression is IBitwiseAndExpression)
            {
                return GetEater<IBitwiseAndExpression>().Eat(snapshot, test, expression as IBitwiseAndExpression);
            }

            if (expression is IBitwiseExclusiveOrExpression)
            {
                return GetEater<IBitwiseExclusiveOrExpression>().Eat(snapshot, test, expression as IBitwiseExclusiveOrExpression);
            }

            if (expression is IBitwiseInclusiveOrExpression)
            {
                return GetEater<IBitwiseInclusiveOrExpression>().Eat(snapshot, test, expression as IBitwiseInclusiveOrExpression);
            }

            if (expression is IConditionalAndExpression)
            {
                return GetEater<IConditionalAndExpression>().Eat(snapshot, test, expression as IConditionalAndExpression);
            }

            if (expression is IConditionalOrExpression)
            {
                return GetEater<ILambdaExpression>().Eat(snapshot, test, expression as ILambdaExpression);
            }

            if (expression is IAdditiveExpression)
            {
                return GetEater<IAdditiveExpression>().Eat(snapshot, test, expression as IAdditiveExpression);
            }

            if (expression is IEqualityExpression)
            {
                return GetEater<IEqualityExpression>().Eat(snapshot, test, expression as IEqualityExpression);
            }

            if (expression is IMultiplicativeExpression)
            {
                return GetEater<IMultiplicativeExpression>().Eat(snapshot, test, expression as IMultiplicativeExpression);
            }

            if (expression is INullCoalescingExpression)
            {
                return GetEater<INullCoalescingExpression>().Eat(snapshot, test, expression as INullCoalescingExpression);
            }

            if (expression is IRelationalExpression)
            {
                return GetEater<IRelationalExpression>().Eat(snapshot, test, expression as IRelationalExpression);
            }

            if (expression is IShiftExpression)
            {
                return GetEater<IShiftExpression>().Eat(snapshot, test, expression as IShiftExpression);
            }

            if (expression is IBinaryExpression)
            {
                return GetEater<IBinaryExpression>().Eat(snapshot, test, expression as IBinaryExpression);
            }

            #endregion


            #region Operator

            if (expression is IAssignmentExpression)
            {
                return GetEater<IAssignmentExpression>().Eat(snapshot, test, expression as IAssignmentExpression);
            }

            if (expression is IPostfixOperatorExpression)
            {
                return GetEater<IPostfixOperatorExpression>().Eat(snapshot, test, expression as IPostfixOperatorExpression);
            }

            if (expression is IPrefixOperatorExpression)
            {
                return GetEater<IPrefixOperatorExpression>().Eat(snapshot, test, expression as IPrefixOperatorExpression);
            }

            if (expression is IUnaryOperatorExpression)
            {
                return GetEater<IUnaryOperatorExpression>().Eat(snapshot, test, expression as IUnaryOperatorExpression);
            }

            if (expression is IOperatorExpression)
            {
                return GetEater<IOperatorExpression>().Eat(snapshot, test, expression as IOperatorExpression);
            }

            #endregion


            #region Primary

            if (expression is IBaseExpression)
            {
                return GetEater<IBaseExpression>().Eat(snapshot, test, expression as IBaseExpression);
            }

            if (expression is IUnsafeCodePointerAccessExpression)
            {
                return GetEater<IUnsafeCodePointerAccessExpression>().Eat(snapshot, test, expression as IUnsafeCodePointerAccessExpression);
            }

            if (expression is IUnsafeCodePointerIndirectionExpression)
            {
                return GetEater<IUnsafeCodePointerIndirectionExpression>().Eat(snapshot, test, expression as IUnsafeCodePointerIndirectionExpression);
            }

            if (expression is IUnsafeCodeSizeOfExpression)
            {
                return GetEater<IUnsafeCodeSizeOfExpression>().Eat(snapshot, test, expression as IUnsafeCodeSizeOfExpression);
            }

            if (expression is I__ArglistExpression)
            {
                return GetEater<I__ArglistExpression>().Eat(snapshot, test, expression as I__ArglistExpression);
            }

            if (expression is IParenthesizedExpression)
            {
                return GetEater<IParenthesizedExpression>().Eat(snapshot, test, expression as IParenthesizedExpression);
            }

            if (expression is IPredefinedTypeExpression)
            {
                return GetEater<IPredefinedTypeExpression>().Eat(snapshot, test, expression as IPredefinedTypeExpression);
            }

            if (expression is ICheckedExpression)
            {
                return GetEater<ICheckedExpression>().Eat(snapshot, test, expression as ICheckedExpression);
            }

            if (expression is IUncheckedExpression)
            {
                return GetEater<IUncheckedExpression>().Eat(snapshot, test, expression as IUncheckedExpression);
            }

            if (expression is IReferenceExpression)
            {
                return GetEater<IReferenceExpression>().Eat(snapshot, test, expression as IReferenceExpression);
            }

            if (expression is IThisExpression)
            {
                return GetEater<IThisExpression>().Eat(snapshot, test, expression as IThisExpression);
            }

            if (expression is ITypeofExpression)
            {
                return GetEater<ITypeofExpression>().Eat(snapshot, test, expression as ITypeofExpression);
            }

            if (expression is IElementAccessExpression)
            {
                return GetEater<IElementAccessExpression>().Eat(snapshot, test, expression as IElementAccessExpression);
            }

            if (expression is IInvocationExpression)
            {
                return GetEater<IInvocationExpression>().Eat(snapshot, test, expression as IInvocationExpression);
            }

            if (expression is ICSharpLiteralExpression)
            {
                return GetEater<ICSharpLiteralExpression>().Eat(snapshot, test, expression as ICSharpLiteralExpression);
            }

            if (expression is IDefaultExpression)
            {
                return GetEater<IDefaultExpression>().Eat(snapshot, test, expression as IDefaultExpression);
            }

            if (expression is IPrimaryExpression)
            {
                return GetEater<IPrimaryExpression>().Eat(snapshot, test, expression as IPrimaryExpression);
            }

            #endregion


            #region Unary

            if (expression is IUnsafeCodeAddressOfExpression)
            {
                return GetEater<IUnsafeCodeAddressOfExpression>().Eat(snapshot, test, expression as IUnsafeCodeAddressOfExpression);
            }

            if (expression is IAwaitExpression)
            {
                return GetEater<IAwaitExpression>().Eat(snapshot, test, expression as IAwaitExpression);
            }

            if (expression is ICastExpression)
            {
                return GetEater<ICastExpression>().Eat(snapshot, test, expression as ICastExpression);
            }

            if (expression is IUnaryExpression)
            {
                return GetEater<IUnaryExpression>().Eat(snapshot, test, expression as IUnaryExpression);
            }

            #endregion


            #region CSharpExpression

            if (expression is IIsExpression)
            {
                return GetEater<IIsExpression>().Eat(snapshot, test, expression as IIsExpression);
            }

            if (expression is IAsExpression)
            {
                return GetEater<IAsExpression>().Eat(snapshot, test, expression as IAsExpression);
            }

            if (expression is IConditionalTernaryExpression)
            {
                return GetEater<IConditionalTernaryExpression>().Eat(snapshot, test, expression as IConditionalTernaryExpression);
            }

            if (expression is IQueryExpression)
            {
                return GetEater<IQueryExpression>().Eat(snapshot, test, expression as IQueryExpression);
            }

            #endregion

            throw new NotSupportedException();
        }

        private static Snapshot EatStatement(Snapshot snapshot, IMethodDeclaration test, ICSharpStatement statement)
        {
            if (statement is IDeclarationStatement)
            {
                return GetEater<IDeclarationStatement>().Eat(snapshot, test, statement as IDeclarationStatement);
            }

            if (statement is IExpressionStatement)
            {
                return GetEater<IExpressionStatement>().Eat(snapshot, test, statement as IExpressionStatement);
            }

            if (statement is IBlock)
            {
                return GetEater<IBlock>().Eat(snapshot, test, statement as IBlock);
            }

            if (statement is IIfStatement)
            {
                return GetEater<IIfStatement>().Eat(snapshot, test, statement as IIfStatement);
            }

            if (statement is IForStatement)
            {
                return GetEater<IForStatement>().Eat(snapshot, test, statement as IForStatement);
            }

            if (statement is IForeachStatement)
            {
                return GetEater<IForeachStatement>().Eat(snapshot, test, statement as IForeachStatement);
            }

            if (statement is IUsingStatement)
            {
                return GetEater<IUsingStatement>().Eat(snapshot, test, statement as IUsingStatement);
            }

            if (statement is IWhileStatement)
            {
                return GetEater<IWhileStatement>().Eat(snapshot, test, statement as IWhileStatement);
            }

            if (statement is IDoStatement)
            {
                return GetEater<IDoStatement>().Eat(snapshot, test, statement as IDoStatement);
            }

            if (statement is ITryStatement)
            {
                return GetEater<ITryStatement>().Eat(snapshot, test, statement as ITryStatement); ;
            }

            if (statement is ISwitchStatement)
            {
                return GetEater<ISwitchStatement>().Eat(snapshot, test, statement as ISwitchStatement);
            }

            if (statement is ISwitchLabelStatement)
            {
                return GetEater<ISwitchLabelStatement>().Eat(snapshot, test, statement as ISwitchLabelStatement);
            }

            if (statement is IThrowStatement)
            {
                return GetEater<IThrowStatement>().Eat(snapshot, test, statement as IThrowStatement);
            }

            if (statement is ILockStatement)
            {
                return GetEater<ILockStatement>().Eat(snapshot, test, statement as ILockStatement);
            }

            if (statement is IReturnStatement)
            {
                return GetEater<IReturnStatement>().Eat(snapshot, test, statement as IReturnStatement);
            }

            if (statement is IYieldStatement)
            {
                return GetEater<IYieldStatement>().Eat(snapshot, test, statement as IYieldStatement);
            }

            if (statement is IGotoStatement)
            {
                return GetEater<IGotoStatement>().Eat(snapshot, test, statement as IGotoStatement);
            }

            if (statement is IGotoCaseStatement)
            {
                return GetEater<IGotoCaseStatement>().Eat(snapshot, test, statement as IGotoCaseStatement);
            }

            if (statement is ILabelStatement)
            {
                return GetEater<ILabelStatement>().Eat(snapshot, test, statement as ILabelStatement);
            }
            if (statement is IEmptyStatement)
            {
                return GetEater<IEmptyStatement>().Eat(snapshot, test, statement as IEmptyStatement);
            }

            if (statement is IUncheckedStatement)
            {
                return GetEater<IUncheckedStatement>().Eat(snapshot, test, statement as IUncheckedStatement);
            }

            if (statement is IUnsafeCodeFixedStatement)
            {
                return GetEater<IUnsafeCodeFixedStatement>().Eat(snapshot, test, statement as IUnsafeCodeFixedStatement);
            }

            if (statement is IUnsafeCodeUnsafeStatement)
            {
                return GetEater<IUnsafeCodeUnsafeStatement>().Eat(snapshot, test, statement as IUnsafeCodeUnsafeStatement);
            }

            throw new NotSupportedException();
        }
    }
}