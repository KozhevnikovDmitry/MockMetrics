using System.Collections.Generic;

using NUnit.Framework;

using SpecManager.BL.Model;

using System.Linq;

namespace SpecManager.BL.Test.Integration
{

    [TestFixture]
    public class SpecCrudTest : IntegrationFixture
    {
        #region Save Spec to DB

        /// <summary>
        /// Тест на сохранение спеки в базу данных с новым Uri
        /// </summary>
        [Test]
        public void SaveSpecToDbWithNewUriTest()
        {
            // Arrange
            var spec = ArrangeSpec();
            var depSpecs = ArrangeDependentSpecs(spec);
            var depTasks = ArrangeDependentTask(spec, depSpecs);

            var arangedspecId = spec.Id;

            // Act
            spec.Uri += "Джигурда";
            spec = CompositionRoot.GetDbSpecSource(spec.Id).Save(spec);

            // Assert
            Assert.AreNotEqual(spec.Id, arangedspecId,
                "Создана новая спека в базе");

            // Assert
            Assert.IsNotNull(CompositionRoot.TestDbManager.RetrieveDomainObject<Spec>(arangedspecId),
                "Исходная спека не была удалена");

            // Assert
            foreach (var depSpec in depSpecs)
            {
                Assert.IsNotNull(CompositionRoot.TestDbManager.RetrieveDomainObject<Spec>(depSpec.Id),
                    "Зависимая спека не была удалена");
            }

            // Assert
            foreach (var depTask in depTasks)
            {
                Assert.IsNotNull(CompositionRoot.TestDbManager.RetrieveDomainObject<Task>(depTask.Id),
                    "Зависимая задача не была удалена");
            }
        }
        
        /// <summary>
        /// Тест на сохранение спеки в базу данных
        /// </summary>
        [Test]
        public void SaveSpecToDbTest()
        {
            // Arrange
            var spec = ArrangeSpec();
            var depSpecs = ArrangeDependentSpecs(spec);
            var depTasks = ArrangeDependentTask(spec, depSpecs);
            var specNodeIds = spec.ChildSpecNodes.Select(t => t.Id).ToList();

            // Act
            spec.Name = "Джигурда";
            spec = CompositionRoot.GetDbSpecSource(spec.Id).Save(spec);

            // Assert
            Assert.AreEqual(spec.ChildSpecNodes.Count, 3,
                "Старые спекноды были удалены");

            // Assert
            foreach (var specNode in spec.ChildSpecNodes)
            {
                Assert.False(specNodeIds.Contains(specNode.Id),
                    "Старые спекноды заменены на новые");
            }

            // Assert
            foreach (var depTask in depTasks)
            {
                Assert.IsNull(CompositionRoot.TestDbManager.RetrieveDomainObject<Task>(depTask.Id),
                    "Зависимая заявка была удалена");
            }
        }

        /// <summary>
        /// Тест на получение списка зависимостей сохраняемой спеки
        /// </summary>
        [Test]
        public void GetPreSaveSpecDependenciesTest()
        {
            // Arrange
            var spec = ArrangeSpec();
            var depSpecs = ArrangeDependentSpecs(spec);
            var depTasks = ArrangeDependentTask(spec, depSpecs);

            // Act
            spec.Name = "Джигурда";
            var warning = CompositionRoot.GetDbSpecSource(spec.Id).PreSave(spec);

            // Assert
            Assert.That(warning.HasWarnings,
                "Найдены зависимости для спеки");

            // Assert
            Assert.AreEqual(warning.Messages.Count, 3 + 2 ,
                "Найдено три зависимые заявки и две зависимые спеки");
        }

        private List<Task> ArrangeDependentTask(Spec sourceSpec, List<Spec> dependentSpec)
        {
            var result = new List<Task>();

            var allSpec = dependentSpec.Concat(new List<Spec> { sourceSpec });

            foreach (var spec in allSpec)
            {
                var task = new Task
                {
                    Content = new Content { SpecId = spec.Id },
                    AgencyId = 1,
                    ServiceId = 1,
                    TaskStatusTypeId = 1,
                    CustomerFio = "Джигурда"
                };

                CompositionRoot.TestDbManager.SaveDomainObject(task.Content);
                task.ContentId = task.Content.Id;
                CompositionRoot.TestDbManager.SaveDomainObject(task);

                result.Add(task);
            }


            return result;
        }

        private List<Spec> ArrangeDependentSpecs(Spec spec)
        {
            var direstDepSpec = new Spec
                                    {
                                        Name = "Спека зависимая напрямую",
                                        Uri = "test/dependent/direct",
                                        ChildSpecNodes =
                                            new List<SpecNode>
                                                {
                                                    new SpecNode
                                                        {
                                                            Name = "Внешная тестовая нода 1",
                                                            SpecNodeType = SpecNodeType.RefSpec,
                                                            RefSpecId = spec.Id,
                                                            RefSpecUri = spec.Uri
                                                        }
                                                }
                                    };

            direstDepSpec = CompositionRoot.GetDataMapper<Spec>().Save(direstDepSpec);

            var indirestDepSpec = new Spec
            {
                Name = "Спека зависимая через одну внешнюю ссылку",
                Uri = "test/dependent/indirect",
                ChildSpecNodes =
                    new List<SpecNode>
                                                {
                                                    new SpecNode
                                                        {
                                                            Name = "Внешная тестовая нода 2",
                                                            SpecNodeType = SpecNodeType.RefSpec,
                                                            RefSpecId = direstDepSpec.Id,
                                                            RefSpecUri = direstDepSpec.Uri
                                                        }
                                                }
            };

            indirestDepSpec = CompositionRoot.GetDataMapper<Spec>().Save(indirestDepSpec);

            return new List<Spec> { direstDepSpec, indirestDepSpec };
        }

        private Spec ArrangeSpec()
        {
            var spec = new Spec
                           {
                               Name = "Тестовая спека",
                               Uri = "test/root",
                               ChildSpecNodes = new List<SpecNode>
                                                    {
                                                        new SpecNode
                                                            {
                                                                Name = "Простая тестовая нода 1",
                                                                SpecNodeType = SpecNodeType.Simple,
                                                                AttrDataType = AttrDataType.String,
                                                                MinOccurs = 1,
                                                                MaxOccurs = 1
                                                            },
                                                        new SpecNode
                                                            {
                                                                Name = "Комплексная тестовая нода",
                                                                SpecNodeType = SpecNodeType.Complex,
                                                                MinOccurs = 1,
                                                                MaxOccurs = 1,
                                                                ChildSpecNodes = new List<SpecNode>
                                                                                     {
                                                                                         new SpecNode
                                                                                                {
                                                                                                    Name = "Простая тестовая нода 2",
                                                                                                    SpecNodeType = SpecNodeType.Simple,
                                                                                                    AttrDataType = AttrDataType.String,
                                                                                                    MinOccurs = 1,
                                                                                                    MaxOccurs = 1
                                                                                                }
                                                                                     }
                                                            },
                                                        new SpecNode
                                                            {
                                                                Name = "Простая тестовая нода 3",
                                                                SpecNodeType = SpecNodeType.Simple,
                                                                AttrDataType = AttrDataType.Number,
                                                                MinOccurs = 1,
                                                                MaxOccurs = 1
                                                            },
                                                    },
                               DescendantNodes = new List<SpecNode>()
                           };

            return CompositionRoot.GetDataMapper<Spec>().Save(spec);
        }

        #endregion
    }
}
