using Common.DA.Interface;
using GU.Trud.BL.Export.Event;

namespace GU.Trud.BL.Export.Interface
{
    /// <summary>
    /// ��������� ��� �������-����� �������� ������.
    /// </summary>
    public interface IGenerateExportService
    {
        /// <summary>
        /// ��������� ������� ������.
        /// </summary>
        /// <param name="dbManager">�������� ������</param>
        void ExportData(IDomainDbManager dbManager);

        /// <summary>
        /// ������� ����������� � ��������� �������� ��������.
        /// </summary>
        event PercentageProgressDelegate ExportProgressed;

        /// <summary>
        /// ���������� ������ �������� ������.
        /// </summary>
        void Cancel();

        /// <summary>
        /// ��������� ��������.
        /// </summary>
        IExportResult ExportResult { get; }
    }
}