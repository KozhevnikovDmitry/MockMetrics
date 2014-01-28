using BLToolkit.DataAccess;
using BLToolkit.Data.Linq;
using Common.DA.Interface;

namespace Common.DA
{
    /// <summary>
    /// Базовый класс для классов предназначенных для отображения доменных объектов в БД.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта</typeparam>
    /// <remarks>
    /// Страшно абстрактный и обобщённый класс попёртый http://projects.rsdn.ru/RFD/wiki/DataAccess и допиленный под интерфейс <c>IDomainObjectAccessor</c>
    /// UPD 3.05.2012, petrenko:
    /// От исходного примера почти ничего не осталось.
    /// Три важных момента:
    /// 1. диспоз DbManager'а целиком лежит на плечах вызывающего кода.
    ///     то, что раньше были конструкции try-finally{Dispose(DbManager)} по факту ничего не давало,
    ///     но приводило к весьма странному поведению. (TODO: следует изучить вопрос более детально)
    /// 2. Query&lt;T&gt; не содержит методы по извлечению/удалению объекта по ключу,
    ///     поэтому используется SqlQuery&lt;T&gt;, который на данный момент (похоже) не развивается.
    /// 3. При текущем дизайне аксессор создается на каждое действие,
    ///     такжке как и генерация запросов Query&lt;T&gt;/SqlQuery&lt;T&gt; (но, возможно, BLT сам кэширует запросы).
    ///     Это является потенциально узким местом в высоконагруженных приложениях.
    /// 
    /// UPD 22.11.2012, kozhevnikov
    /// При Insert'e объекта, у которого первичный ключ является внешним ключом (связь один к одному)
    /// Метод Insert затирает значение первичного ключа на 0.
    /// Видимо это связано с отсутствием identity и sequence у сущности,
    /// так как insert выполняется методом InsertWithIdentity.
    /// При этом данные ложатся в базу корректно.
    /// 
    /// Костыльно это решается в датамапперах, повторным проставлением id.
    /// </remarks>
    public abstract class AbstractAccessor<T> : DataAccessor<T>, IDomainObjectAccessor
    {
        private readonly SqlQuery<T> _query = new SqlQuery<T>();

        #region IDomainObjectAccessor

        public IDomainObject SelectByKey(object key)
        {
            return (IDomainObject)_query.SelectByKey(GetDbManager(), key);
        }

        public object Insert(IDomainObject obj)
        {
            return Query<T>.InsertWithIdentity(DataContextInfo.Create(GetDbManager()), (T)obj);
        }

        public object Update(IDomainObject obj)
        {
            return Query<T>.Update(DataContextInfo.Create(GetDbManager()), (T)obj);
        }

        public object InsertOrUpdate(IDomainObject obj)
        {
            return Query<T>.InsertOrReplace(DataContextInfo.Create(GetDbManager()), (T)obj);
        }

        public void Delete(IDomainObject obj)
        {
            Query<T>.Delete(DataContextInfo.Create(GetDbManager()), (T)obj);
        }

        public void DeleteByKey(object key)
        {
            _query.DeleteByKey(GetDbManager(), key);
        }

        #endregion
    }
}
