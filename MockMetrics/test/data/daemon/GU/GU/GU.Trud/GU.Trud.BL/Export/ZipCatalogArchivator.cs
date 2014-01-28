using System.IO;
using GU.Trud.BL.Export.Interface;
using Ionic.Zip;

namespace GU.Trud.BL.Export
{
    /// <summary>
    /// Класс предназначенный проведения zip-архивации каталогов.
    /// </summary>
    internal class ZipCatalogArchivator : ICatalogArchivator
    {
        /// <summary>
        /// Осуществляет zip-архивацию каталога файлов.
        /// </summary>
        /// <param name="archiveCatalog">Полное имя архивируемого каталога</param>
        /// <param name="destinationFile">Полное имя файла архива</param>
        public void Archive(string archiveCatalog, string destinationFile)
        {
            using (var zip = new ZipFile())
            {
                foreach (var file in Directory.GetFiles(archiveCatalog))
                {
                    zip.AddFile(file, string.Empty);
                }
                zip.Save(destinationFile);
            }
        }
    }
}
