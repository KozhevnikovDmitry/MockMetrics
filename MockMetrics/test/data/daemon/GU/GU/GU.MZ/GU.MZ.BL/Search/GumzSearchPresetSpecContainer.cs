using System;
using System.Collections.Generic;

using Common.BL.Search.SearchSpecification;
using Common.Types;

using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Search
{
    /// <summary>
    /// ����� ��������� �������� ���������� ������� ��������������
    /// </summary>
    [Serializable]
    public class GumzSearchPresetSpecContainer : SearchPresetSpecContainer
    {
        /// <summary>
        /// ����� ��������� �������� ���������� ������� ��������������
        /// </summary>
        public GumzSearchPresetSpecContainer()
        {
            PresetSpecList = new List<SearchPresetSpec>
                {
                    CreateLicensePreset(),
                    CreateLicenseHolderPreset(),
                    CreateLicenseDossierPreset(),
                    CreateDossierFilePreset(),
                    CreateEmployeePreset(),
                    CreateExpertPreset()
                };
            LastUpdate = new DateTime(2014, 01, 10, 13, 0, 0);
        }

        /// <summary>
        /// ������ ������ ������ ���������.
        /// </summary>
        /// <returns>������ ��� ���������</returns>
        private SearchPresetSpec CreateExpertPreset()
        {
            var expertPreset = new PresetSpec<Expert>();

            var expert = Expert.CreateInstance();
            
            var accDocNumNameIncludes = new PresetSpecExpression(
                Util.GetPropertyName(() => expert.AccreditateDocumentNumber), SearchConditionType.Includes, typeof(Expert));

            expertPreset.PresetSpecDets.Add(accDocNumNameIncludes);

            return expertPreset;
        }

        /// <summary>
        /// ������ ������ ������ ��������.
        /// </summary>
        /// <returns>������ ��� ��������</returns>
        private SearchPresetSpec CreateLicensePreset()
        {
            var licensePreset = new PresetSpec<License>();

            var license = License.CreateInstance();
            var licenseRequisites = LicenseRequisites.CreateInstance();

            var regNumberIncludes = new PresetSpecExpression(
                Util.GetPropertyName(() => license.RegNumber), SearchConditionType.Includes, typeof(License));
            

            var blankNumberIncludes = new PresetSpecExpression(
                Util.GetPropertyName(() => license.BlankNumber), SearchConditionType.Includes, typeof(License));

            licensePreset.PresetSpecDets.Add(regNumberIncludes);
            licensePreset.PresetSpecDets.Add(blankNumberIncludes);

            licensePreset.AddOrderField(
                typeof(License), Util.GetPropertyName(() => license.GrantDate), OrderDirection.Descending);

            return licensePreset;
        }

        /// <summary>
        /// ������ ������ ������ �����������.
        /// </summary>
        /// <returns>������ ��� �����������</returns>
        private SearchPresetSpec CreateLicenseHolderPreset()
        {
            var licenseHolderPreset = new PresetSpec<LicenseHolder>();

            var licenseHolder = LicenseHolder.CreateInstance();

            var innIncludes = new PresetSpecExpression(
                Util.GetPropertyName(() => licenseHolder.Inn), SearchConditionType.Includes, typeof(LicenseHolder));
            var ogrnIncludes = new PresetSpecExpression(
                Util.GetPropertyName(() => licenseHolder.Ogrn), SearchConditionType.Includes, typeof(LicenseHolder));

            licenseHolderPreset.PresetSpecDets.Add(innIncludes);
            licenseHolderPreset.PresetSpecDets.Add(ogrnIncludes);

            return licenseHolderPreset;
        }

        /// <summary>
        /// ������ ������ ������ ������������ ���.
        /// </summary>
        /// <returns>������ ��� ������������ ���</returns>
        private SearchPresetSpec CreateLicenseDossierPreset()
        {
            var licenseDossierPreset = new PresetSpec<LicenseDossier>();

            var licenseDossier = LicenseDossier.CreateInstance();

            var regNumberIncludes = new PresetSpecExpression(
                Util.GetPropertyName(() => licenseDossier.RegNumber), SearchConditionType.Includes, typeof(LicenseDossier));

            licenseDossierPreset.PresetSpecDets.Add(regNumberIncludes);

            licenseDossierPreset.AddOrderField(
                typeof(LicenseDossier), Util.GetPropertyName(() => licenseDossier.CreateDate), OrderDirection.Descending);

            return licenseDossierPreset;
        }

        /// <summary>
        /// ������ ������ ������ ����� ���������������.
        /// </summary>
        /// <returns>������ ��� ����� ������������ ���</returns>
        private SearchPresetSpec CreateDossierFilePreset()
        {
            var dossierFilePreset = new PresetSpec<DossierFile>();

            var dossierFile = DossierFile.CreateInstance();

            var regNumberEquals = new PresetSpecExpression(
                Util.GetPropertyName(() => dossierFile.RegNumber), SearchConditionType.Equals, typeof(DossierFile));
            var taskIdEquals = new PresetSpecExpression(
                Util.GetPropertyName(() => dossierFile.TaskId), SearchConditionType.Equals, typeof(DossierFile));

            dossierFilePreset.PresetSpecDets.Add(regNumberEquals);
            dossierFilePreset.PresetSpecDets.Add(taskIdEquals);

            dossierFilePreset.AddOrderField(
                typeof(DossierFile), Util.GetPropertyName(() => dossierFile.CreateDate), OrderDirection.Descending);

            return dossierFilePreset;
        }

        /// <summary>
        /// ������ ������ ������ �����������.
        /// </summary>
        /// <returns>������ ��� �����������</returns>
        private SearchPresetSpec CreateEmployeePreset()
        {
            var employeePreset = new PresetSpec<Employee>();

            var employee = Employee.CreateInstance();
            var dbUser = DbUser.CreateInstance();

            var fioIncludes = new PresetSpecExpression(
                Util.GetPropertyName(() => dbUser.UserText), SearchConditionType.Includes, typeof(DbUser));
            var phoneIncludes = new PresetSpecExpression(
                Util.GetPropertyName(() => employee.Phone), SearchConditionType.Includes, typeof(Employee));
            var emailEquals = new PresetSpecExpression(
                Util.GetPropertyName(() => employee.Email), SearchConditionType.Includes, typeof(Employee));

            employeePreset.PresetSpecDets.Add(fioIncludes);
            employeePreset.PresetSpecDets.Add(phoneIncludes);
            employeePreset.PresetSpecDets.Add(emailEquals);

            return employeePreset;
        }

    }
}