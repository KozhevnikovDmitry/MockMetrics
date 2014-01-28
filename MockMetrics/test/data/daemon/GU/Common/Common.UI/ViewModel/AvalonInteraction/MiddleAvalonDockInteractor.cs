using System;
using System.Windows.Controls;

using Common.DA.Interface;
using Common.UI.ViewModel.AvalonInteraction.InteractionEvents;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.AvalonInteraction
{
    /// <summary>
    /// Класс, преднзначенный промежуточной обработки - проброса - событий взаимодействия с AvalonDockVM
    /// </summary>
    public class MiddleAvalonDockInteractor : BaseAvalonDockInteractor
    {
        public MiddleAvalonDockInteractor(object sender)
            : base(sender)
        {
        }
    }
}
