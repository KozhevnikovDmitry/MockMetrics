using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common.Exceptions;

namespace GU.Enisey.BL.Converters.Common
{
    public abstract class ContentXmlImporter : IContentXmlImporter
    {
        protected readonly IContentPolicy ContentPolicy;

        protected readonly IDictionaryManager DictionaryManager;

        protected ContentXmlImporter(List<ContentXsdMatch> matchList, IContentPolicy contentPolicy, IDictionaryManager dictionaryManager)
        {
            ContentPolicy = contentPolicy;
            DictionaryManager = dictionaryManager;
            SupportedMatches = matchList;
        }

        private List<ContentXsdMatch> SupportedMatches { get; set; }

        public XName[] SupportedTags { get { return SupportedMatches.Select(t => t.RootName).ToArray(); } }

        public Content ImportFromXml(XmlData xmlData)
        {
            var context = ImportFromXmlContext(xmlData);
            return context.Content;
        }

        protected ConvertationContext ImportFromXmlContext(XmlData xmlData)
        {
            var match = FindMatch(xmlData.Xml);
            var contentXml = xmlData.Xml;
            if (match.SubRootName != null)
            {
                contentXml = contentXml.Elements().Single();
                if (match.SubSubRootName != null)
                    contentXml = contentXml.Elements().Single();
            }

            var spec = DictionaryManager.GetDictionary<Spec>().Single(t => t.Uri == match.ContentUri);

            var convertationContext = new ConvertationContext(contentXml) { Attachements = xmlData.Attachements };
            ConvertContent(contentXml.Elements().ToList(), spec, convertationContext);
            var content = convertationContext.Content;

            var notVisitedElements = contentXml.Descendants().Except(convertationContext.VisitedElements).ToList();
            if (notVisitedElements.Count > 0)
            {
                var notVisitedElementNames = notVisitedElements
                    .Select(e => String.Join("/", e.AncestorsAndSelf().Reverse().Select(ea => ea.Name.LocalName)));
                throw new ConverterException(string.Format("Неизвестные элементы (отсутствуют в спецификации): {0}", String.Join("\n", notVisitedElementNames)));
            }
            var notVisitedAttributes = contentXml.Descendants().Attributes().Except(convertationContext.VisitedAttributes).ToList();
            if (notVisitedAttributes.Count > 0)
            {
                var notVisitedAttributeNames = notVisitedAttributes
                    .Select(a => String.Join("/", a.Parent.AncestorsAndSelf().Reverse().Select(ea => ea.Name.LocalName)) + " " + a.Name.LocalName);
                throw new ConverterException(string.Format("Неизвестные атрибуты (отсутствуют в спецификации): {0}", String.Join("\n", notVisitedAttributeNames)));
            }

            return convertationContext;
        }

        private ContentXsdMatch FindMatch(XElement xml)
        {
            var rootName = xml.Name;
            var subRootName = xml.Elements().Count() == 1 ? xml.Elements().Single().Name : null;
            var subSubRootName = subRootName != null && xml.Elements().Elements().Count() == 1 ? xml.Elements().Elements().Single().Name : null;

            var matches = SupportedMatches
                .Where(t => t.RootName == rootName
                            && (t.SubRootName == null || t.SubRootName == subRootName)
                            && (t.SubSubRootName == null || t.SubSubRootName == subSubRootName))
                .ToList();

            if (matches.Count == 0)
                throw new ConverterException(string.Format("Converter doesn't support element {0}", xml.Name));

            if (matches.Count > 1)
                throw new ConverterException(string.Format("Too many matches for element {0}", xml.Name));

            return matches.Single();
        }

        protected void ConvertContent(List<XElement> elements, Spec spec, ConvertationContext convertationContext)
        {
            var content = ContentPolicy.CreateEmpty(spec);
            convertationContext.Content = content;

            //немного костыль: для реализации ContentVm принято соглашение что в заявлении всегда есть ровно один корневой фиктивный Complex элемент, который отсутствует в файле от Енисей
            if (spec.RootSpecNodes.Count != 1)
                throw new ConverterException(string.Format("Неверно настроенная спецификация {0}: должен быть ровно один корневой элемент", spec.Id));
            var rootSpecNode = spec.RootSpecNodes[0];
            if (rootSpecNode.SpecNodeType != SpecNodeType.Complex)
                throw new NodeConverterException(rootSpecNode, string.Format("Неверно настроенная спецификация {0}: корневой элемент должен быть типа Complex", spec.Id));

            var rootContentNode = ContentPolicy.CreateEmptyNode(rootSpecNode);
            var contentNodes = ConvertContentNodes(elements, rootSpecNode.ChildSpecNodes, convertationContext);
            rootContentNode.ChildContentNodes.AddRange(contentNodes);

            content.RootContentNodes.Add(rootContentNode);
        }

        protected List<ContentNode> ConvertContentNodes(List<XElement> elements, List<SpecNode> specNodes, ConvertationContext convertationContext)
        {
            var contentNodes = new List<ContentNode>();

            foreach (var specNode in specNodes)
            {
                if (specNode.SpecNodeType == SpecNodeType.ComplexChoice)
                {
                    var choiceNodes = new List<ContentNode>();
                    foreach (var childSpecNode in specNode.ChildSpecNodes)
                    {
                        var childElements = GetElementsForSpecNode(elements, childSpecNode);
                        foreach (var childElement in childElements)
                        {
                            var choiceNode = ContentPolicy.CreateEmptyNode(specNode);
                            choiceNode.ChildContentNodes.Add(ConvertContentNode(childElement, childSpecNode, convertationContext));
                            choiceNodes.Add(choiceNode);
                        }
                    }

                    if (choiceNodes.Count < specNode.MinOccurs)
                        throw new NodeConverterException(specNode, string.Format("Количество элементов меньше минимального (минимальное {0}, в наличии {1})", specNode.MinOccurs, choiceNodes.Count));
                    if (choiceNodes.Count > specNode.MaxOccurs)
                        throw new NodeConverterException(specNode, string.Format("Количество элементов больше максимального (максимальное {0}, в наличии {1})", specNode.MaxOccurs, choiceNodes.Count));

                    contentNodes.AddRange(choiceNodes);
                }
                else
                {
                    var nodeElements = GetElementsForSpecNode(elements, specNode);

                    if (nodeElements.Count < specNode.MinOccurs)
                        throw new NodeConverterException(specNode, string.Format("Количество элементов меньше минимального (минимальное {0}, в наличии {1})", specNode.MinOccurs, nodeElements.Count));
                    if (nodeElements.Count > specNode.MaxOccurs)
                        throw new NodeConverterException(specNode, string.Format("Количество элементов больше максимального (максимальное {0}, в наличии {1})", specNode.MaxOccurs, nodeElements.Count));

                    foreach (var element in nodeElements)
                        contentNodes.Add(ConvertContentNode(element, specNode, convertationContext));

                    //необязательное поле (simple 0..1) заводим даже если оно отсуствует в исходном файле
                    if (nodeElements.Count == 0 &&
                        specNode.SpecNodeType == SpecNodeType.Simple &&
                        specNode.MinOccurs == 0
                        && specNode.MaxOccurs == 1)
                    {
                        contentNodes.Add(ContentPolicy.CreateEmptyNode(specNode));
                    }
                }
            }

            return contentNodes;
        }

        protected virtual ContentNode ConvertContentNode(XElement element, SpecNode specNode, ConvertationContext convertationContext)
        {
            //TODO: костыли со спецобработкой. подумать как-бы выделить в отдельные классы
            if (specNode.SpecNodeType == SpecNodeType.RefSpec && specNode.RefSpec.Uri == "ref/document")
            {
                try
                {
                    var docNode = ContentPolicy.CreateDefaultNode(specNode);
                    var docRoot = element.Elements().SingleOrDefault();
                    var docTypeSpecNode = specNode.RefSpec.RootSpecNodes.SingleOrDefault(t => t.Tag == "DocumentType");
                    var docTypeDictItem = docTypeSpecNode.Dict.DictDets.SingleOrDefault(t => t.ItemKey == docRoot.Name.LocalName);

                    var docTypeNode = docNode.ChildContentNodes.Single(t => t.SpecNode.Tag == "DocumentType");
                    var seriesNode = docNode.ChildContentNodes.Single(t => t.SpecNode.Tag == "Series");
                    var numberNode = docNode.ChildContentNodes.Single(t => t.SpecNode.Tag == "Number");
                    var issueDateNode = docNode.ChildContentNodes.Single(t => t.SpecNode.Tag == "IssueDate");
                    var issuerNode = docNode.ChildContentNodes.Single(t => t.SpecNode.Tag == "Issuer");

                    var series = docRoot.Elements().SingleOrDefault(t => t.Name.LocalName == "Series");
                    var number = docRoot.Elements().SingleOrDefault(t => t.Name.LocalName == "Number");
                    var issueDate = docRoot.Elements().SingleOrDefault(t => t.Name.LocalName == "Date" || t.Name.LocalName == "IssueDate");
                    var issuer = docRoot.Elements().SingleOrDefault(t => t.Name.LocalName == "IssueOrgan" || t.Name.LocalName == "Issuer");

                    docTypeNode.StrValue = docTypeDictItem.ItemName;
                    docTypeNode.DictKey = docTypeDictItem.ItemKey;
                    seriesNode.StrValue = series != null ? series.Value : null;
                    numberNode.StrValue = number != null ? number.Value : null;
                    issueDateNode.DateValue = issueDate != null ? (DateTime?)DateTime.Parse(issueDate.Value) : null;
                    issuerNode.StrValue = issuer != null ? issuer.Value : null;

                    convertationContext.Visit(element, docRoot, series, number, issueDate, issuer);

                    return docNode;
                }
                catch (Exception ex)
                {
                    throw new NodeConverterException(specNode, "Ошибка разбора ref/document", ex);
                }
            }

            convertationContext.Visit(element);

            var contentNode = ContentPolicy.CreateEmptyNode(specNode);

            if (specNode.SpecNodeType == SpecNodeType.Complex)
            {
                var childNodes = ConvertContentNodes(element.Elements().ToList(), specNode.ChildSpecNodes, convertationContext);
                contentNode.ChildContentNodes.AddRange(childNodes);
            }
            else if (specNode.SpecNodeType == SpecNodeType.RefSpec)
            {
                var childNodes = ConvertContentNodes(element.Elements().ToList(), specNode.RefSpec.RootSpecNodes, convertationContext);
                contentNode.ChildContentNodes.AddRange(childNodes);
            }
            else if (specNode.SpecNodeType == SpecNodeType.Simple)
            {
                SetContentNodeValue(contentNode, element, convertationContext);
            }
            //ComplexChoice обрабатывается в ConvertContentNodes
            //else if (specNode.SpecNodeType == SpecNodeType.ComplexChoice)
            else
            {
                throw new NodeConverterException(specNode,
                                                     string.Format("Неизвестный тип узла {0}", specNode.SpecNodeType));
            }

            return contentNode;
        }

        protected void SetContentNodeValue(ContentNode contentNode, XElement element, ConvertationContext convertationContext)
        {
            var hasChild = element.HasElements;
            switch (contentNode.SpecNode.AttrDataType)
            {
                case AttrDataType.String:
                    if (hasChild) throw new NodeConverterException(contentNode.SpecNode, "Имеются потомки для простого типа");
                    contentNode.StrValue = element.Value;
                    break;
                case AttrDataType.Number:
                    if (hasChild) throw new NodeConverterException(contentNode.SpecNode, "Имеются потомки для простого типа");
                    if (!string.IsNullOrWhiteSpace(element.Value))
                    {
                        decimal numVal;
                        if (decimal.TryParse(element.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out numVal))
                            contentNode.NumValue = numVal;
                        else
                            throw new NodeConverterException(contentNode.SpecNode,
                                                                 string.Format("Неверный формат числа {0}", element.Value));
                    }
                    break;
                case AttrDataType.Date:
                    if (hasChild) throw new NodeConverterException(contentNode.SpecNode, "Имеются потомки для простого типа");
                    if (!string.IsNullOrWhiteSpace(element.Value))
                    {
                        DateTime dateVal;
                        if (DateTime.TryParse(element.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                            contentNode.DateValue = dateVal;
                        else
                            throw new NodeConverterException(contentNode.SpecNode,
                                                                 string.Format("Неверный формат даты {0}", element.Value));
                    }
                    break;
                case AttrDataType.Boolean:
                    if (hasChild) throw new NodeConverterException(contentNode.SpecNode, "Имеются потомки для простого типа");
                    if (!string.IsNullOrWhiteSpace(element.Value))
                    {
                        bool boolVal;
                        if (Boolean.TryParse(element.Value, out boolVal))
                            contentNode.BoolValue = boolVal;
                        else
                        {
                            if (element.Value.ToLower() == "да") contentNode.BoolValue = true;
                            else if (element.Value.ToLower() == "нет") contentNode.BoolValue = false;
                            else
                            {
                                throw new NodeConverterException(contentNode.SpecNode,
                                                                     string.Format("Неверный формат флажка {0}",
                                                                                   element.Value));
                            }
                        }
                    }
                    break;
                case AttrDataType.List:
                    if (element.HasElements)
                    {
                        var subElements = element.Elements().ToArray();
                        if (!(subElements.Length == 2 && subElements[0].Name.LocalName == "code" && subElements[1].Name.LocalName == "displayText"))
                            throw new NodeConverterException(contentNode.SpecNode, string.Format("Неверный формат значения списка с кодом {0}", element));
                        contentNode.DictKey = subElements[0].Value;
                        contentNode.StrValue = subElements[1].Value;
                        convertationContext.Visit(subElements);
                    }
                    else
                    {
                        contentNode.StrValue = element.Value;
                    }
                    break;
                case AttrDataType.File:

                    var name = element.Elements().SingleOrDefault(t => t.Name.LocalName == "Name");
                    var url = element.Elements().SingleOrDefault(t => t.Name.LocalName == "URL");
                    var mimeType = element.Elements().SingleOrDefault(t => t.Name.LocalName == "Type");

                    if (name == null || url == null || mimeType == null)
                        throw new NodeConverterException(contentNode.SpecNode, "Ошибка загрузки поля типа Файл: неверный формат описания");

                    if (!convertationContext.Attachements.ContainsKey(url.Value))
                        throw new NodeConverterException(contentNode.SpecNode, string.Format("Ошибка загрузки поля типа Файл: не найден путь {0}", url.Value));

                    contentNode.BlobName = name.Value;
                    contentNode.BlobType = mimeType.Value;
                    contentNode.BlobValue = convertationContext.Attachements[url.Value];
                    contentNode.BlobSize = contentNode.BlobValue.Length;

                    convertationContext.Visit(name, url, mimeType);

                    break;

                default:
                    throw new NodeConverterException(contentNode.SpecNode, string.Format("Неизвестный тип данных узла {0}", contentNode.SpecNode.AttrDataType));
            }
        }

        protected virtual List<XElement> GetElementsForSpecNode(List<XElement> elements, SpecNode specNode)
        {
            if (string.IsNullOrWhiteSpace(specNode.Tag))
                throw new NodeConverterException(specNode, "specNode.Tag is empty");

            return elements.Where(t => t.Name.LocalName == specNode.Tag).ToList();
        }

    }
}
