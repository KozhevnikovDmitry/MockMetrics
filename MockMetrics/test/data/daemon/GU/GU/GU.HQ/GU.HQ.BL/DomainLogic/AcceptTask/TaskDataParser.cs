using System;
using System.Collections.Generic;
using BLToolkit.EditableObjects;

using Common.BL.DictionaryManagement;
using GU.BL;
using GU.BL.Import;
using GU.DataModel;
using GU.HQ.BL.DomainLogic.AcceptTask.Interface;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.DomainLogic.AcceptTask
{
    /// <summary>
    /// Класс, занимающегося импортом сырых данных из заявок в данные сущностей предметной области "Постановка на учет в качестве нуждающегося в муниципальном жилье".
    /// </summary>
    [Obsolete("Этот класс надо полностью переписать под новый формат заявки")]
    public class TaskDataParser : ITaskDataParser
    {
        ///// <summary>
        ///// Импортер данных из заявки
        ///// </summary>
        //private readonly ITaskDataImporter _taskDataImporter;

        ///// <summary>
        ///// Менеджер спраовчников
        ///// </summary>
        //private readonly IDictionaryManager _dictionaryManager;

        ///// <summary>
        ///// Класс, занимающийся импортом сырых данных из заявок в данные сущностей предметной области Лицензирование.
        ///// </summary>
        ///// <param name="taskDataImporter">Импортер данных из заявки</param>
        ///// <param name="dictionaryManager">Менеджер спраовчников</param>
        //public TaskDataParser(ITaskDataImporter taskDataImporter, IDictionaryManager dictionaryManager)
        //{
        //    _taskDataImporter = taskDataImporter;
        //    _dictionaryManager = dictionaryManager;
        //}

        ///// <summary>
        ///// Получить адреса заявителя
        ///// </summary>
        ///// <param name="task"></param>
        ///// <returns></returns>
        //private EditableList<PersonAddress> GetDeclarerAddresses(Task task)
        //{
        //    var declarerAddress = PersonAddress.CreateInstance();
        //    var address = Address.CreateInstance();

        //    //---------------- Заявитель. Адрес проживания. 
        //    declarerAddress.AddressTypeId = AddressType.Residence;
        //    declarerAddress.DateStart = DateTime.Now;
        //    declarerAddress.DateStart = Convert.ToDateTime("31.12.2999");

        //    address.PostIndex = _taskDataImporter.GetSingleAttrValue("application", "applicant", "zip", task).StrValue;
        //    address.City = _taskDataImporter.GetSingleAttrValue("application", "applicant", "city", task).StrValue;
        //    address.Street = _taskDataImporter.GetSingleAttrValue("application", "applicant", "street", task).StrValue;
        //    address.HouseNum = _taskDataImporter.GetSingleAttrValue("application", "applicant", "house", task).StrValue;
        //    address.KorpNum = _taskDataImporter.GetSingleAttrValue("application", "applicant", "corps", task).StrValue;
        //    //                _taskDataImporter.GetSingleAttrValue("applicant", "address", "building", task).StrValue;  
        //    address.KvNum = _taskDataImporter.GetSingleAttrValue("application", "applicant", "flat", task).StrValue;

        //    declarerAddress.Address = address;

        //    return new EditableList<PersonAddress> { declarerAddress };
        //}

        ///// <summary>
        ///// Получить данные заявителя
        ///// </summary>
        ///// <param name="task"></param>
        ///// <returns></returns>
        //private Person GetDeclarer(Task task)
        //{
        //    var declarer = Person.CreateInstance();
            
        //    //------ Заявитель
        //    declarer.Name = _taskDataImporter.GetSingleAttrValue("application", "applicant", "name", task).StrValue;
        //    declarer.Sname = _taskDataImporter.GetSingleAttrValue("application", "applicant", "surname", task).StrValue;
        //    declarer.Patronymic = _taskDataImporter.GetSingleAttrValue("application", "applicant", "patronymic", task).StrValue;
        //    declarer.BirthDate = _taskDataImporter.GetSingleAttrValue("application", "applicant", "birthday", task).DateValue.Value;
        //    declarer.Sex = _taskDataImporter.GetSingleAttrValue("application", "applicant", "sex", task).StrValue.Equals("Женский") ? Sex.Female : Sex.Male;

        //    declarer.FioCurrent = String.Format("{0} {1} {2}", declarer.Name, declarer.Sname, declarer.Patronymic);

        //    declarer.Addresses = GetDeclarerAddresses(task);
        //    declarer.Documents = new EditableList<PersonDoc>();

        //    return declarer;
        //}

        ///// <summary>
        ///// Получитьстепень родтсва 
        ///// </summary>
        ///// <param name="relative"></param>
        ///// <returns></returns>
        //private int GetRelativeTtypeID(string relative)
        //{
        //    if (relative.Equals("супруг(а)"))
        //        return 1;

        //    if (relative.Equals("сын"))
        //        return 2;

        //    if (relative.Equals("дочь"))
        //        return 3;

        //    if (relative.Equals("отец"))
        //        return 4;

        //    if (relative.Equals("мать"))
        //        return 5;

        //    if (relative.Equals("признан членом семьи"))
        //        return 6;

        //    return 7;
        //}


        ///// <summary>
        ///// получить список родственников заявителя
        ///// </summary>
        ///// <returns></returns>
        //private EditableList<DeclarerRelative> GetDeclarerRelatives(Task task)
        //{
        //    var declarerRelatives = new EditableList<DeclarerRelative>();

        //    List<int?> sectNumbers = _taskDataImporter.GetSectionNumbers("application", "relative", task);
        //    foreach (var sectNum in sectNumbers)
        //    {
        //        Person person = Person.CreateInstance();
        //        person.Sname = _taskDataImporter.GetSingleAttrValue("application", "relative", sectNum, null, "surname", task).StrValue;
        //        person.Name = _taskDataImporter.GetSingleAttrValue("application", "relative", sectNum, null, "name", task).StrValue;
        //        person.Patronymic = _taskDataImporter.GetSingleAttrValue("application", "relative", sectNum, null, "patronymic", task).StrValue;

        //        var dateValue = _taskDataImporter.GetSingleAttrValue("application", "relative", sectNum, null, "birthday", task).DateValue;
        //        if (dateValue != null) person.BirthDate = dateValue.Value;
                
        //        DeclarerRelative relative = DeclarerRelative.CreateInstance();

        //        var relativeTypeName = _taskDataImporter.GetSingleAttrValue("application", "relative", sectNum, null, "relationToApplicant", task).StrValue;
        //        if (!string.IsNullOrEmpty(relativeTypeName)) relative.RelativeTypeId = GetRelativeTtypeID(relativeTypeName);
                
        //        relative.Person = person;

        //        if (!string.IsNullOrEmpty(person.Sname) && !string.IsNullOrEmpty(person.Name) && !string.IsNullOrEmpty(person.Patronymic)
        //            && dateValue != null && !string.IsNullOrEmpty(relativeTypeName)) 
        //        declarerRelatives.Add(relative);
        //    }

        //    return declarerRelatives;
        //}

        ///// <summary>
        ///// добавление элемента в список сонований учета, которые указал заявитель
        ///// </summary>
        ///// <param name="task"></param>
        ///// <param name="baseRegId">идентификатор из базы</param>
        ///// <param name="tagName">имя тэга</param>
        ///// <param name="declarerRegBaseItems"></param>
        //private void GetDeclarerRegBaseItem(Task task, int baseRegId, string tagName, EditableList<DeclarerBaseRegItem> declarerRegBaseItems)
        //{
        //    var boolValue = _taskDataImporter.GetSingleAttrValue("application", "applicant", tagName, task).BoolValue;
        //    if (boolValue != null && boolValue.Value)
        //    {
        //        var declarerRegBaseItem = DeclarerBaseRegItem.CreateInstance();
        //        declarerRegBaseItem.QueueBaseRegTypeId = baseRegId;

        //        declarerRegBaseItems.Add(declarerRegBaseItem);
        //    }
        //}

        ///// <summary>
        ///// Получить список оснований учета которые указал заявитель
        ///// </summary>
        ///// <param name="task"></param>
        ///// <returns></returns>
        //private DeclarerBaseReg GetDeclarerRegBase(Task task)
        //{
        //    var declarerRegBase = DeclarerBaseReg.CreateInstance();
        //    declarerRegBase.OtherBaseReg = _taskDataImporter.GetSingleAttrValue("application", "applicant", "other", task).StrValue;

        //    declarerRegBase.BaseRegItems = new EditableList<DeclarerBaseRegItem>();

        //    GetDeclarerRegBaseItem(task, 9, "noHouse", declarerRegBase.BaseRegItems);
        //    GetDeclarerRegBaseItem(task, 10, "squareBelowNorm", declarerRegBase.BaseRegItems);
        //    GetDeclarerRegBaseItem(task, 11, "livingRequirementInconsistency", declarerRegBase.BaseRegItems);
        //    GetDeclarerRegBaseItem(task, 12, "seriouslyIllRelative", declarerRegBase.BaseRegItems);

        //    return declarerRegBase;
        //}

        ///// <summary>
        ///// Устанавливаем статус заявления
        ///// </summary>
        ///// <returns>список истории заявлений</returns>
        //private EditableList<ClaimStatusHist> GetClaimStatusHistList()
        //{
        //    var claimStatusHist = new EditableList<ClaimStatusHist>();
        //    var claimStatus = ClaimStatusHist.CreateInstance();
        //    var uUser = GuFacade.GetDbUser();

        //    claimStatus.ClaimStatusTypeId = (int) ClaimStatusType.DataCheck;
        //    claimStatus.Date = DateTime.Now;
        //    claimStatus.UUserId = uUser.Id;

        //    claimStatusHist.Add(claimStatus);

        //    return claimStatusHist;
        //}

        /// <summary>
        /// Возвращает экземпляр Claim, заполненный по данным заявки.
        /// </summary>
        /// <param name="task">Объект Заявка</param>
        /// <returns>Экземпляр Claim, заполненный по данным заявки</returns>
        public Claim GetClaim(Task task)
        {
            throw new NotImplementedException();

            ////заявление
            //var claim = Claim.CreateInstance();

            //if (task.CreateDate != null) claim.ClaimDate = task.CreateDate.Value;
            //claim.CurrentStatusTypeId = ClaimStatusType.DataCheck;
            //claim.Task = task;
            //claim.Note = task.Note;
            //claim.AgencyId = task.AgencyId;
            //claim.Agency = task.Agency;


            //claim.Declarer = GetDeclarer(task);
            //claim.Relatives = GetDeclarerRelatives(task);
            //claim.DeclarerBaseReg = GetDeclarerRegBase(task);
            //claim.ClaimStatusHist = GetClaimStatusHistList();

            //// списки не могут быть null
            //claim.ClaimCategories = new EditableList<ClaimCategory>();
            //claim.Notices = new EditableList<Notice>();
            //claim.QueuePrivList = new EditableList<QueuePriv>();

            //return claim;
        }
    }
}
