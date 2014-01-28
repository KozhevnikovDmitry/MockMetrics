using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GU.HQ.BL.Reporting.Data
{
    public class ClaimRegistr
    {
        public string Username { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<ClaimRegistrInfo> ClaimRegistrInfoList { get; set; }

        public class ClaimRegistrInfo
        {
            public string DocDate { get; set; }               // Дата решения о принятии на учет, колонка 2
            public string DeclarerInfo { get; set; }            //Ф.И.О, дата рождения гражданина, подавшего заявление, колонка 3
            public int DeclarerRelativesCnt { get; set; }       //количество членов семьи (без учета г/с), колонка 4
            public string DeclarerRelativesInfo { get; set; }   //Ф.И.О и дата рождения членов семьи заявителя, колонка 5
            public string PrivDocDate { get; set; }           //дата возникновения права на внеочередное предоставление жилого помещения, колонка 6
            public string QueuePrivRegInfo { get; set; }        //номер и дата решения  о включении в список на внеочередное предоставление жилого помещения. Основания данного решения, колонка 7
            public int NumInQueuePriv { get; set; }             //номер по списку на внеочередное предоставление жилого помещения, колонка 8
            public string QueuePrivDeRegInfo { get; set; }      //дата и основание утраты права на внеочередное предоставление жилого помещения, колонка 9
            public string QueuePrivDeRegDecisionInfo { get; set; }   //дата и номер решения органа, осуществляющего постановку на учет, об исключении из списка на внеочередное предоставление жилого помещения, колонка 10
            public string HouseProvidedInfo { get; set; }       // дата и номер решения органа, осуществляющего принятие на учет, о предоставлении жилого помещения, колонка 11
            public string AddressInfo { get; set; }             // адрес проживания, колонка 12
            public string HouseProvidedAddressInfo { get; set; }// адрес предоставленного жилого помещения, колонка 13
            public string HouseOwn { get; set; }                // данные о наличии жилых помещения на праве собственности после предоставления жилого помещения по договору социального найма, колонка 14
            public string Note { get; set; }                    // примечание, колонка 15
            public string Category { get; set; }                // категория, колонка 16
            public string AddressDescInfo { get; set; }         // характеристика ЖП (количество комнат, подселение, домовладение), колонка 17
            public string AddressArea { get; set; }             // площадь ЖП (жилая/общая), колонка 18
            public string AddressHouseDoc { get; set; }         // основание проживания в жилом помещении, колонка 19
        }
    }
}
