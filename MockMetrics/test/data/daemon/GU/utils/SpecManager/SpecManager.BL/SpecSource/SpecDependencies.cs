using System.Collections.Generic;
using System.Linq;

using SpecManager.BL.Interface;
using SpecManager.BL.Model;

namespace SpecManager.BL.SpecSource
{
    public class SpecDependencies
    {
        public SpecDependencies(string uri, IDomainDbManager dbManager)
        {
            this.Specs = new List<Spec>();
            this.Services = new List<Service>();
            this.Tasks = new List<Task>();

            var similarSpec = dbManager.GetDomainTable<Spec>().SingleOrDefault(t => t.Uri == uri);

            if (similarSpec != null)
            {
                this._overridenSpecId = similarSpec.Id;
                this.Specs = this.GetDependentSpecs(uri, dbManager);
                this.Specs.Add(similarSpec);

                this.Services = this.GetDependentServices(this.Specs, dbManager);
                this.Tasks = this.GetDependentTasks(this.Specs, dbManager);

                this.Specs.Remove(similarSpec);
            }
            else
            {
                this._overridenSpecId = null;
            }
        }

        public bool HasDependencies
        {
            get
            {
                return this.Specs.Any() || this.Services.Any() || this.Tasks.Any();
            }
        }

        private readonly int? _overridenSpecId;

        public int? OverridenSpecId
        {
            get
            {
                return this._overridenSpecId;
            }
        }

        public List<Spec> Specs { get; private set; }

        public List<Service> Services { get; private set; }

        public List<Task> Tasks { get; private set; }

        private List<Spec> GetDependentSpecs(string uri, IDomainDbManager dbManager)
        {
            var specList = dbManager.GetDomainTable<SpecNode>()
                                    .Where(t => t.RefSpec.Uri == uri)
                                    .Select(t => t.Spec)
                                    .ToList();

            var result = new List<Spec>();

            foreach (var depSpec in specList)
            {
                result.AddRange(this.GetDependentSpecs(depSpec.Uri, dbManager));
            }

            result.AddRange(specList);

            return result.Distinct(new DomainObjectComparer<Spec>()).OrderBy(t => t.Uri).ToList();
        }

        private List<Service> GetDependentServices(List<Spec> dependentSpecs, IDomainDbManager dbManager)
        {
            var serviceList = new List<Service>();
            foreach (var depSpec in dependentSpecs)
            {
                var depServices = dbManager.GetDomainTable<Service>().Where(t => t.Spec.Uri == depSpec.Uri).ToList();

                serviceList.AddRange(depServices);
            }
            return serviceList.Distinct(new DomainObjectComparer<Service>()).OrderBy(t => t.ServiceGroupId).ThenBy(t => t.Order).ThenBy(t => t.Id).ToList();
        }

        private List<Task> GetDependentTasks(List<Spec> dependentSpecs, IDomainDbManager dbManager)
        {
            var taskList = new List<Task>();
            foreach (var depSpec in dependentSpecs)
            {
                var depTasks = dbManager.GetDomainTable<Task>().Where(t => t.Content.Spec.Uri == depSpec.Uri).ToList();

                taskList.AddRange(depTasks);
            }
            return taskList.Distinct(new DomainObjectComparer<Task>()).OrderBy(t => t.AgencyId).ThenBy(t => t.ServiceId).ThenBy(t => t.Id).ToList();
        }

    }
}