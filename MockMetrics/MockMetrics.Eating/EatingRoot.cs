using System;
using HaveBox;

namespace MockMetrics.Eating
{
    public class EatingRoot
    {
        public static EatingRoot Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly EatingRoot instance = new EatingRoot();
        }

        private readonly IContainer _container;

        private EatingRoot()
        {
            _container = new Container();
            _container.Configure(config => config.MergeConfig(new EatingConfig(GetType().Assembly, _container)));

            #region Expressions to eat

            // - already implemented
            //*ICSharpLiteralExpression
            //*IObjectCreationExpression
            //*IInvocationExpression

            // - to work with, eat call reference
            //*IVariableInitializer

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
            //*IArrayCreationExpression
            //*IAnonymousMethodExpression
            //*IAnonymousObjectCreationExpression

            // - simply eat containing expression
            //*IPostfixOperatorExpression
            //*IPrefixOperatorExpression
            //*IAwaitExpression
            //*IUnaryOperatorExpression
            //*IParenthesizedExpression

            // - simply eat containing expressions and return none
            //*IAssignmentExpression (case: int i; i = 0;)

            // - depends of return type, may be stub, target, mock(after that simple eating)
            //*IDefaultExpression
            //*ILambdaExpression
            //*IReferenceExpression
            //*IElementAccessExpression
            //*IAsExpression
            //*ICastExpression


            // - commom expression interfaces, never eat
            //*IUnaryExpression
            //*IPrimaryExpression
            //*IOperatorExpression
            //*IPrimaryExpression
            //*ICreationExpression

            // - have an idea!
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

            //*IDeclarationStatement
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
            //IArglistParameterDeclaration ? Really rare to use

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
    }
}