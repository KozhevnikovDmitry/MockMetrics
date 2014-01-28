using System;

using GU.MZ.DataModel.Dossier;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel.Event
{
    /// <summary>
    /// Делегат событий запроса на пересобираение ViewModel'ов тома лицензионного дела
    /// </summary>
    public delegate void ViewModelRebuildRequested(object sender, ViewModelRebuildRequestedEventArgs eventArgs);

    /// <summary>
    /// Класс аргументов события <see cref="ViewModelRebuildRequested"/>
    /// </summary>
    public class ViewModelRebuildRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Сохранённый том
        /// </summary>
        public DossierFile DossierFile { get; private set; }

        /// <summary>
        /// Класс аргументов события <see cref="ViewModelRebuildRequested"/>
        /// </summary>
        /// <param name="dossierFile">Сохранённый том</param>
        public ViewModelRebuildRequestedEventArgs(DossierFile dossierFile)
        {
            DossierFile = dossierFile;
        }
    }
}
