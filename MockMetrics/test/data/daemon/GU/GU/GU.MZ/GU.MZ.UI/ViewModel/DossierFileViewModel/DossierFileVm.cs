using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.ValidationViewModel;
using Common.UI.WeakEvent;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SmartViewModel;
using GU.MZ.UI.ViewModel.SupervisionViewModel;
using GU.MZ.UI.ViewModel.SupervisionViewModel.Event;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    /// <summary>
    /// Класс ViewModel для окна ведения тома лицензионного дела
    /// </summary>
    public class DossierFileVm : SmartEditableVm<DossierFile>
    {
        private readonly SupervisionFacade _superviser;
        private readonly IDialogUiFactory _uiFactory;

        /// <summary>
        /// Слушатель слабых событий запроса на пересобирание vm'а редактирования тома
        /// </summary>
        private IWeakEventListener _vmRebuildListener;

        /// <summary>
        /// Класс ViewModel для окна ведения тома лицензионного дела
        /// </summary>
        public DossierFileVm(SupervisionFacade superviser, IDialogUiFactory uiFactory, FileSupervisionVm fileSupervisionVm, DossierFileOrdersVm dossierFileOrdersVm, DossierFileSourceVm dossierFileSourceVm)
        {
            _superviser = superviser;
            _uiFactory = uiFactory;
            FileSupervisionVm = fileSupervisionVm;
            DossierFileOrdersVm = dossierFileOrdersVm;
            DossierFileSourceVm = dossierFileSourceVm;
            AvalonInteractor.RegisterCaller(FileSupervisionVm);
            AvalonInteractor.RegisterCaller(DossierFileSourceVm);
            AvalonInteractor.RegisterCaller(DossierFileOrdersVm);
            if (_vmRebuildListener == null)
            {
                _vmRebuildListener = new WeakEventListener<ViewModelRebuildRequestedEventArgs>(OnRebuildRequsted);
            }
            VmRebuildRequestedWeakEventManager.AddListener(FileSupervisionVm, _vmRebuildListener);
        }

        protected override void Rebuild()
        {
            FileSupervisionVm.Initialize(Entity);
            DossierFileOrdersVm.Initialize(Entity);
            DossierFileSourceVm.Initialize(Entity);
        }

        #region EventHandling

        /// <summary>
        /// Пробрасывает событие запроса на пересобирание vm'а редактирования тома
        /// </summary>
        private void OnRebuildRequsted(object sender, ViewModelRebuildRequestedEventArgs eventArgs)
        {
            Entity = eventArgs.DossierFile;
            Rebuild();
            RaiseDisplayNameChanged(Entity.ToString());
            RaiseIsDirtyChanged();
        }

        #endregion

        #region Binding Properties

        public FileSupervisionVm FileSupervisionVm { get; private set; }

        public DossierFileSourceVm DossierFileSourceVm { get; private set; }

        public DossierFileOrdersVm DossierFileOrdersVm { get; private set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Сохраняет изменения в редактируемом томе.
        /// </summary>
        protected override void Save()
        {
            try
            {
                var validationResults = Validate();
                if (!validationResults.IsValid)
                {
                    _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResults.Errors), "Ошибочно заполненные поля");
                    FileSupervisionVm.RaiseIsValidChanged();
                    return;
                }
                if (FileSupervisionVm.IsSpecificMapping)
                {
                    Entity = FileSupervisionVm.SaveDossierFile();
                }
                else
                {
                    _editableFacade.Save<DossierFile>(this);
                }
                _superviser.Initialize(Entity);
                Rebuild();
                RaiseDisplayNameChanged(Entity.ToString());
                RaiseIsDirtyChanged();
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new VMException("Непредвиденная ошибка", ex));
            }
        }

        protected override void OnClose()
        {
            FileSupervisionVm.Dispose();
            base.OnClose();
        }

        #endregion
    }
}
