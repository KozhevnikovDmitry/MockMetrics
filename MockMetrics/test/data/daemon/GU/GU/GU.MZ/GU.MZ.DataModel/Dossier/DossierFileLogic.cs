using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.Types;
using Common.Types.Exceptions;
using GU.DataModel;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Violation;

namespace GU.MZ.DataModel.Dossier
{
    /// <summary>
    /// Дополнительная возможности тома по манипулированию собственными данными
    /// </summary>
    public partial class DossierFile
    {
        #region Proxy Properties

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual DossierFileScenarioStep CurrentFileStep
        {
            get
            {
                return DossierFileStepList.SingleOrDefault(s => s.ScenarioStepId == CurrentScenarioStepId);
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual ScenarioStep CurrentScenarioStep
        {
            get
            {
                return Scenario.ScenarioStepList.Single(s => s.Id == CurrentScenarioStepId);
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual Service Service
        {
            get
            {
                if (Task == null)
                {
                    return null;
                }
                return Task.Service;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual int ServiceGroupId
        {
            get
            {
                if (Task == null || Task.Service == null)
                {
                    return 0;
                }
                return Task.Service.ServiceGroup.Id;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual bool IsLinkaged
        {
            get
            {
                if (LicenseDossier != null &&
                    LicenseDossier.LicenseHolder != null &&
                    HolderRequisites != null)
                {
                    return true;
                }

                return false;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual bool IsDossierNew
        {
            get
            {
                if (LicenseDossier == null)
                {
                    return true;
                }

                return LicenseDossier.PersistentState == PersistentState.New;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual bool IsHolderNew
        {
            get
            {
                if (LicenseDossier == null || LicenseDossier.LicenseHolder == null)
                {
                    return true;
                }

                return LicenseDossier.LicenseHolder.PersistentState == PersistentState.New;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual bool IsLinkagedWithNewRequisites
        {
            get
            {
                if (LicenseDossier == null ||
                    LicenseDossier.LicenseHolder == null ||
                    LicenseDossier.LicenseHolder.RequisitesList == null)
                {
                    return true;
                }

                return LicenseDossier.LicenseHolder.RequisitesList.IsDirty;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual ScenarioType ScenarioType
        {
            get { return Scenario.ScenarioType; }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string EmployeeName
        {
            get
            {
                if (Employee == null)
                {
                    return null;
                }
                return Employee.Name;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string EmployeePosition
        {
            get
            {
                if (Employee == null)
                {
                    return null;
                }
                return Employee.Position;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string EmployeeContacts
        {
            get
            {
                if (Employee == null)
                {
                    return null;
                }
                return string.Format("{0}, {1}", Employee.Phone, Employee.Email);
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string LicensedActivityName
        {
            get
            {
                if (LicensedActivity == null)
                {
                    return null;
                }
                return LicensedActivity.Name;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string HolderInn
        {
            get
            {
                if (HolderRequisites == null || HolderRequisites.LicenseHolder == null)
                {
                    return null;
                }
                return HolderRequisites.LicenseHolder.Inn;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string HolderOgrn
        {
            get
            {
                if (HolderRequisites == null || HolderRequisites.LicenseHolder == null)
                {
                    return null;
                }
                return HolderRequisites.LicenseHolder.Ogrn;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string HolderFullName
        {
            get
            {
                if (HolderRequisites == null)
                {
                    return null;
                }
                return HolderRequisites.FullName;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string HolderShortName
        {
            get
            {
                if (HolderRequisites == null)
                {
                    return null;
                }
                return HolderRequisites.ShortName;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string HolderFirmName
        {
            get
            {
                if (HolderRequisites == null)
                {
                    return null;
                }
                return HolderRequisites.FirmName;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string HolderAddress
        {
            get
            {
                if (HolderRequisites == null && HolderRequisites.Address == null)
                {
                    return null;
                }
                return HolderRequisites.Address.ToLongString();
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual DateTime? TaskStamp
        {
            get
            {
                if (Task == null)
                {
                    return null;
                }
                return Task.CreateDate;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string TaskStampShortString
        {
            get
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }

                if (!Task.CreateDate.HasValue)
                {
                    throw new BLLException("Дата заведения заявки для тома не указана");
                }


                return Task.CreateDate.Value.ToShortDateString();
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string TaskCustomerEmail
        {
            get
            {
                if (Task == null)
                {
                    return null;
                }
                return Task.CustomerEmail;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual TaskStatusType TaskState
        {
            get
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }
                return Task.CurrentState;
            }
            set
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }
                Task.CurrentState = value;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual string LicenseRegNumber
        {
            get
            {
                if (License == null)
                {
                    return null;
                }
                return License.RegNumber;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual DateTime? LicenseGrantDate
        {
            get
            {
                if (License == null)
                {
                    return null;
                }
                return License.GrantDate;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual bool IsRejected
        {
            get
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }

                return TaskState == TaskStatusType.Rejected;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual bool IsNewLicense
        {
            get
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }

                return Task.ServiceId == 1 || Task.ServiceId == 6 || Task.ServiceId == 11;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual bool IsRenewalLicense
        {
            get
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }

                return Task.ServiceId == 2 || Task.ServiceId == 7 || Task.ServiceId == 12;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual bool IsStopLicense
        {
            get
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }

                return Task.ServiceId == 3 || Task.ServiceId == 8 || Task.ServiceId == 13;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual bool IsDone
        {
            get
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }

                return Task.CurrentState == TaskStatusType.Done;
            }
        }

        [MapIgnore]
        [NoInstance]
        [CloneIgnore]
        public virtual bool IsReady
        {
            get
            {
                if (Task == null)
                {
                    throw new BLLException("Заявка для тома не найдена");
                }

                return Task.CurrentState == TaskStatusType.Ready;
            }
        }

        #endregion


        #region Orders

        public void CollectOrders()
        {
            try
            {
                SubscribeToOrders();
                var list = new List<IOrder>();
                list.AddRange(DossierFileStepList.Select(t => t.InspectionOrder));
                list.AddRange(DossierFileStepList.Select(t => t.ExpertiseOrder));
                list.AddRange(DossierFileStepList.SelectMany(t => t.StandartOrderList));
                list.RemoveAll(order => order == null);
                Orders = new ObservableCollection<IOrder>(list.OrderBy(t => t.Stamp));
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка формирования списка приказов тома.", ex);
            }
        }

        private void SubscribeToOrders()
        {
            foreach (var step in DossierFileStepList)
            {
                step.PropertyChanged += (sender, args) =>
                {
                    var hostStep = sender as DossierFileScenarioStep;
                    if (args.PropertyName == Util.GetPropertyName(() => hostStep.InspectionOrder))
                    {
                        var removeItem = Orders.SingleOrDefault(t => t.Id == hostStep.Id);

                        if (hostStep.InspectionOrder == null)
                        {
                            if (removeItem == null)
                            {
                                throw new BLLException("Удаляемый приказ в списке не найден");
                            }

                            Orders.Remove(removeItem);
                        }
                        else
                        {
                            if (removeItem != null)
                            {
                                Orders.Remove(removeItem);
                            }

                            Orders.Add(hostStep.InspectionOrder);
                        }
                    }

                    if (args.PropertyName == Util.GetPropertyName(() => step.ExpertiseOrder))
                    {
                        var removeItem = Orders.SingleOrDefault(t => t.Id == hostStep.Id);

                        if (hostStep.ExpertiseOrder == null)
                        {
                            if (removeItem == null)
                            {
                                throw new BLLException("Удаляемый приказ в списке не найден");
                            }

                            Orders.Remove(removeItem);
                        }
                        else
                        {
                            if (removeItem != null)
                            {
                                Orders.Remove(removeItem);
                            }

                            Orders.Add(hostStep.ExpertiseOrder);
                        }
                    }
                };

                step.StandartOrderList.CollectionChanged += (sender, args) =>
                {
                    if (args.NewItems != null)
                    {
                        foreach (var order in args.NewItems.Cast<IOrder>())
                        {
                            if (Orders.Contains(order))
                            {
                                throw new BLLException("Добавлямый приказ уже находится в списке");
                            }

                            Orders.Add(order);
                        }
                    }

                    if (args.OldItems != null)
                    {
                        foreach (var order in args.OldItems.Cast<IOrder>())
                        {
                            if (!Orders.Contains(order))
                            {
                                throw new BLLException("Удаляемый приказ в списке не найден");
                            }
                            Orders.Remove(order);
                        }
                    }
                };
            }
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public ObservableCollection<IOrder> Orders { get; private set; }

        #endregion


        #region Methods

        #region Get Step Entities

        public virtual void RaiseIsDirtyChanged()
        {
            OnPropertyChanged(Util.GetPropertyName(() => IsDirty));
        }

        public virtual ViolationNotice StepViolationNotice(ScenarioStep step)
        {
            return GetStep(step) == null ? null : GetStep(step).ViolationNotice;
        }

        public virtual Notice StepNotice(ScenarioStep step)
        {
            return GetStep(step) == null ? null : GetStep(step).Notice;
        }

        public virtual ViolationResolveInfo StepViolationResolveInfo(ScenarioStep step)
        {
            return GetStep(step) == null ? null : GetStep(step).ViolationResolveInfo;
        }

        public virtual DocumentExpertise StepDocumentExpertise(ScenarioStep step)
        {
            return GetStep(step) == null ? null : GetStep(step).DocumentExpertise;
        }

        public virtual Inspection StepInspection(ScenarioStep step)
        {
            return GetStep(step) == null ? null : GetStep(step).Inspection;
        }

        public virtual StandartOrder StepStandartOrder(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                return null;
            }

            return fileStep.StepStandartOrder();
        }

        public virtual ExpertiseOrder StepExpertiseOrder(ScenarioStep step)
        {
            return GetStep(step) == null ? null : GetStep(step).ExpertiseOrder;
        }

        public virtual InspectionOrder StepInspectionOrder(ScenarioStep step)
        {
            return GetStep(step) == null ? null : GetStep(step).InspectionOrder;
        }

        public virtual DossierFileScenarioStep GetStep(ScenarioStep step)
        {
            return DossierFileStepList.SingleOrDefault(s => s.ScenarioStepId == step.Id);
        }

        public virtual DossierFileScenarioStep PreviousStep(ScenarioStep step)
        {
            var fileStep = DossierFileStepList.SingleOrDefault(s => s.ScenarioStepId == step.Id);
            if (fileStep != null)
            {
                var index = DossierFileStepList.IndexOf(fileStep);
                if (index > 0)
                {
                    return DossierFileStepList[DossierFileStepList.IndexOf(fileStep) - 1];
                }
            }

            return null;
        }

        public virtual ViolationNotice GetViolationNotice()
        {
            if (DossierFileStepList == null)
            {
                throw new BLLException("Список этапов ведения не заполнен для тома");
            }

            var notices = DossierFileStepList.Select(t => t.ViolationNotice).Where(t => t != null).ToList();

            if (notices.Count() > 1)
            {
                throw new BLLException("Найдено более одного уведомления о нарушениях для тома");
            }

            return notices.SingleOrDefault();
        }

        public virtual StandartOrder GetStandartOrderOfType(StandartOrderType orderType)
        {
            if (DossierFileStepList == null)
            {
                throw new BLLException("Список этапов ведения не заполнен для тома");
            }

            var orders = DossierFileStepList.Where(t => t.StandartOrderList != null)
                                            .SelectMany(t => t.StandartOrderList)
                                            .Where(t => t.OrderOption != null)
                                            .Where(t => t.OrderType == orderType)
                                            .ToList();

            if (orders.Count() > 1)
            {
                throw new BLLException(string.Format("Найдено более одного приказа типа [{0}] для тома", orderType.GetDescription()));
            }

            return orders.SingleOrDefault();
        }

        #endregion


        #region Remove Step Entities

        public virtual void RemoveStepNotice(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                throw new BLLException(string.Format("Этап ведения тома [{0}] не найден", step.Name));
            }

            var child = fileStep.Notice;
            fileStep.AddToDelete(child);
            fileStep.Notice = null;
        }

        public virtual void RemoveStepViolationNotice(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                throw new BLLException(string.Format("Этап ведения тома [{0}] не найден", step.Name));
            }
            var child = fileStep.ViolationNotice;
            fileStep.AddToDelete(child);
            fileStep.ViolationNotice = null;
        }

        public virtual void RemoveStepViolationResolveInfo(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                throw new BLLException(string.Format("Этап ведения тома [{0}] не найден", step.Name));
            }

            var child = fileStep.ViolationResolveInfo;
            fileStep.AddToDelete(child);
            fileStep.ViolationResolveInfo = null;
        }

        public virtual void RemoveStepDocumentExpertise(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                throw new BLLException(string.Format("Этап ведения тома [{0}] не найден", step.Name));
            }

            var child = fileStep.DocumentExpertise;
            fileStep.AddToDelete(child);
            fileStep.DocumentExpertise = null;
        }

        public virtual void RemoveStepInspection(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                throw new BLLException(string.Format("Этап ведения тома [{0}] не найден", step.Name));
            }

            var child = fileStep.Inspection;
            fileStep.AddToDelete(child);
            fileStep.Inspection = null;
        }

        public virtual void RemoveStepStandartOrder(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                throw new BLLException(string.Format("Этап ведения тома [{0}] не найден", step.Name));
            }

            var child = fileStep.StepStandartOrder();
            fileStep.StandartOrderList.Remove(child);
        }

        public virtual void RemoveStepExpertiseOrder(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                throw new BLLException(string.Format("Этап ведения тома [{0}] не найден", step.Name));
            }

            var child = fileStep.ExpertiseOrder;
            fileStep.AddToDelete(child);
            fileStep.ExpertiseOrder = null;
        }

        public virtual void RemoveStepInspectionOrder(ScenarioStep step)
        {
            var fileStep = GetStep(step);
            if (fileStep == null)
            {
                throw new BLLException(string.Format("Этап ведения тома [{0}] не найден", step.Name));
            }

            var child = fileStep.InspectionOrder;
            fileStep.AddToDelete(child);
            fileStep.InspectionOrder = null;
        }

        #endregion

        #endregion


        #region Delete Cache

        public virtual void RemoveStepServiceResult()
        {
            if (DossierFileServiceResult == null)
            {
                throw new BLLException("Резульат ведения тома не найден");
            }

            AddToDelete(DossierFileServiceResult);
            DossierFileServiceResult = null;
        }

        public virtual void AddToDelete(DossierFileServiceResult dossierFileServiceResult)
        {
            if (dossierFileServiceResult.PersistentState == PersistentState.Old ||
                dossierFileServiceResult.PersistentState == PersistentState.NewDeleted)
            {
                DeleteServiceResults.Add(dossierFileServiceResult);
            }
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public List<DossierFileServiceResult> DeleteServiceResults { get; set; }
        
        #endregion
    }
}
