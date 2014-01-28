using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DomainContext;
using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.Types.Exceptions;

using GU.BL.Reporting.Data;
using GU.DataModel;
using System.Linq.Dynamic;

namespace GU.BL.Reporting.Mapping
{
    public class TaskStatReport : DbReport
    {
        public string Username { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public string Header { get; set; }
        public string[] Groups { get; set; }

        public class GroupInfo
        {
            public string Name { get; set; }
            public string Caption { get; set; }
            public string FieldIdExpression { get; set; }
            public Func<int?, string> FieldNameFunc { get; set; }
            public Func<int?, int?> OrderFunc { get; set; }
        }

        protected Dictionary<string, GroupInfo> GroupInfos = new List<GroupInfo>
            {
                new GroupInfo
                    {
                        Name = "Agency",
                        Caption = "Ведомство",
                        FieldIdExpression = "AgencyId",
                        FieldNameFunc = t => t.HasValue ? GuFacade.GetDictionaryManager().GetDictionaryItem<Agency>(t.Value).Name : "",
                    },
                new GroupInfo
                    {
                        Name = "Service",
                        Caption = "Услуга",
                        FieldIdExpression = "ServiceId",
                        FieldNameFunc = t => t.HasValue ? GuFacade.GetDictionaryManager().GetDictionaryItem<Service>(t.Value).Name : "",
                        OrderFunc = t => t.HasValue ? (int?)GuFacade.GetDictionaryManager().GetDictionaryItem<Service>(t.Value).Order : null
                    },
                new GroupInfo
                    {
                        Name = "ServiceGroup",
                        Caption = "Группа услуг",
                        FieldIdExpression = "Service.ServiceGroupId",
                        FieldNameFunc = t => t.HasValue ? GuFacade.GetDictionaryManager().GetDictionaryItem<ServiceGroup>(t.Value).ServiceGroupName : ""
                    },
                new GroupInfo
                    {
                        Name = "CurrentState",
                        Caption = "Статус",
                        FieldIdExpression = "Int32(CurrentState)",
                        FieldNameFunc = t => t.HasValue ? GuFacade.GetDictionaryManager().GetEnumDictionary<TaskStatusType>()[t.Value] : "",
                        OrderFunc = t => t
                    }
            }
            .ToDictionary(t => t.Name, t => t);
        

        public TaskStatReport(IDomainContext domainContext)
            : base(domainContext)
        {
            ViewPath = "Reporting/View/Common/TaskStatReport.mrt";
        }

        protected virtual IQueryable GetBaseQuery(IDomainDbManager dbManager, DateTime? date1, DateTime? date2)
        {
            var queryBase = dbManager.GetDomainTable<Task>();
            if (date1.HasValue)
                queryBase = queryBase.Where(t => t.CreateDate >= date1.Value.Date);
            if (date2.HasValue)
                queryBase = queryBase.Where(t => t.CreateDate < date2.Value.Date.AddDays(1));
            return queryBase;
        }

        protected override object RetrieveOperation(IDomainDbManager dbManager)
        {
            try
            {
                var taskStat = new TaskStat
                    {
                        Username = Username,
                        Stamp = DateTime.Now,
                        Date1 = Date1,
                        Date2 = Date2,
                        Header = Header
                    };
                var nameInfo = GroupInfos[Groups[Groups.Length - 1]];
                var groupInfos = Groups.Take(Groups.Length - 1).Select(t => GroupInfos[t]).ToList();

                var groupStr = String.Format("{0} as Id", nameInfo.FieldIdExpression);
                taskStat.NameCaption = nameInfo.Caption;
                if (groupInfos.Count > 0)
                {
                    groupStr += String.Format(", {0} as GroupId1", groupInfos[0].FieldIdExpression);
                    taskStat.GroupNameCaption = groupInfos[0].Caption;
                }
                if (groupInfos.Count > 1)
                {
                    groupStr += String.Format(", {0} as GroupId2", groupInfos[1].FieldIdExpression);
                    taskStat.GroupName2Caption = groupInfos[1].Caption;
                }
                if (groupInfos.Count > 2)
                {
                    groupStr += String.Format(", {0} as GroupId3", groupInfos[2].FieldIdExpression);
                    taskStat.GroupName3Caption = groupInfos[2].Caption;
                }

                groupStr = string.Format("new ({0})", groupStr);

                var query = GetBaseQuery(dbManager, Date1, Date2)
                    .GroupBy(groupStr, "it")
                    .Select("new(Key, it.Count() as Count)");

                var items = new List<TaskStat.TaskStatItem>();
                foreach (dynamic d in query)
                {
                    var item = new TaskStat.TaskStatItem();
                    item.Id = d.Key.Id;
                    item.Name = nameInfo.FieldNameFunc(d.Key.Id);
                    item.Order = nameInfo.OrderFunc == null ? null : nameInfo.OrderFunc(d.Key.Id);
                    item.Count = d.Count;
                    if (groupInfos.Count > 0)
                    {
                        item.GroupId = d.Key.GroupId1;
                        item.GroupName = groupInfos[0].FieldNameFunc(d.Key.GroupId1);
                        item.GroupOrder = groupInfos[0].OrderFunc == null ? null : groupInfos[0].OrderFunc(d.Key.GroupId1);
                    }
                    if (groupInfos.Count > 1)
                    {
                        item.GroupId2 = d.Key.GroupId2;
                        item.GroupName2 = groupInfos[1].FieldNameFunc(d.Key.GroupId2);
                        item.GroupOrder2 = groupInfos[1].OrderFunc == null ? null : groupInfos[1].OrderFunc(d.Key.GroupId2);
                    }
                    if (groupInfos.Count > 2)
                    {
                        item.GroupId3 = d.Key.GroupId3;
                        item.GroupName3 = groupInfos[2].FieldNameFunc(d.Key.GroupId3);
                        item.GroupOrder3 = groupInfos[2].OrderFunc == null ? null : groupInfos[2].OrderFunc(d.Key.GroupId3);
                    }
                    items.Add(item);
                }

                taskStat.TaskStatItems = items;
                return taskStat;
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при формировании данных отчёта по статистике заявок", ex);
            }
        }
    }
}
