using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Helpers
{
    [TestFixture]
    public class MetricHelperTests
    {
        //[Test]
        //public void ValueOfKindAsTypeOfKind_EatTargetCallTest()
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Assert
        //    foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
        //    {
        //        Assert.AreEqual(helper.CastExpressionType(ExpressionKind.TargetCall, kind), ExpressionKind.Result);
        //    }
        //}

        //[Test]
        //public void ValueOfKindAsTypeOfKind_EatResultTest()
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Assert
        //    foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
        //    {
        //        if (kind != ExpressionKind.TargetCall)
        //            Assert.AreEqual(helper.CastExpressionType(ExpressionKind.Result, kind), ExpressionKind.Result);
        //    }
        //}

        //[Test]
        //public void ValueOfKindAsTypeOfKind_EatMockTest()
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Assert
        //    foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
        //    {
        //        if (kind != ExpressionKind.TargetCall && kind != ExpressionKind.Result)
        //            Assert.AreEqual(helper.CastExpressionType(ExpressionKind.Mock, kind), ExpressionKind.Mock);
        //    }
        //}

        //[Test]
        //public void ValueOfKindAsTypeOfKind_EatTargetTest()
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Assert
        //    foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
        //    {
        //        if (kind != ExpressionKind.TargetCall &&
        //            kind != ExpressionKind.Result &&
        //            kind != ExpressionKind.Mock)
        //            Assert.AreEqual(helper.CastExpressionType(ExpressionKind.Target, kind), ExpressionKind.Target);
        //    }
        //    foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
        //    {
        //        if (kind != ExpressionKind.TargetCall &&
        //            kind != ExpressionKind.Result &&
        //            kind != ExpressionKind.Mock)
        //            Assert.AreEqual(helper.CastExpressionType(kind, ExpressionKind.Target), ExpressionKind.Target);
        //    }
        //}

        //[Test]
        //public void ValueOfKindAsTypeOfKind_EatAnyValueKindTest()
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Assert
        //    foreach (ExpressionKind valueKind in Enum.GetValues(typeof(ExpressionKind)))
        //    {
        //        if (valueKind != ExpressionKind.TargetCall &&
        //            valueKind != ExpressionKind.Result &&
        //            valueKind != ExpressionKind.Mock &&
        //            valueKind != ExpressionKind.Target)
        //            foreach (ExpressionKind typeKind in Enum.GetValues(typeof(ExpressionKind)))
        //            {
        //                if (typeKind != ExpressionKind.Target)
        //                    Assert.AreEqual(helper.CastExpressionType(valueKind, typeKind), valueKind);
        //            }
        //    }
        //}

        //[Test]
        //public void InvocationKindByParentReferenceKind_EatTargetCallTest()
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Act
        //    var kind = helper.InvocationKindByParentReferenceKind(ExpressionKind.TargetCall);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.Result);

        //}

        //[Test]
        //public void InvocationKindByParentReferenceKind_EatTargetTest()
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Act
        //    var kind = helper.InvocationKindByParentReferenceKind(ExpressionKind.Target);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.TargetCall);
        //}

        //[Test]
        //public void InvocationKindByParentReferenceKindTest()
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Assert
        //    foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
        //    {
        //        if (kind != ExpressionKind.TargetCall && kind != ExpressionKind.Target)
        //            Assert.AreEqual(helper.InvocationKindByParentReferenceKind(kind), kind);
        //    }
        //}

        //[TestCase(ExpressionKind.Target, Result = ExpressionKind.Result)]
        //[TestCase(ExpressionKind.TargetCall, Result = ExpressionKind.Result)]
        //[TestCase(ExpressionKind.Mock, Result = ExpressionKind.Result)]
        //[TestCase(ExpressionKind.Result, Result = ExpressionKind.Result)]
        //[TestCase(ExpressionKind.Stub, Result = ExpressionKind.Stub)]
        //[TestCase(ExpressionKind.StubCandidate, Result = ExpressionKind.StubCandidate)]
        //[TestCase(ExpressionKind.Assert, Result = ExpressionKind.Result)]
        //[TestCase(ExpressionKind.None, Result = ExpressionKind.None)]
        //public ExpressionKind ReferenceKindByParentReferenceKindTest(ExpressionKind kind)
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Assert
        //    return helper.ReferenceKindByParentReferenceKind(kind);
        //}

        //[TestCase(ExpressionKind.Target, Result = ExpressionKind.Target)]
        //[TestCase(ExpressionKind.TargetCall, Result = ExpressionKind.Result)]
        //[TestCase(ExpressionKind.Mock, Result = ExpressionKind.Mock)]
        //[TestCase(ExpressionKind.Result, Result = ExpressionKind.Result)]
        //[TestCase(ExpressionKind.Stub, Result = ExpressionKind.Stub)]
        //[TestCase(ExpressionKind.StubCandidate, Result = ExpressionKind.Stub)]
        //[TestCase(ExpressionKind.Assert, Result = ExpressionKind.Result)]
        //[TestCase(ExpressionKind.None, Result = ExpressionKind.None)]
        //public ExpressionKind KindOfAssignmentTest(ExpressionKind kind)
        //{
        //    // Arrange
        //    var helper = new VarTypeHelper();

        //    // Assert
        //    return helper.KindOfAssignment(kind);
        //}

        //[Test]
        //public void EatCastType_DynamicAsStubCandidateTest()
        //{
        //    // Arrange
        //    var snapshot = Mock.Of<ISnapshot>();
        //    var typeUsage = Mock.Of<IDynamicTypeUsage>();
        //    var helper = Mock.Of<EatExpressionHelper>();
        //    var typeEater = new TypeEater(helper);

        //    // Act
        //    var kind = typeEater.EatCastType(snapshot, typeUsage);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        //}

        //[Test]
        //public void EatCastType_PredefinedAsStubCandidateTest()
        //{
        //    // Arrange
        //    var snapshot = Mock.Of<ISnapshot>();
        //    var typeUsage = Mock.Of<IPredefinedTypeUsage>();;
        //    var helper = Mock.Of<EatExpressionHelper>();
        //    var typeEater = new TypeEater(helper);

        //    // Act
        //    var kind = typeEater.EatCastType(snapshot, typeUsage);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        //}

        //[Test]
        //public void EatCastType_UserTypeAsTargetTest()
        //{
        //    // Arrange
        //    var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == true);
        //    var typeUsage = Mock.Of<IUserTypeUsage>();
        //    var classType = Mock.Of<ITypeElement>(t => t.Module.Name == "ModuleName");
        //    var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
        //    var typeEater = new TypeEater(helper);

        //    // Act
        //    var kind = typeEater.EatCastType(snapshot, typeUsage);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.Target);
        //}

        //[Test]
        //public void EatCastType_UserTypeAsMockTest()
        //{
        //    // Arrange
        //    var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == true);
        //    var typeUsage = Mock.Of<IUserTypeUsage>();
        //    var classType = Mock.Of<ITypeElement>(t => t.Module.Name == "ModuleName");
        //    var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
        //    var typeEater = new TypeEater(helper);

        //    // Act
        //    var kind = typeEater.EatCastType(snapshot, typeUsage);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.Mock);
        //}

        //[Test]
        //public void EatCastType_UserTypeAsStubCandidateTest()
        //{
        //    // Arrange
        //    var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == false);
        //    var typeUsage = Mock.Of<IUserTypeUsage>();
        //    var classType = Mock.Of<ITypeElement>(t => t.Module.Name == "ModuleName");
        //    var helper = Mock.Of<EatExpressionHelper>(t => t.GetUserTypeUsageClass(typeUsage) == classType);
        //    var typeEater = new TypeEater(helper);

        //    // Act
        //    var kind = typeEater.EatCastType(snapshot, typeUsage);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        //}

        //[Test]
        //public void EatVariableType_TargetTest()
        //{
        //    // Arrange
        //    var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == true);
        //    var type = Mock.Of<IType>();
        //    var helper = Mock.Of<EatExpressionHelper>(t => t.GetTypeClass(type) == Mock.Of<ITypeElement>(e => e.Module.Name == "ModuleName"));
        //    var typeEater = new TypeEater(helper);

        //    // Act
        //    var kind = typeEater.EatVariableType(snapshot, type);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.Target);
        //}

        //[Test]
        //public void EatVariableType_MockTest()
        //{
        //    // Arrange
        //    var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == true);
        //    var type = Mock.Of<IType>();
        //    var helper = Mock.Of<EatExpressionHelper>(t => t.GetTypeClass(type) == Mock.Of<ITypeElement>(e => e.Module.Name == "ModuleName"));
        //    var typeEater = new TypeEater(helper);

        //    // Act
        //    var kind = typeEater.EatVariableType(snapshot, type);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.Mock);
        //}

        //[Test]
        //public void EatVariableType_StubCandidateTest()
        //{
        //    // Arrange
        //    var snapshot = Mock.Of<ISnapshot>(t => t.IsInTestScope("ModuleName") == false && t.IsInTestProject("ModuleName") == false);
        //    var type = Mock.Of<IType>();
        //    var helper = Mock.Of<EatExpressionHelper>(t => t.GetTypeClass(type) == Mock.Of<ITypeElement>(e => e.Module.Name == "ModuleName"));
        //    var typeEater = new TypeEater(helper);

        //    // Act
        //    var kind = typeEater.EatVariableType(snapshot, type);

        //    // Assert
        //    Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        //}

        //[Test]
        //public void NullEaterTest()
        //{
        //    // Assert
        //    Assert.Throws<ArgumentNullException>(() => new TypeEater(null));
        //}
        
        //[Test]
        //public void EatCastType_NullSnapshotTest()
        //{
        //    // Arrange
        //    var typeEater = new TypeEater(Mock.Of<EatExpressionHelper>());

        //    // Assert
        //    Assert.Throws<ArgumentNullException>(() => typeEater.EatCastType(null, Mock.Of<ITypeUsage>()));
        //}

        //[Test]
        //public void EatCastType_NullTypeUsageTest()
        //{
        //    // Arrange
        //    var typeEater = new TypeEater(Mock.Of<EatExpressionHelper>());

        //    // Assert
        //    Assert.Throws<ArgumentNullException>(() => typeEater.EatCastType(Mock.Of<ISnapshot>(), null));
        //}

        //[Test]
        //public void EatVariableType_NullSnapshotTest()
        //{
        //    // Arrange
        //    var typeEater = new TypeEater(Mock.Of<EatExpressionHelper>());

        //    // Assert
        //    Assert.Throws<ArgumentNullException>(() => typeEater.EatVariableType(null, Mock.Of<IType>()));
        //}

        //[Test]
        //public void EatVariableType_NullTypeTest()
        //{
        //    // Arrange
        //    var typeEater = new TypeEater(Mock.Of<EatExpressionHelper>());

        //    // Assert
        //    Assert.Throws<ArgumentNullException>(() => typeEater.EatVariableType(Mock.Of<ISnapshot>(), null));
        //}
    }
}
