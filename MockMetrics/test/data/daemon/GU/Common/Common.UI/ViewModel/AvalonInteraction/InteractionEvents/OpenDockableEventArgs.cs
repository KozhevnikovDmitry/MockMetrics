using System;
using System.Windows.Controls;

using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.AvalonInteraction.InteractionEvents
{
    /// <summary>
    /// ������� ����� ���������� ��� ������� �������������� AvalonDock.
    /// </summary>
    public abstract class AvalonInteractEventArgs : EventArgs
    {
        /// <summary>
        /// ���� ����������� �� ��, ��� ������� ������ ���� ������� �� AvalonDockHost �� ������ �������(����������).
        /// </summary>
        public bool IsInterHost { get; set; }
    }

    /// <summary>
    /// ������� ����� ���������� ��� ������� �������� ������� �� AvalonDock.
    /// </summary>
    public abstract class OpenDockableEventArgs : AvalonInteractEventArgs
    {
        /// <summary>
        /// ������������ ��� �������.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// ��������� �������, ������������ � ������������� �������� �������.
        /// </summary>
        /// <param name="displayName">������������ ��� �������</param>
        /// <param name="isInterHost"> </param>
        protected OpenDockableEventArgs(string displayName, bool isInterHost)
        {
            this.DisplayName = displayName;
            this.IsInterHost = isInterHost;
        }
    }

    /// <summary>
    /// ��������� �������, ������������ � ������������� �������� �������.
    /// </summary>
    public class OpenSimpleDockableEventArgs : OpenDockableEventArgs
    {
        /// <summary>
        /// View, ������� ������ ���� ������� �� ������� AvalonDock.
        /// </summary>
        public UserControl View { get; private set; }

        /// <summary>
        /// ��������� �������, ������������ � ������������� �������� �������.
        /// </summary>
        /// <param name="view">View, ������� ������ ���� ������� �� ������� AvalonDock</param>
        /// <param name="displayName">������������ ��� �������</param>
        public OpenSimpleDockableEventArgs(UserControl view, string displayName, bool isInterHost)
            : base(displayName, isInterHost)
        {
            this.View = view;
        }
    }

    /// <summary>
    /// ��������� �������, ������������ � ������������� �������� ������� ������.
    /// </summary>
    public class OpenSearchDockableEventArgs : OpenDockableEventArgs
    {
        /// <summary>
        /// ViewModel ��� ������� ������.
        /// </summary>
        public ISearchVM SearchVm { get; set; }

        /// <summary>
        /// �������� ��� ��������.
        /// </summary>
        public Type DomainType { get; set; }

        /// <summary>
        /// ��������� �������, ������������ � ������������� �������� ������� ������.
        /// </summary>
        /// <param name="displayName">������������ ��� �������</param>
        /// <param name="domainType">�������� ��� ��������</param>
        /// <param name="isInterHost"> </param>
        public OpenSearchDockableEventArgs(string displayName, Type domainType, bool isInterHost)
            : base(displayName, isInterHost)
        {
            this.DomainType = domainType;
        }

        /// <summary>
        /// ��������� �������, ������������ � ������������� �������� ������� ������.
        /// </summary>
        /// <param name="displayName">������������ ��� �������</param>
        /// <param name="searchVm">ViewModel ��� ������� ������</param>
        /// <param name="isInterHost"></param>
        public OpenSearchDockableEventArgs(string displayName, ISearchVM searchVm, bool isInterHost)
            : base(displayName, isInterHost)
        {
            this.SearchVm = searchVm;
        }
    }

    /// <summary>
    /// ��������� �������, ������������ � ������������� �������� ������� ��������������.
    /// </summary>
    public class OpenEditableDockableEventArgs : OpenDockableEventArgs
    {
        /// <summary>
        /// �������� ��� ��������������
        /// </summary>
        public IDomainObject Entity { get; set; }

        /// <summary>
        /// �������� ��� ��������
        /// </summary>
        public Type DomainType { get; set; }

        /// <summary>
        /// ��������� �������, ������������ � ������������� �������� ������� ��������������.
        /// </summary>
        /// <param name="displayName">������������ ��� �������</param>
        /// <param name="entity">�������� ��� ��������������</param>
        /// <param name="domainType">�������� ��� ��������</param>
        /// <param name="isInterHost"> </param>
        public OpenEditableDockableEventArgs(string displayName, IDomainObject entity, Type domainType, bool isInterHost)
            : base(displayName, isInterHost)
        {
            this.Entity = entity;
            this.DomainType = domainType;
        }
    }

    /// <summary>
    /// ��������� �������, ������������ � ������������� �������� ������� � �������.
    /// </summary>
    public class OpenReportDockableEventArgs : OpenDockableEventArgs
    {
        /// <summary>
        /// �����
        /// </summary>
        public IReport Report { get; set; }

        /// <summary>
        /// ���� ������ ��������� ��� ������
        /// </summary>
        public bool IsDesigner { get; set; }

        /// <summary>
        /// ��������� �������, ������������ � ������������� �������� ������� � �������.
        /// </summary>
        /// <param name="displayName">������������ ��� �������</param>
        /// <param name="report">������ �����</param>
        /// <param name="isDesigner">���� ������ ��������� ��� ������</param>
        /// <param name="isInterHost"> </param>
        public OpenReportDockableEventArgs(string displayName, IReport report, bool isDesigner, bool isInterHost)
            : base(displayName, isInterHost)
        {
            Report = report;
            this.IsDesigner = isDesigner;
        }
    }

    /// <summary>
    /// ��������� �������, ������������ ����� ���������(��������, ������) ������� ��������������.
    /// </summary>
    public class ManageEditableDockableEventArgs : AvalonInteractEventArgs
    {
        public int EntityId { get; set; }

        public Type DomainType { get; set; }

        public ManageEditableDockableEventArgs(int entityId, Type domainType, bool isInterHost)
        {
            EntityId = entityId;
            DomainType = domainType;
            IsInterHost = isInterHost;
        }
    }
}