using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.Archive.DataModel;
using GU.BL.Reporting.Mapping;
using GU.DataModel;

namespace GU.Archive.BL.Reporting.Mapping
{
    public class TaskPostStatReport : TaskStatReport
    {
        public TaskPostStatReport(IDomainContext domainContext)
            : base(domainContext)
        {
            var archiveGroupInfos = new List<GroupInfo>
            {
                new GroupInfo
                    {
                        Name = "DeliveryType",
                        Caption = "Способ доставки",
                        FieldIdExpression = "Int32?(DeliveryType)",
                        FieldNameFunc = t => t.HasValue ? ArchiveFacade.GetDictionaryManager().GetEnumDictionary<DeliveryType>()[t.Value] : "<не указано>",
                        OrderFunc = t => t
                    },
                new GroupInfo
                    {
                        Name = "RequestType",
                        Caption = "Тип запроса",
                        FieldIdExpression = "Int32?(RequestType)",
                        FieldNameFunc = t => t.HasValue ? ArchiveFacade.GetDictionaryManager().GetEnumDictionary<RequestType>()[t.Value] : "<не указано>",
                        OrderFunc = t => t
                    },
                new GroupInfo
                    {
                        Name = "Author",
                        Caption = "Тип автора",
                        FieldIdExpression = "Int32(AuthorId)",
                        FieldNameFunc = t => t != 0 ? ArchiveFacade.GetDictionaryManager().GetDictionaryItem<Author>(t.Value).Name : "<не указано>",
                        OrderFunc = t => t
                    }
            };

            archiveGroupInfos.ForEach(t => GroupInfos.Add(t.Name, t));
        }

        protected override IQueryable GetBaseQuery(IDomainDbManager dbManager, DateTime? date1, DateTime? date2)
        {
            var queryBase =
                from t in dbManager.GetDomainTable<Task>()
                join p in dbManager.GetDomainTable<Post>() 
                on t.Id equals p.TaskId into tp
                from p in tp.DefaultIfEmpty()
                select new
                    {
                        t.Id, t.CreateDate, t.CurrentState, 
                        t.AgencyId, t.Agency, 
                        t.ServiceId, t.Service,
                        DeliveryType = p != null ? p.DeliveryType : (DeliveryType?)null,
                        RequestType = p != null ? p.RequestType : (RequestType?)null,
                        p.AuthorId
                    }
                ;

            if (date1.HasValue)
                queryBase = queryBase.Where(t => t.CreateDate >= date1.Value.Date);
            if (date2.HasValue)
                queryBase = queryBase.Where(t => t.CreateDate < date2.Value.Date.AddDays(1));

            return queryBase;
        }
    }
}
