using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PostGrad.Core.DomainModel
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Адрес.
    /// </summary>
    [TableName("gumz.address")]
    public abstract class Address : IdentityDomainObject<Address>, IPersistentObject
    {
        /// <summary>
        /// Значение первичного ключа. 
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.address_seq")]
        [MapField("address_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Название страны.
        /// </summary>
        public abstract string Country { get; set; }

        /// <summary>
        /// Название региона, субъекта федерации.
        /// </summary>
        [MapField("country_region")]
        public abstract string CountryRegion { get; set; }

        /// <summary>
        /// Название района региона.
        /// </summary>
        public abstract string Area { get; set; }

        /// <summary>
        /// Почтовый индекс.
        /// </summary>
        public abstract string Zip { get; set; }

        /// <summary>
        /// Название населённого пункта.
        /// </summary>
        public abstract string City { get; set; }

        /// <summary>
        /// Название улицы.
        /// </summary>
        public abstract string Street { get; set; }

        /// <summary>
        /// Номер дома.
        /// </summary>
        public abstract string House { get; set; }

        /// <summary>
        /// Номер строения.
        /// </summary>
        public abstract string Build { get; set; }

        /// <summary>
        /// Номер квартиры.
        /// </summary>
        public abstract string Flat { get; set; }

        /// <summary>
        /// Примечение. Предназначено так же для ввода адреса одной строкой.
        /// </summary>
        public abstract string Note { get; set; }

        public override string ToString()
        {
            return string.Format("г. {0}, ул. {1}, д. {2}",City, Street, House);
        }

        public virtual string ToLongString()
        {
            var sb = new StringBuilder();
            sb.Append(Zip).Append(", ");
            if (!string.IsNullOrEmpty(CountryRegion))
            {
                sb.Append(CountryRegion).Append(", ");
            }
            sb.Append(City).Append(", ").Append(Street).Append(", ").Append(House);

            if (!string.IsNullOrEmpty(Build))
            {
                sb.Append(", ").Append(Build);
            }
            if (!string.IsNullOrEmpty(Flat))
            {
                sb.Append(", ").Append("кв ").Append(Flat);
            }
            return sb.ToString();
        }
    }
}
