using System;

namespace GU.Trud.BL.Export.Event
{
    /// <summary>
    /// Делегат метода обработки для события прогресса некого процесса с процентами и сообщением.
    /// </summary>
    /// <param name="senrer">Хозяин события</param>
    /// <param name="e">Аргументы события</param>
    public delegate void PercentageProgressDelegate(object senrer, PercentageProgressEventArgs e);

    /// <summary>
    /// Аргументы для события прогресса некого процесса с процентами и сообщением.
    /// </summary>
    public class PercentageProgressEventArgs : EventArgs
    {
        public PercentageProgressEventArgs()
        {
            
        }

        public PercentageProgressEventArgs(int percentage, string message, bool isCompleted = false)
        {
            Percentage = percentage;
            Message = message;
            IsCompleted = isCompleted;
        }

        public bool IsCompleted { get; set; }

        public int Percentage { get; set; }

        public string Message { get; set; }
    }
}
