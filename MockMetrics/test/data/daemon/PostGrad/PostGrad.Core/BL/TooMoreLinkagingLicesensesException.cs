using System;
using PostGrad.Core.Common;
using PostGrad.Core.DomainModel.Dossier;

namespace PostGrad.Core.BL
{
    public class TooMoreLinkagingLicesensesException : BLLException
    {
        public TooMoreLinkagingLicesensesException(LicenseDossier dossier, string regNumber, DateTime grantDate)
            : base(string.Format("В лицензионном деле id=[{0}] находится более одной лицензии с [{1}] от [{2}]", dossier.Id, regNumber, grantDate.ToShortDateString()))
        {
            
        }
    }
}