using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class AddressDescValidator : AbstractDomainValidator<AddressDesc>
    {
         public AddressDescValidator ()
         {
             var t = AddressDesc.CreateInstance();
             _validationActions[Util.GetPropertyName(() => t.RoomCount)] = ValidateRoomCount;
             _validationActions[Util.GetPropertyName(() => t.Floor)] = ValidateFloor;
             _validationActions[Util.GetPropertyName(() => t.HouseComfort)] = ValidateHouseComfort;
             _validationActions[Util.GetPropertyName(() => t.HousePrivate)] = ValidateHousePrivate;
             _validationActions[Util.GetPropertyName(() => t.AreaGenegal)] = ValidateAreaGenegal;
             _validationActions[Util.GetPropertyName(() => t.AreaLiving)] = ValidateAreaLiving;
             _validationActions[Util.GetPropertyName(() => t.HouseDoc)] = ValidateHouseDoc;
         }

         /// <summary>
         /// Валидация кол-ва комнат
         /// </summary>
         /// <param name="addressDesc"></param>
         /// <returns></returns>
         private string ValidateRoomCount(AddressDesc addressDesc)
         {
             return !Regex.IsMatch(addressDesc.RoomCount.ToString(), @"^\d{1,3}$") || addressDesc.RoomCount <= 0 ? "Поле 'Кол-во комнат' не заполнено или заполнено не верно!" : null;
         }

         /// <summary>
         /// Валидация кол-ва этажей
         /// </summary>
         /// <param name="addressDesc"></param>
         /// <returns></returns>
         private string ValidateFloor(AddressDesc addressDesc)
         {
             return !Regex.IsMatch(addressDesc.Floor.ToString(), @"^\d{1,3}$") || addressDesc.Floor <= 0 ? "Поле 'Этаж' не заполнено или заполнено не верно!" : null;
         }

         /// <summary>
         /// Валидация Общей площади
         /// </summary>
         /// <param name="addressDesc"></param>
         /// <returns></returns>
         private string ValidateAreaGenegal(AddressDesc addressDesc)
         {
             return !Regex.IsMatch(addressDesc.AreaGenegal.ToString(), @"^\d+(,\d{1,2})?$") || addressDesc.AreaGenegal <= 0 ? "Поле 'Общая площадь' не заполнено или заполнено не верно!" : null;
         }

         /// <summary>
         /// Валидация жилой площади
         /// </summary>
         /// <param name="addressDesc"></param>
         /// <returns></returns>
         private string ValidateAreaLiving(AddressDesc addressDesc)
         {
             return !Regex.IsMatch(addressDesc.AreaLiving.ToString(), @"^\d+(,\d{1,2})?$") || addressDesc.AreaLiving <= 0 ? "Поле 'Жилая площадь' не заполнено или заполнено не верно!" : null;
         }
        
         /// <summary>
         /// Валидация основания проживания
         /// </summary>
         /// <param name="addressDesc"></param>
         /// <returns></returns>
         private string ValidateHouseDoc(AddressDesc addressDesc)
         {
             return addressDesc.HouseDoc.Length <= 0 ? "Поле 'Основания проживания' не заполнено!" : null;
         }

         /// <summary>
         /// Валидация комфортабельности
         /// </summary>
         /// <param name="addressDesc"></param>
         /// <returns></returns>
         private string ValidateHouseComfort(AddressDesc addressDesc)
         {
             return addressDesc.HouseComfort <= 0 ? "Поле 'Тип комфортабельности' не заполнено!" : null;
         }
         //!Regex.IsMatch(((TextBox) sender).Text, @"^\d+(,\d{1,2})?$")
         /// <summary>
         /// Валидация комфортабельности
         /// </summary>
         /// <param name="addressDesc"></param>
         /// <returns></returns>
         private string ValidateHousePrivate(AddressDesc addressDesc)
         {
             return addressDesc.HousePrivate <= 0 ? "Поле 'Тип жилья по приватности' не заполнено!" : null;
         }

    }
}