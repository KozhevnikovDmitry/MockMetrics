using BLToolkit.Aspects;
using BLToolkit.EditableObjects;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using NUnit.Framework;
using System.Linq;
using CommonData = GU.MZ.DataModel.CommonData;

namespace GU.MZ.BL.Test.BltoolkitTest
{
    /// <summary>
    /// Тесты операций клонирования EdtableObject
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class CloneEditableObjectTest
    {
        /// <summary>
        /// Тест на сохранение флажка IsDirty у сущности Address при клонирование
        /// </summary>
        [Test]
        public void PreserveIsDirtyAssosiatedAddressPerCloneTest()
        {
            // Arrange
            var obj = LicenseObject.CreateInstance();
            obj.Id = 1;
            obj.Address = Address.CreateInstance();
            obj.Address.City = "City";
            obj.Address.Id = 1;
            obj.AcceptChanges();

            // Act
            var clone = obj.Clone();
            clone.Id = 0;
            clone.Address.Id = 0;

            var secondClone = clone.Clone();

            // Assert
            Assert.AreNotEqual(obj, clone);
            Assert.AreNotEqual(clone, secondClone);
            Assert.AreNotEqual(obj.Address, clone.Address);
            Assert.AreNotEqual(clone.Address, secondClone.Address);

            Assert.False(obj.IsDirty);
            Assert.False(obj.Address.IsDirty);

            Assert.True(clone.IsDirty);
            Assert.True(clone.Address.IsDirty);

            Assert.True(secondClone.IsDirty);
            Assert.False(secondClone.Address.IsDirty);
        }
        
        /// <summary>
        /// Тест показывает багу клонирование EditableObject
        /// У клона другое "начальное" состояние, при RejectChanges клон очистит свои значения на дефолтные
        /// В результате, если изменение исходного объекта сводится, например, к зачистке поля на дефолтное значение (string.Empty),
        /// клон будет иметь IsDirty = false.
        /// Бага проявляется при попытке сохранить такой клон (клонирование делается в каждом DataMapper'е), когда данные не ложатся в БД,
        /// из-за того, что объект формально не грязный. 
        /// </summary>
        [Test]
        public void OrigialValueForClonedTaskTest()
        {
            // Arrange
            var task = Task.CreateInstance();
            task.Note = "100";
            task.AcceptChanges();
            task.Note = "200";

            // Act
            var clone = task.Clone();
            task.RejectChanges();
            clone.RejectChanges();

            // Assert
            Assert.AreEqual(task.Note, "100");
            Assert.AreEqual(clone.Note, string.Empty);
        }

        [Test]
        public void OrigialValueForSmartClonedTaskTest()
        {
            // Arrange
            var task = Task.CreateInstance();
            task.Note = "100";
            task.AcceptChanges();
            task.Note = "200";

            // Act
            var clone1 = task.Clone();
            task.RejectChanges();
            var clone2 = task.Clone();
            clone1.CopyTo(clone2);
            clone2.RejectChanges();

            // Assert
            Assert.AreEqual(task.Note, "100");
            Assert.AreEqual(clone2.Note, "100");
        }

        /// <summary>
        /// Тест показывающий выпадание StackOverflow при клонировании родительской сущности со списком дочерних сущностей
        /// При обычном клонировании, без применения CloneIgnore, переполенение возникает из-за свойства Child
        /// </summary>
        [Test]
        public void StackOverFlowOnCloneEditableObject()
        {
            // Arrange
            var parent = Parent.CreateInstance();
            var child = Child.CreateInstance();
            parent.Childs = new EditableList<Child> { child };
            child.Parent = parent;

            // Assert
            var clone = parent.Clone();
        }

        public abstract class Parent : DomainObject<Parent>, IPersistentObject
        {
            public EditableList<Child> Childs { get; set; }

            [CloneIgnore]
            public virtual Child Child
            {
                get { return Childs.First(); }
            }
        }

        public abstract class Child : DomainObject<Child>, IPersistentObject
        {
            public abstract Parent Parent { get; set; }
        }

        [Test]
        public void Testaction()
        {
            // Arrange
            var test = TestEnt.CreateInstance();

            // Act
            Assert.IsInstanceOf<IHasCommonData>(test);

            // Assert
        }


        [Test]
        public void OrderSaveTest()
        {
            // Arrange
            var order = StandartOrder.CreateInstance();
            order.Id = 1;
            order.RegNumber = "100500";

            var detail = StandartOrderDetail.CreateInstance();
            detail.SubjectId = "500100";
            order.DetailList.Add(detail);

            var clone = order.Clone();
            clone.DetailList.Single().OrderId = 1;
            clone.DetailList.Single().StandartOrder = clone;

            clone.AcceptChanges();
        }
    }

    public interface IHasCommonData
    {
        CommonData CommonCommon { get; set; }
    }

    public class HasCommonDataImpl : IHasCommonData
    {
        public CommonData CommonCommon { get; set; }
    }

    [Mixin(typeof(IHasCommonData), "_hasCommonDataImpl")]
    public abstract class TestEnt : IdentityDomainObject<TestEnt>, IPersistentObject
    {
        protected HasCommonDataImpl _hasCommonDataImpl;

        public TestEnt()
        {
            _hasCommonDataImpl = new HasCommonDataImpl();
        }

        public abstract override int Id { get; set; }
    }
}
