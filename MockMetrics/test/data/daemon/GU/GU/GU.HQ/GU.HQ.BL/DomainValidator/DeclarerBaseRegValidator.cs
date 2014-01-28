using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class DeclarerBaseRegValidator : AbstractDomainValidator<DeclarerBaseReg>
    {
        public DeclarerBaseRegValidator()
        {
            var t = DeclarerBaseReg.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.BaseRegItems)] = ValidateBaseRegItems;
        }

        /// <summary>
        /// Валидация указанного заявителем
        /// </summary>
        /// <param name="declarerBaseReg"></param>
        /// <returns></returns>
        private string ValidateBaseRegItems(DeclarerBaseReg declarerBaseReg)
        {  
            return declarerBaseReg.BaseRegItems.Count() != declarerBaseReg.BaseRegItems.GroupBy(t => t.QueueBaseRegTypeId).Count()
                       ? "Два или более основания постановки на учет совпадают!"
                       : null;
        }
    }
}
