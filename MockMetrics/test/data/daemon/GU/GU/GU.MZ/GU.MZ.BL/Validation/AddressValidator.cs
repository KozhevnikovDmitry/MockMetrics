using System.Text.RegularExpressions;

using Common.BL.Validation;
using Common.Types;

using GU.MZ.DataModel;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Валидатор адресов
    /// </summary>
    public class AddressValidator : AbstractDomainValidator<Address>
    {
        public AddressValidator()
        {
            var address = Address.CreateInstance();
            _validationActions[Util.GetPropertyName(() => address.Zip)] = ValidateZip;
            _validationActions[Util.GetPropertyName(() => address.City)] = ValidateCity;
            _validationActions[Util.GetPropertyName(() => address.Street)] = ValidateStreet;
            _validationActions[Util.GetPropertyName(() => address.House)] = ValidateHouse;
        }

        private string ValidateZip(Address address)
        {
            if (string.IsNullOrEmpty(address.Zip))
            {
                return "Поле индекс должно быть заполнено";
            }

            if (!Regex.IsMatch(address.Zip, @"^\d{6}$"))
            {
                return "Поле индекс должно быть заполнено корректно : 888888";
            }

            return null;
        }

        private string ValidateCity(Address address)
        {
            return string.IsNullOrEmpty(address.City) ? "Поле Город должно быть заполнено" : null;
        }

        private string ValidateStreet(Address address)
        {
            return string.IsNullOrEmpty(address.Street) ? "Поле Улица должно быть заполнено" : null;
        }

        private string ValidateHouse(Address address)
        {
            return string.IsNullOrEmpty(address.House) ? "Поле Дом должно быть заполнено" : null;
        }
    }
}
