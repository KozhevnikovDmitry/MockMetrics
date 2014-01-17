using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Person;

namespace PostGrad.Core.BL
{
    public interface IDossierFileBuilder
    {
        /// <summary>
        /// ����� ������, ��� ������� ����� ����������� ���
        /// </summary>
        /// <param name="task">������ ������</param>
        /// <returns>������� ����</returns>
        IDossierFileBuilder FromTask(Task task);

        /// <summary>
        /// ��������� ��������������� ����� �����.
        /// </summary>
        /// <param name="inventoryRegNumber">��������������� ����� �����</param>
        /// <returns>������� ����</returns>
        IDossierFileBuilder WithInventoryRegNumber(int? inventoryRegNumber);

        /// <summary>
        /// ��������� ������������� ����������.
        /// </summary>
        /// <param name="responsibleEmployee">������������ ���������</param>
        /// <returns>������� ����</returns>
        IDossierFileBuilder ToEmployee(Employee responsibleEmployee);

        /// <summary>
        /// ��������� ������ "������� � ������������"
        /// </summary>
        /// <param name="notice">����������� � �������</param>
        /// <returns>������� ����</returns>
        IDossierFileBuilder WithAcceptedStatus(string notice);

        /// <summary>
        /// ��������� ����������� ��������
        /// </summary>
        /// <param name="documentName">��� ���������</param>
        /// <param name="quantity">����������</param>
        /// <returns>������� ����</returns>
        /// <exception cref="CantAddProvidedDocumentWithEmptyNameException"></exception>
        /// <exception cref="CantAddProvidedDocumentWithNegativeQuantityException"></exception>
        IDossierFileBuilder AddProvidedDocument(string documentName, int quantity);

        /// <summary>
        /// ��������� ������ "����������"
        /// </summary>
        /// <param name="notice">����������� � �������</param>
        /// <returns>������� ����</returns>
        IDossierFileBuilder WithRejectedStatus(string notice);

        /// <summary>
        /// ���������� ��������� ������ ��� ������������� ����
        /// </summary>
        /// <returns> ��������� ���</returns>
        /// <exception cref="BuildingDataNotCompleteException">������������ ������ ��� ����� ��� ���������� ������</exception>
        /// <exception cref="CantSetStatusException">���������� ���������� ������ ��� ������</exception>
        DossierFile Build();
    }
}