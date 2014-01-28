using System;
using System.Linq;
using Common.BL.DomainContext;
using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.HQ.BL.Reporting.Data;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.Reporting.Mapping
{
    public class ClaimRegistrReportMapper : AbstractReportMapper<ClaimRegistr>
    {
        private readonly string _username;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public ClaimRegistrReportMapper(string username, DateTime startDate, DateTime endDate, IDomainContext domainContext)
            : base(domainContext)
        {
            this._username = username;
            this._startDate = startDate;
            this._endDate = endDate;
        }

        public override ClaimRegistr Retrieve(IDomainDbManager dbManager)
        {
            try
            {
                return new ClaimRegistr
                    {
                        ClaimRegistrInfoList = (from cqr in dbManager.GetDomainTable<ClaimQueueReg>().Where(t => t.DocumentDate >= _startDate && t.DocumentDate <= _endDate)
                                                join c in dbManager.GetDomainTable<Claim>() on cqr.Id equals c.Id
                                                join p in dbManager.GetDomainTable<Person>() on c.DeclarerId equals p.Id
                                                join dr in dbManager.GetDomainTable<DeclarerRelative>() on c.Id equals dr.ClaimId
                                                join p1 in dbManager.GetDomainTable<Person>() on dr.PersonId equals p1.Id
                                                join qp in dbManager.GetDomainTable<QueuePriv>() on cqr.Id equals qp.ClaimId into g
                                                from qp in g.DefaultIfEmpty()
                                               
                                                join qpr in dbManager.GetDomainTable<QueuePrivReg>() on qp.QueuePrivRegId equals qpr.Id into g3
                                                from qpr in g3.DefaultIfEmpty()

                                                join qpdr in dbManager.GetDomainTable<QueuePrivDeReg>() on qp.QueuePrivDeRegId equals qpdr.Id into g1
                                                from qpdr in g1.DefaultIfEmpty()

                                                join hp in dbManager.GetDomainTable<HouseProvided>() on cqr.Id equals hp.ClaimId into g2
                                                from hp in g2.DefaultIfEmpty()

                                                join ahp in dbManager.GetDomainTable<Address>() on hp.AddressId equals ahp.Id into g4
                                                from ahp in g4.DefaultIfEmpty()

                                                join adhp in dbManager.GetDomainTable<AddressDesc>() on ahp.Id equals adhp.Id into g5
                                                from adhp in g5.DefaultIfEmpty()

                                                join pa in dbManager.GetDomainTable<PersonAddress>().Where(t => t.AddressTypeId == AddressType.Residence) on p.Id equals pa.PersonId into g6
                                                from pa in g6.DefaultIfEmpty()

                                                join a in dbManager.GetDomainTable<Address>() on pa.AddressId equals a.Id into g7
                                                from a in g7.DefaultIfEmpty()

                                                select new ClaimRegistr.ClaimRegistrInfo 
                                                   {
                                                       DocDate = cqr.DocumentDate.ToString(),
                                                       DeclarerInfo = p.FioCurrent + " "+ p.BirthDate.ToShortDateString() ,
                                                       DeclarerRelativesInfo = p1.Sname + " " + p1.Name + " " + p1.Patronymic + " " + p1.BirthDate.ToShortDateString(),
                                                       PrivDocDate = (qpr == null ? "" : qpr.DateLaw.ToString()),
                                                       QueuePrivRegInfo = (qpr == null ? "" : qpr.DecisionNum + " " + qpr.DecisionDate.Value.ToShortDateString() + " " + qpr.DocumentNum + " " + qpr.DocumentDate.Value.ToShortDateString()),
                                                       QueuePrivDeRegInfo = (qpdr == null ? "" : qpdr.DocumentNum + " " + qpdr.DocumentDate.Value.ToShortDateString()),
                                                       QueuePrivDeRegDecisionInfo = (qpdr == null ? "" : qpdr.DecisionNum + " " + qpdr.DecisionDate.Value.ToShortDateString()),
                                                       AddressInfo =  (a == null ? "" : a.PostIndex + ", " + a.City + ", ул.  " + a.Street + ", дом " + a.HouseNum + ", корп. " + a.KorpNum + ", кв.  " + a.KvNum),
                                                       HouseProvidedInfo = (hp == null ? "" : hp.DocumentNum  + " " +  hp.DocumentDate.Value.ToShortDateString() ),
                                                       HouseProvidedAddressInfo = (ahp == null ? "" : ahp.PostIndex + ", " + ahp.City + ", ул.  " + ahp.Street + ", дом " + ahp.HouseNum + ", корп. " + ahp.KorpNum + ", кв.  " + ahp.KvNum),
                                                       HouseOwn = (hp == null ? "" : hp.HomeOwn),
                                                       AddressHouseDoc = (adhp == null ? "" : adhp.HouseDoc),
                                                       AddressDescInfo = (adhp == null ? "" : "Комнат: " + adhp.RoomCount),
                                                       AddressArea = (adhp == null ? "" : adhp.AreaGenegal + " // " + adhp.AreaLiving),
                                                       Note = (hp == null ? "" : hp.Note)
                                                   }).OrderBy(t => t.DocDate).ToList(),
                        Username = _username,
                        StartDate = _startDate.Date,
                        EndDate = _endDate.Date
                    };
                 
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при формировании данных отчёта по оказанным услугам", ex);
            }
        }
    }
}
