using System;
using System.IO;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View.SearchView;
using Common.UI.ViewModel;

using GU.Trud.BL;
using GU.Trud.DataModel;
using GU.Trud.UI.View;
using Microsoft.Practices.Prism.Commands;

namespace GU.Trud.UI.ViewModel
{
    public class TrudRepositoryVM : BaseAvalonDockVM
    {
        public TrudRepositoryVM()
            : base(new SingletonDockableUiFactory())
        {
            GenerateExportsCommand = new DelegateCommand(GenerateExports);
            ShowExportsCommand = new DelegateCommand(ShowExports);
        }

        #region Binding Properties

        #endregion

        #region Binding Commands

        public DelegateCommand GenerateExportsCommand { get; protected set; }

        public DelegateCommand ShowExportsCommand { get; protected set; }

        private void ShowExports()
        {
            try
            {
                var taskExportSearchVm = UIFacade.GetSearchVM<TaskExport>();
                taskExportSearchVm.ChooseResultRequested += OnExportSendingRequested;
                AddDockable("Выгрузки", new SearchView(), taskExportSearchVm);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch(Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка",ex));
            }
        }

        void OnExportSendingRequested(object sender, Common.UI.ViewModel.Event.ChooseItemRequestedEventArgs e)
        {
            try
            {
                string opCat = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                                            "GU.TRUD", 
                                            "SENDING");
                TrudFacade.GetSendExportService(opCat).SendExport((TaskExport)e.Result);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        private void GenerateExports()
        {
            try
            {
                string opCat = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                                            "GU.TRUD", 
                                            "EXPORT", 
                                            Guid.NewGuid().ToString());
                var exportService = TrudFacade.GetExportService(opCat);
                var progressIndicator = new ProgressView
                    {
                        DataContext = new ProgressVM(exportService) { Title = "Экспорт данных" }, 
                        Owner = Application.Current.MainWindow
                    };
                using (var db = new TrudDbManager())
                {
                    exportService.ExportData(db);
                }
                progressIndicator.ShowDialog();

                if(exportService.ExportResult != null && exportService.ExportResult.IsFailed)
                {
                    NoticeUser.ShowError(exportService.ExportResult.Exception);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        #endregion

    }
}
