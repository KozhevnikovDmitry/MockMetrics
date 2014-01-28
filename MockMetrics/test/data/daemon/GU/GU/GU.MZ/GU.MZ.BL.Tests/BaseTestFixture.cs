namespace GU.MZ.BL.Tests
{
    /// <summary>
    /// Базовый класс тестов
    /// </summary>
    public class BaseTestFixture
    {
        /// <summary>
        /// Базовый класс тестов
        /// </summary>
        public BaseTestFixture()
        {
            TestObjectMother = new TestObjectMother();
        }
        
        /// <summary>
        /// Объект, порождающий тестовые объекты
        /// </summary>
        public TestObjectMother TestObjectMother { get; protected set; }
    }
}
