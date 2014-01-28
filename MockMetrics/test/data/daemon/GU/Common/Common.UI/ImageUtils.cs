using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Common.Types.Exceptions;

using Microsoft.Win32;

namespace Common.UI
{
    /// <summary>
    /// Класс, содержащий методы работы с изображениями.
    /// </summary>
    public static class ImageUtils
    {
        /// <summary>
        /// Возвращает иконку ассоциированную с файлом.
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Иконка, ассоциированная с файлом</returns>
        public static ImageSource GetIconSourceByPath(string filePath)
        {
            try
            {
                ImageSource icon = null;
                if (icon == null && File.Exists(filePath))
                {
                    using (Icon sysicon = Icon.ExtractAssociatedIcon(filePath))
                    {
                        icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                    sysicon.Handle,
                                    System.Windows.Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());
                    }
                }

                return icon;
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка получения иконки.", ex);
            }
        }

        /// <summary>
        /// Возвращает иконку ассоциированную с расширением файла
        /// </summary>
        /// <param name="extension">Расширение</param>
        /// <returns>Иконка, ассоциированная с расширением файла</returns>
        public static ImageSource GetIconSourceByExtension(string extension)
        {
            try
            {
                ImageSource icon = null;
                using (Icon sysicon = FileIconLoader.GetFileIcon(extension, true))
                {
                    icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                sysicon.Handle,
                                System.Windows.Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
                }
                

                return icon;
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка получения иконки.", ex);
            }
        }

        /// <summary>
        /// Возвращает массив байт файла.
        /// </summary>
        /// <param name="filename">Путь к файлу</param>
        /// <returns>Массив байт файла</returns>
        public static byte[] StreamFile(string filename)
        {
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return data;
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка при загрузке файла.", ex);
            }
        }

        /// <summary>
        /// Возвращает массив байт объекта BitmapImage.
        /// </summary>
        /// <param name="imageSource">Объект BitmapImage</param>
        /// <returns>Массив байт объекта BitmapImage</returns>
        public static byte[] BufferFromImage(BitmapImage imageSource)
        {
            try
            {
                Stream stream = imageSource.StreamSource;
                Byte[] buffer = null;
                if (stream != null && stream.Length > 0)
                {
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        buffer = br.ReadBytes((Int32)stream.Length);
                    }
                }

                return buffer;
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка при парсе изображения.", ex);
            }
        }

        /// <summary>
        /// Возвращает объект BitmapImage по массиву байт изображения.
        /// </summary>
        /// <param name="rawImageBytes">Массив байт изображения</param>
        /// <returns>объект BitmapImage</returns>
        public static BitmapImage GetImage(byte[] rawImageBytes)
        {
            try
            {
                BitmapImage imageSource = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(rawImageBytes))
                {
                    ms.Position = 0;
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.UriSource = null;
                    imageSource.StreamSource = ms;
                    imageSource.EndInit();
                }
                return imageSource;
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка при формировании изображения.", ex);
            }
        }

        /// <summary>
        /// Возвращает MIME-тип ассоциированый с расширением.
        /// </summary>
        /// <param name="extension">Расширение</param>
        /// <returns>MIME-тип ассоциированый с расширением</returns>
        public static ContentType GetContentType(string extension)
        {

            string result = string.Empty;
            try
            {
                if (!extension.StartsWith("."))
                    extension = "." + extension;
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false);
                if (key != null)
                {
                    result = key.GetValue("Content Type", null).ToString();
                }
                else
                {
                    result = "application/unknown";
                }
            }
            catch (Exception)
            {
                result = "application/unknown";
            }
            return new ContentType(result);
            
        }

        /// <summary>
        /// Класс загрузчик иконок.
        /// </summary>
        /// <remarks>
        /// Был позаимствован с ныне забытого ресурса.
        /// </remarks>
        private static class FileIconLoader
        {
            [StructLayout(LayoutKind.Sequential)]
            private struct SHFILEINFO
            {
                public IntPtr hIcon;
                public IntPtr iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string szTypeName;
            };

            private const uint SHGFI_ICON = 0x100;
            private const uint SHGFI_LARGEICON = 0x0;
            private const uint SHGFI_SMALLICON = 0x1;
            private const uint SHGFI_USEFILEATTRIBUTES = 0x10;

            private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

            [DllImport("shell32.dll")]
            private static extern IntPtr SHGetFileInfo(string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbSizeFileInfo,
            uint uFlags);

            public static Icon GetFileIcon(string extension, bool largeIcon)
            {
                 string fileName = "*" + extension;

                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hImg;
                if (largeIcon)
                {
                    hImg = SHGetFileInfo(fileName, FILE_ATTRIBUTE_NORMAL, ref shinfo,
                    (uint)Marshal.SizeOf(shinfo),
                    SHGFI_ICON |
                    SHGFI_LARGEICON |
                    SHGFI_USEFILEATTRIBUTES);
                }
                else
                {
                    hImg = SHGetFileInfo(fileName, FILE_ATTRIBUTE_NORMAL, ref shinfo,
                    (uint)Marshal.SizeOf(shinfo),
                    SHGFI_ICON |
                    SHGFI_SMALLICON |
                    SHGFI_USEFILEATTRIBUTES);
                }
                try
                {
                    return Icon.FromHandle(shinfo.hIcon);
                }
                catch
                { 
                    return null;
                }
            } 
        }         
    }
}
