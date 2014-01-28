namespace Common.UI.ViewModel.AvalonInteraction.InteractionEvents
{
    /// <summary>
    /// Делегат для события, оповещающего о необходимости открытия вкладки.
    /// </summary>
    /// <param name="sender">Отправитель события</param>
    /// <param name="e">Аргументы события</param>
    public delegate void OpenDockable(object sender, OpenDockableEventArgs e);

    /// <summary>
    /// Делегат для события, оповещающего о необходимости открытия вкладки.
    /// </summary>
    /// <param name="sender">Отправитель события</param>
    /// <param name="e">Аргументы события</param>
    public delegate void OpenSearchDockable(object sender, OpenSearchDockableEventArgs e);

    /// <summary>
    /// Делегат для события, оповещающего о необходимости открытия вкладки.
    /// </summary>
    /// <param name="sender">Отправитель события</param>
    /// <param name="e">Аргументы события</param>
    public delegate void OpenEditableDockable(object sender, OpenEditableDockableEventArgs e);

    /// <summary>
    /// Делегат для события, оповещающего о необходимости открытия вкладки.
    /// </summary>
    /// <param name="sender">Отправитель события</param>
    /// <param name="e">Аргументы события</param>
    public delegate void OpenReportDockable(object sender, OpenReportDockableEventArgs e);
    
    /// <summary>
    /// Делегат для события, оповещающего о смене состояния вкладки.
    /// </summary>
    /// <param name="sender">Отправитель события</param>
    /// <param name="e">Аргументы события</param>
    public delegate void ManageEditableDockable(object sender, ManageEditableDockableEventArgs e);
}
