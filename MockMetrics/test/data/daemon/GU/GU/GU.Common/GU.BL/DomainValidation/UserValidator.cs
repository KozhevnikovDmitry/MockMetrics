using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;

using GU.DataModel;

namespace GU.BL.DomainValidation
{
    public class UserValidator : AbstractDomainValidator<DbUser>
    {
        public UserValidator()
        {
            var t = DbUser.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.Name)] = ValidateUserName;
            _validationActions[Util.GetPropertyName(() => t.AgencyId)] = ValidateUserAgency;
        }

        private string ValidateUserName(DbUser user)
        {
            if (!Regex.IsMatch(user.Name, @"^[a-zA-Z][a-zA-Z_0-9]+$"))
            {
                return "Логин может содержать только английские символы";
            }

            return null;
        }

        private string ValidateUserAgency(DbUser user)
        {
            if(user.AgencyId==null)
            {
                return "Ведомство обязательно к заполнению";
            }

            return null;
        }
    }
}
