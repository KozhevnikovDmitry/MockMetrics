using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.DA;
using GU.DataModel;
using GU.MZ.BL.Tests;
using GU.MZ.BL.Tests.AcceptanceTest;
using NUnit.Framework;

namespace GU.MZ.BL.Test.AcceptanceTest.TaskData
{
    /// <summary>
    /// Базовый класс для классов с приёмочными тестами на ведение тома лицензионного дела
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public abstract class ContentFixture : MzAcceptanceTests
    {

        #region Arrange

        /// <summary>
        /// Подготавливает Заявку для теста
        /// </summary>
        /// <returns>Заявка для теста</returns>
        protected Task ArrangeTask(int serviceId)
        {
            var taskPolicy = MzLogicFactory.GetTaskPolicy();
            var task = taskPolicy.CreateDefaultTask(DictionaryManager.GetDictionaryItem<Service>(serviceId));
            task.CustomerFio = "Джигурда-жигурда";
            task.CustomerEmail = "gigurda@gmail.ru";
            task.CustomerPhone = "100500100500";

            task.Content = GenerateData(task.Service.Spec);
            
            //task = taskPolicy.SetStatus(TaskStatusType.CheckupWaiting, string.Empty, task);
            task = MzLogicFactory.ResolveDataMapper<Task>().Save(task);
            return task;
        }

        private Content GenerateData(Spec spec)
        {
            var data = Content.CreateInstance();
            data.Spec = spec;
            data.RootContentNodes = new EditableList<ContentNode>(GenerateDataElements(spec, data));
            return data;
        }

        private List<ContentNode> GenerateDataElements(Spec spec, Content content)
        {
            var dataElements = new List<ContentNode>();
            foreach (var element in spec.RootSpecNodes)
            {
                int cnt = RandomProvider.Random(element.MinOccurs, element.MaxOccurs ?? 10);
                for (int i = 0; i < cnt; i++)
                {
                    var dataElement = GenerateDataElement(element, content);
                    dataElement.Content = content;
                    //Content.DataElements.Add(dataElement);
                    dataElements.Add(dataElement);
                }
            }
            return dataElements;
        }

        private ContentNode GenerateDataElement(SpecNode specNode, Content content)
        {
            var dataElement = ContentNode.CreateInstance();
            dataElement.Content = content;
            //Content.DataElements.Add(dataElement);
            dataElement.SpecNode = specNode;

            if (specNode.SpecNodeType == SpecNodeType.Complex)
            {
                foreach (var childElement in specNode.ChildSpecNodes)
                {
                    var childDataElement = GenerateDataElement(childElement, content);
                    dataElement.ChildContentNodes.Add(childDataElement);
                    childDataElement.ParentContentNode = dataElement;
                }
            }
            else if (specNode.SpecNodeType == SpecNodeType.ComplexChoice)
            {
                var childElement = specNode.ChildSpecNodes[RandomProvider.Random(specNode.ChildSpecNodes.Count)];
                var childDataElement = GenerateDataElement(childElement, content);
                dataElement.ChildContentNodes.Add(childDataElement);
                childDataElement.ParentContentNode = dataElement;
            }
            else if (specNode.SpecNodeType == SpecNodeType.RefSpec)
            {
                var childDataElements = GenerateDataElements(specNode.RefSpec, content);
                foreach (var childDataElement in childDataElements)
                {
                    dataElement.ChildContentNodes.Add(childDataElement);
                    childDataElement.ParentContentNode = dataElement;
                }
            }
            else if (specNode.SpecNodeType == SpecNodeType.Simple)
            {
                switch (specNode.AttrDataType)
                {
                    case AttrDataType.Boolean:
                        {
                            dataElement.BoolValue = true;
                            break;
                        }
                    case AttrDataType.Date:
                        {
                            dataElement.DateValue = DateTime.Today;
                            break;
                        }
                    case AttrDataType.Number:
                        {
                            dataElement.NumValue = 1;
                            break;
                        }
                    case AttrDataType.String:
                        {
                            string str = null;
                            if (!string.IsNullOrEmpty(specNode.FormatRegexp))
                                str = GenerateStringFromRegexp(specNode.FormatRegexp);
                            str = str ?? "Тест";

                            dataElement.StrValue = str;
                            break;
                        }
                    case AttrDataType.List:
                        {
                            var dictItem = specNode.Dict.DictDets.First();
                            dataElement.StrValue = dictItem.ItemName;
                            dataElement.DictKey = dictItem.ItemKey;
                            break;
                        }
                }
            }
            else
            {
                throw new Exception("unknown SpecNodeType");
            }

            return dataElement;
        }

        private string GenerateStringFromRegexp(string formatRegexp)
        {
            //в идеале можно прикрутить автогенератор случайных строк по рег. выражению https://github.com/moodmosaic/Fare
            //пока что сюда можно прописывать встречающиеся рег. выражения
            switch (formatRegexp)
            {
                case @"^\d{6}$": return "123456";
                default: return null;
            }
        }

        
        #endregion

        
        /// <summary>
        /// Тест на заведение новой заявки
        /// </summary>
        [TestCase(1)]
        [TestCase(5)]
        public void NewTaskSavingTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Assert
            Assert.AreEqual(task.PersistentState, PersistentState.Old, "Saving task assert");
        }
    }
}
