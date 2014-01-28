using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;

namespace GU.Enisey.BL.Converters.Common
{
    [Obsolete("Тестовый класс, удалить")]
    public class TestBirthCertificateValidityInquiryXmlImporter : InquiryXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/interauthority-interaction/krsk/ValidityBirthCertificate/inquiry/1.00";

        private static readonly List<ContentXsdMatch> Matches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch {ContentUri = "zags/inquiry/validityBirthCertificate/request", RootName = Namespace + "ValidityBirthCertificate"}
            };

        public TestBirthCertificateValidityInquiryXmlImporter(IContentPolicy contentPolicy, IDictionaryManager dictionaryManager, IInquiryPolicy inquiryPolicy)
            : base(Matches, contentPolicy, dictionaryManager, inquiryPolicy)
        {
        }
    }
}
