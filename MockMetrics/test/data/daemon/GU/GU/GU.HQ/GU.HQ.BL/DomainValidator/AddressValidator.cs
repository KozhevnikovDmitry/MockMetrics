using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class AddressValidator : AbstractDomainValidator<Address>
    {
        public AddressValidator()
        {
            var t = Address.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.PostIndex)] = ValidatePostIndex;
            _validationActions[Util.GetPropertyName(() => t.City)] = ValidateCity;
            _validationActions[Util.GetPropertyName(() => t.Street)] = ValidateStreet;
            _validationActions[Util.GetPropertyName(() => t.HouseNum)] = ValidateHouseNum;
            _validationActions[Util.GetPropertyName(() => t.KvNum)] = ValidateKvNum;
            _validationActions[Util.GetPropertyName(() => t.KorpNum)] = ValidateKorpNum;
        }

        /// <summary>
        /// Валидация почтового индекса
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string ValidatePostIndex(Address address)
        {
            return !Regex.IsMatch(address.PostIndex, @"^\d{6}$") ? "Поле 'Почтовый индекс' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация города
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string ValidateCity(Address address)
        {
            return !Regex.IsMatch(address.City, @"^[а-яА-Я\s\-\.]{1,500}$") ? "Поле 'Город' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация улицы
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string ValidateStreet(Address address)
        {
            return !Regex.IsMatch(address.Street, @"^[а-яА-Я\s\-\.]{1,500}$") ? "Поле 'Улица' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация номера дома
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string ValidateHouseNum(Address address)
        {
            return !Regex.IsMatch(address.HouseNum, @"^[\dа-яА-Я\s\-//]{1,20}$") ? "Поле 'Номер дома' не заполнено или заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация корпуса
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string ValidateKorpNum(Address address)
        {
            return !Regex.IsMatch(address.KorpNum, @"^([\dа-яА-Я\s\-//]{1,20})?$") ? "Поле 'Корпус/строение' заполнено не верно!" : null;
        }

        /// <summary>
        /// Валидация номера квартиры
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string ValidateKvNum(Address address)
        {
            return !Regex.IsMatch(address.KvNum, @"^([\dа-яА-Я\s\-//]{1,20})?$") ? "Поле 'Квартира' заполнено не верно!" : null;
        }
    }
}