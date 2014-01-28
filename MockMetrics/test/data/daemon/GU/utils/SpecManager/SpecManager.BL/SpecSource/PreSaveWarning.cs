using System.Collections.Generic;
using System.Linq;

using SpecManager.BL.Model;

namespace SpecManager.BL.SpecSource
{
    public class PreSaveWarning
    {
        public PreSaveWarning()
        {
            Messages = new List<string>();
        }

        public PreSaveWarning(SpecDependencies specDependencies)
            :this()
        {
            foreach (var depSpec in specDependencies.Specs)
            {
                this.Messages.Add(depSpec.ToString());
            }

            foreach (var depService in specDependencies.Services)
            {
                this.Messages.Add(depService.ToString());
            }

            foreach (var depTask in specDependencies.Tasks)
            {
                this.Messages.Add(depTask.ToString());
            }
        }

        public PreSaveWarning(SpecValidations specValidations)
            : this()
        {
            foreach (var validation in specValidations.Messages)
            {
                this.Messages.Add(validation);
            }  
        }

        public PreSaveWarning(SpecDependencies specDependencies, SpecValidations specValidations)
            : this(specDependencies)
        {
            foreach (var validation in specValidations.Messages)
            {
                this.Messages.Add(validation);
            }   
        }

        public bool HasWarnings
        {
            get
            {
                return Messages.Any();
            }
        }

        public string Title { get; set; }

        public List<string> Messages { get; private set; }
    }
}
