using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eat
{
    public class ExpessionEat
    {
        public ExpessionEat()
        {

        }

        private IExpressionEater<T> GetEater<T>() where T : ICSharpExpression
        {
            throw new NotImplementedException();
        }

        public Snapshot Eat(Snapshot snapshot, IMethodDeclaration test, ICSharpExpression expression)
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

            throw new NotImplementedException();
        }
    }
}

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
//IShiftExpressio
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
