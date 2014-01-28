using System.ComponentModel;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// Интерфейс для классов ViewModel с возможностью подтверждающих действий с введёнными данными.
    /// </summary>
    /// <remarks>
    /// Экземпляры <c>IConfirmableVM</c> используются в классе <c>ConfirmableDialogVM</c>.
    /// Идея в том, что есть OK\CANCEL-диалоги, которые проводят некоторые бизнес-операции по нажатию ОК.
    /// При этом может так получится, что в подверждении нужно будет отказать и продолжить работу в диалоге.
    /// Пример - окно логина в приложение.
    /// Для реализации такого сценария сделан IConfirmableVM со специальными методами подтверждения, восстановления состояния.
    /// И флажком IsConfirmed.
    /// </remarks>
    public interface IConfirmableVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Выполняет подтверждение.
        /// </summary>
        void Confirm();

        /// <summary>
        /// Переустанавалиает состояние объекта.
        /// </summary>
        void ResetAfterFail();

        /// <summary>
        /// Возвращает флаг подтверждённости.
        /// </summary>
        bool IsConfirmed { get; }
    }
}
