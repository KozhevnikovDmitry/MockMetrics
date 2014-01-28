using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using SpecManager.BL.Exceptions;
using SpecManager.BL.Model;

namespace SpecManager.BL.SpecSource
{
    public class XmlSpecSource : ISpecSource
    {
        private readonly string _sourcePath;
        
        public XmlSpecSource(string sourcePath)
        {
            _sourcePath = sourcePath;
        }

        public Spec Get()
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(Spec));
                using (var xr = new XmlTextReader(_sourcePath))
                {
                    var spec =  (Spec)xmlSerializer.Deserialize(xr);
                    spec.SetParentness();
                    return spec;
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при сериализации Spec", ex);
            }
        }

        public Spec Save(Spec spec)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(Spec));
                using (var xw = new XmlTextWriter(_sourcePath, Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    xmlSerializer.Serialize(xw, spec);
                }
                var result = Get();
                return result;
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при десериализации Spec", ex);
            }
        }

        public string Name
        {
            get
            {
                return string.Format("file=[{0}]", this._sourcePath);
            }
        }

        public PreSaveWarning PreSave(Spec spec)
        {
            try
            {
                return new PreSaveWarning(spec.Validate()) {Title = "Валидация спеки перед сохранением в XML"};
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при проверке условий сохранения в xml файл", ex);
            }
        }
    }
}
