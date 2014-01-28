using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.ViewModel.Interfaces;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.EmployeeViewModel;
using GU.MZ.UI.ViewModel.SmartViewModel;
using GU.MZ.UI.ViewModel.SupervisionViewModel.Interface;

namespace GU.MZ.UI
{
    public interface ISupervisionViewFacotry
    {
        Dictionary<UserControl, ISupervisionStepVm> ResolveSupervisionSteps();
    }

    public class SupervisionViewFacotry : ISupervisionViewFacotry
    {
        private readonly ILifetimeScope _context;

        public SupervisionViewFacotry(ILifetimeScope context)
        {
            _context = context;
        }

        public Dictionary<UserControl, ISupervisionStepVm> ResolveSupervisionSteps()
        {
            try
            {
                var superviser = _context.Resolve<SupervisionFacade>();

                var result = new Dictionary<UserControl, ISupervisionStepVm>();
                var stepList = superviser.DossierFile.Scenario.ScenarioStepList.OrderBy(t => t.SortOrder).ToList();
                int i = 0;
                foreach (var scenarioStep in stepList)
                {
                    i++;
                    var view = _context.ResolveNamed<UserControl>(scenarioStep.Id.ToString());
                    var viewModel = _context.ResolveNamed<ISupervisionStepVm>(scenarioStep.Id.ToString());
                    viewModel.BaseInit(_context.Resolve<IDialogUiFactory>(), _context.Resolve<ChooseResponsibleEmployeeVm>(), scenarioStep);
                    result.Add(view, viewModel);
                }

                return result;
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException("Ошибка формирования набора View для ведения тома.", ex);
            }
        }
    }

    public interface IEntityInfoVmFactory
    {
        IEntityInfoVm<T> GetEntityInfoVm<T>(T entity) where T : IDomainObject;
    }

    public class MzUiFactory : UiFactory, IEntityInfoVmFactory
    {
        private readonly IEntityFacade _entityFacade;

        public MzUiFactory(ILifetimeScope context, IEntityFacade entityFacade)
            : base(context)
        {
            _entityFacade = entityFacade;
        }

        #region UiFactory Overrides

        public override IListItemVM<T> GetListItemVm<T>(T item)
        {
            try
            {
                var vm = this._context.Resolve<IListItemVM<T>>(new NamedParameter("entity", item), new NamedParameter("isValidateable", true));

                if (vm is ISmartValidateableVm<T>)
                {
                    (vm as ISmartValidateableVm<T>).Initialize(item);
                }
                return vm;
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IListItemVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IListItemVM<{0}>", typeof(T).Name), ex);
            }
        }

        public override IEditableVM<T> GetEditableVm<T>(T entity, bool isEditable = true)
        {

            try
            {
                if (entity is DossierFile)
                {
                    return (IEditableVM<T>)GetDossierFileVm(entity as DossierFile);
                }

                var scope = _context.BeginLifetimeScope();
                var upd = new ContainerBuilder();
                upd.RegisterInstance(entity).As<T>().AsSelf();
                upd.Update(scope.ComponentRegistry);

                var vm = scope.Resolve<IEditableVM<T>>(new NamedParameter("isEditable", isEditable));


                if (vm is ISmartEditableVm<T>)
                {
                    (vm as ISmartEditableVm<T>).Initialize(entity, _entityFacade);
                }

                return vm;
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IEditableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IEditableVM<{0}>", typeof(T).Name), ex);
            }
        }

        private IEditableVM<DossierFile> GetDossierFileVm(DossierFile dossierFile)
        {
            var scope = _context.BeginLifetimeScope();
            var addFile = new ContainerBuilder();
            addFile.RegisterInstance(dossierFile).As<DossierFile>().AsSelf();
            addFile.Update(scope.ComponentRegistry);
            var superviser = scope.Resolve<SupervisionFacade>();
            superviser.Initialize(dossierFile);

            var addSuperviser = new ContainerBuilder();
            addSuperviser.RegisterInstance(superviser).AsSelf();
            addSuperviser.Update(scope.ComponentRegistry);

            var result = scope.Resolve<ISmartEditableVm<DossierFile>>();
            result.Initialize(dossierFile, _entityFacade);
            return result;
        }

        public override IDomainValidateableVM<T> GetDomainValidateableVm<T>(T entity)
        {
            try
            {
                var vm = _context.Resolve<IDomainValidateableVM<T>>(new NamedParameter("entity", entity), new NamedParameter("isValidateable", true));

                if (vm is ISmartValidateableVm<T>)
                {
                    (vm as ISmartValidateableVm<T>).Initialize(entity);
                }

                return vm;
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IDomainValidateableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IDomainValidateableVM<{0}>", typeof(T).Name), ex);
            }
        }

        #endregion

        #region IEntityInfoVmFactory  Implementation

        public IEntityInfoVm<T> GetEntityInfoVm<T>(T entity) where T : IDomainObject
        {
            var result = _context.Resolve<IEntityInfoVm<T>>();
            result.Initialize(entity);
            return result;
        }
        
        #endregion
    }
}
