namespace Common.BL.Authentification
{
    /// <summary>
    /// Интерфейс классов, ответсвенных за аутентификацию пользователей в приложении
    /// </summary>
    public interface IAuthentificator
    {
        /// <summary>
        /// Проводит аутентификацию пользователя, возвращает флаг успешности аутентификации.
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns>Флаг успешности аутентификации</returns>
        bool AuthentificateUser(string username, string password);

        /// <summary>
        /// Сообщение об ошибке аутентификации
        /// </summary>
        string ErrorMessage { get; }
    }
}
