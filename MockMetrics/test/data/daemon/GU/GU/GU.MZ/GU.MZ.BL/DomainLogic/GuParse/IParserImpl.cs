using GU.MZ.BL.DomainLogic.MzTask;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    /// <summary>
    /// Интерфейс для реализации парсера данных заявок
    /// Содержит тип услуги для выбора парсера на фасаде ParserFacade
    /// </summary>
    public interface IParserImpl : IParser
    {
        LicenseServiceType LicenseServiceType { get; }
    }
}