using System.Windows.Forms;
using Stimulsoft.Report;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Viewer;

namespace Common.UI.ViewModel.Report
{
    /// <summary>
    /// Класс ViewModel для отображения Windows Forms просмотрщика StimulSoft отчётов. 
    /// </summary>
    internal class StimulReportVM : AbstractReportVM
    {
        /// <summary>
        /// Класс ViewModel для отображения Windows Forms просмотрщика StimulSoft отчётов. 
        /// </summary>
        /// <param name="stiReport">StimulSoft отчёт</param>
        /// <param name="isDesigner">Флаг открытия в режиме дизайнера</param>
        public StimulReportVM(StiReport stiReport, bool isDesigner = false)
        {
            WindowsFormsHost.Child = isDesigner ? (Control)CreateDesignerControl(stiReport) : CreateViewerControl(stiReport);
        }

        private StiDesignerControl CreateDesignerControl(StiReport report)
        {
            return new StiDesignerControl(report);
        }

        private StiViewerControl CreateViewerControl(StiReport report)
        {
            InitStiLocalization();
            var viewer = new StiViewerControl();
            viewer.Report = report;
            viewer.ShowOpen = false;
            viewer.ShowCloseButton = false;
            return viewer;
        }

        private void InitStiLocalization()
        {
            //TODO: установку локализации по хорошему бы вынести в что-то типа UIInitializer, но такого нету

            //NOTE: поскольку при паблише не копируется контент из подключаемых сборок,
            //то нужно в главный проект дополнительно подключить линк на файл локализации

            //вроде бы стимул должен подцепить текущую локализацию, но почему то этого не происходит, нужно указать вручную
            StiOptions.Configuration.DirectoryLocalization = @"Localization\Sti";
            StiOptions.Configuration.Localization = "ru.xml";
        }
    }
}
