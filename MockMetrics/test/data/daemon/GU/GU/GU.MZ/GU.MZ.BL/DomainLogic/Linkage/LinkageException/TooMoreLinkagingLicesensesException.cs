using System;
using Common.Types.Exceptions;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    public class TooMoreLinkagingLicesensesException : BLLException
    {
        public TooMoreLinkagingLicesensesException(LicenseDossier dossier, string regNumber, DateTime grantDate)
            : base(string.Format("В лицензионном деле id=[{0}] находится более одной лицензии с [{1}] от [{2}]", dossier.Id, regNumber, grantDate.ToShortDateString()))
        {
            
        }
    }
}