using System.Collections.Generic;

using System.Linq;

namespace SpecManager.BL.Model
{
    public class SpecValidations
    {
        public SpecValidations()
        {
            this.Messages = new List<string>();
        }

        public bool IsValid
        {
            get
            {
                return ! this.Messages.Any();
            }
        }

        public List<string> Messages { get; private set; }
    }
}
