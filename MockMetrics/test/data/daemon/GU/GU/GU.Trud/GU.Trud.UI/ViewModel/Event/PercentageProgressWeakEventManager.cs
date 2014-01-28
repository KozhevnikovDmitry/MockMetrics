using Common.UI.WeakEvent;
using GU.Trud.BL.Export.Interface;

namespace GU.Trud.UI.ViewModel.Event
{
    /// <summary>
    /// Менеждер слабых событий <c>PercentageProgressDelegate(object sender, PercentageProgressEventArgs e)</c>.
    /// </summary>
    /// <remarks>
    /// Использовать для источников реализующих <c>IExportService</c>.
    /// </remarks>
    public class PercentageProgressWeakEventManager : WeakEventManagerBase<PercentageProgressWeakEventManager, IGenerateExportService>
    {
        protected override void StartListening(IGenerateExportService source)
        {
            source.ExportProgressed += DeliverEvent;
        }

        protected override void StopListening(IGenerateExportService source)
        {
            source.ExportProgressed -= DeliverEvent;
        }
    }
}
