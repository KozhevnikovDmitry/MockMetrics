using System.Collections.Generic;
using Common.Types.Exceptions;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    public class UnrecognizedStopAddressesException : BLLException
    {
        public DossierFile DossierFile { get; private set; }
        public List<Address> Addresses { get; private set; }

        public UnrecognizedStopAddressesException(DossierFile dossierFile, List<Address> unrecognizedAddresses)
            :base("По одному или нескольким адресам не найдены соответствующие объекты с номенклатурой.")
        {
            DossierFile = dossierFile;
            Addresses = unrecognizedAddresses;
        }
    }
}