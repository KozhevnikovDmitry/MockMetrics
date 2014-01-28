using System;
using Common.UI.ViewModel.AvalonInteraction.InteractionEvents;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.AvalonInteraction
{
    /// <summary>
    ///  ласс, предназанченный дл€ конечной обработки - открытие вкладок - событий взаимодействи€ с AvalonDockVM
    /// </summary>
    /// <remarks>
    ///  ласс использует ссылку на IAvalonDockVM и сам обрабатывает запросы на открытие вкладок.
    /// ¬ случае, если вкладка должна быть открыта на другом IAvalonDockVM 
    /// - это когда выставлен флажок IsInterHost в аргументах событи€ -
    /// —обытие таки возбждаетс€ снова. ≈го обработка ложитс€ на ViewModel верхнего уровн€.
    /// </remarks>
    public class UltimateAvalonDockInteractor : BaseAvalonDockInteractor
    {
        /// <summary>
        /// –одительский VM, на котором должныоткрыватьс€ вкладки.
        /// </summary>
        private readonly IAvalonDockVM _avalonDockVM;

        /// <summary>
        ///  ласс, предназанченный дл€ конечной обработки - открытие вкладок - событий взаимодействи€ с AvalonDockVM
        /// </summary>
        /// <param name="avalonDockVM">–одительский ViewModel</param>
        public UltimateAvalonDockInteractor(IAvalonDockVM avalonDockVM)
            : base(avalonDockVM)
        {
            _avalonDockVM = avalonDockVM;
        }

        #region Event Raisers

        public override void RaiseOpenDockable(object sender, OpenSimpleDockableEventArgs e)
        {
            if (e.IsInterHost)
            {
                base.RaiseOpenDockable(this, e);
                return;
            }

            _avalonDockVM.AddDockable(e.DisplayName, e.View);
        }

        public override void RaiseOpenSearchDockable(object sender, OpenSearchDockableEventArgs e)
        {
            if (e.IsInterHost)
            {
                base.RaiseOpenSearchDockable(this, e);
                return;
            }

            if (e.SearchVm != null)
            {
                _avalonDockVM.AddSearchDockable(e.DisplayName, e.SearchVm);
            }
            else
            {
                _avalonDockVM.AddSearchDockable(e.DisplayName, e.DomainType);
            }
        }

        public override void RaiseOpenEditableDockable(object sender, OpenEditableDockableEventArgs e)
        {
            try
            {
                if (e.IsInterHost)
                {
                    base.RaiseOpenEditableDockable(this, e);
                    return;
                }

                if (!_avalonDockVM.IsAlreadyOpenned(e.Entity, true))
                {
                    _avalonDockVM.AddEditableDockable(e.DisplayName, e.Entity, e.DomainType);
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        public override void RaiseOpenReportDockable(object sender, OpenReportDockableEventArgs e)
        {

            if (e.IsInterHost)
            {
                base.RaiseOpenReportDockable(this, e);
                return;
            }

            _avalonDockVM.AddReportDockable(e.DisplayName, e.Report);
        }

        public override void RaiseCloseEditableDockable(object sender, ManageEditableDockableEventArgs e)
        {
            if (e.IsInterHost)
            {
                base.RaiseCloseEditableDockable(this, e);
                return;
            }

            if (_avalonDockVM.IsAlreadyOpenned(e.EntityId.ToString(), e.DomainType, false))
            {
                _avalonDockVM.CloseEditableDockable(e.EntityId.ToString(), e.DomainType);
            }
        }

        public override void RaiseEditableDisplayNameChanged(object sender, ManageEditableDockableEventArgs e)
        {
            if (e.IsInterHost)
            {
                base.RaiseEditableDisplayNameChanged(this, e);
                return;
            }

            if (_avalonDockVM.IsAlreadyOpenned(e.EntityId.ToString(), e.DomainType, false))
            {
                _avalonDockVM.DisplayNameChanged(e.EntityId.ToString(), e.DomainType);
            }
        }

        #endregion
    }
}