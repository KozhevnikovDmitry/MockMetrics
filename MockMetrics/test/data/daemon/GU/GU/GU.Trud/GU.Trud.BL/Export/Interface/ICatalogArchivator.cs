namespace GU.Trud.BL.Export.Interface
{
    /// <summary>
    /// Интерфейс классов осуществляющих архивацию каталогов с файлами.
    /// </summary>
    internal interface ICatalogArchivator
    {
        /// <summary>
        /// Осуществляет архивацию каталога файлов.
        /// </summary>
        /// <param name="archiveCatalog">Полное имя архивируемого каталога</param>
        /// <param name="destinationFile">Полное имя файла архива</param>
        void Archive(string archiveCatalog, string destinationFile);
    }
}
