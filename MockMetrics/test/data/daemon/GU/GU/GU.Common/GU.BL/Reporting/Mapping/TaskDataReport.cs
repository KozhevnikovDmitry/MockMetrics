using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DictionaryManagement;
using Common.BL.Exceptions;
using Common.BL.ReportMapping;

using GU.BL.Reporting.Data;
using GU.DataModel;

namespace GU.BL.Reporting.Mapping
{
    public class TaskDataReport : IReport
    {
        private readonly Task _task;

        private readonly string _username;

        private readonly IDictionaryManager _dictionaryManager;

        public string ViewPath { get; private set; }

        public string DataAlias { get; private set; }

        public TaskDataReport(Task task, string username, IDictionaryManager dictionaryManager)
        {
            this._task = task;
            _username = username;
            _dictionaryManager = dictionaryManager;
            ViewPath = "Reporting/View/Common/TaskDataReport.mrt";
            DataAlias = "data";
        }

        public object RetrieveData()
        {
            var customerContacts = new List<string>();
            if (!string.IsNullOrWhiteSpace(this._task.CustomerPhone))
                customerContacts.Add("тел: " + this._task.CustomerPhone);
            if (!string.IsNullOrWhiteSpace(this._task.CustomerEmail))
                customerContacts.Add("email: " + this._task.CustomerEmail);

            return new TaskData
                {
                    Agency = this._task.Service.ServiceGroup.Agency.Name,
                    Service = string.Format("{0} ({1})", this._task.Service.Name, this._task.Service.ServiceGroup.ServiceGroupName),
                    Customer = this._task.CustomerFio,
                    CustomerContacts = string.Join(", ", customerContacts),
                    TaskId = this._task.Id,
                    CreateDate = this._task.CreateDate,
                    Status =
                        _dictionaryManager.GetEnumDictionary<TaskStatusType>()[(int)this._task.CurrentState],

                    Username = _username,

                    Items = this.MapContent(_task.Content).ToList()
                };


        }

        private IEnumerable<TaskDataItem> MapContent(Content content)
        {
            var result = MapNodes(content.RootContentNodes, null);

            for (var i = 0; i < result.Count(); i++)
            {
                result.ElementAt(i).Order = i;
            }

            return result;
        }

        private IEnumerable<TaskDataItem> MapNodes(EditableList<ContentNode> childNodes, Guid? parentId)
        {
            var result = new List<TaskDataItem>();
            foreach (var contentNode in childNodes)
            {
                result.Add(new TaskDataItem(Guid.NewGuid(), parentId, contentNode.SpecNode.Name, contentNode.GetValue()));

                if (contentNode.ChildContentNodes != null)
                {
                    result.AddRange(MapNodes(contentNode.ChildContentNodes, result.Last().Id));
                }
            }

            return result;
        }
    }

    internal static class ContentNodeExtensions
    {
        public static string GetValue(this ContentNode contentNode)
        {
            if (contentNode.SpecNode.SpecNodeType != SpecNodeType.Simple)
            {
                return string.Empty;
            }

            switch (contentNode.SpecNode.AttrDataType)
            {
                case AttrDataType.String: return contentNode.StrValue;
                case AttrDataType.Number: return contentNode.NumValue.ToString();
                case AttrDataType.File: return string.Format("файл: {0} ({1}, размер {2})", contentNode.BlobName, contentNode.BlobType, contentNode.BlobSize);
                case AttrDataType.Date: return contentNode.DateValue.HasValue ? (contentNode.DateValue.Value.ToString("dd.MM.yyyy")) : string.Empty;
                case AttrDataType.Boolean: return contentNode.BoolValue.HasValue ? (contentNode.BoolValue.Value ? "да" : "нет") : string.Empty;
                case AttrDataType.List: return contentNode.StrValue;
                default: throw new DomainBLLException(String.Format("Unknown AttrDataType={0}", contentNode.SpecNode.AttrDataType), contentNode);
            }
        }
    }
}
