using System.Collections.Generic;
using System.Text;

namespace Common.BL.Validation
{
    /// <summary>
    /// Класс представляющий результаты валидации доменного объекта
    /// </summary>
    public class ValidationErrorInfo 
    {
        /// <summary>
        /// Список сообщений о невалидных свойствах объекта.
        /// </summary>
        private List<string> _errors = new List<string>();

        public ValidationErrorInfo()
        {
        }

        public ValidationErrorInfo(string message)
        {
            this.AddError(message);
        }
        
        public void AddError(string message)
        {
            this._errors.Add(message);
        }

        public void AddErrors(IEnumerable<string> messages)
        {
            this._errors.AddRange(messages);
        }

        public void AddResult(ValidationErrorInfo result)
        {
            this.AddErrors(result.Errors);
        }

        public void AddResult(ValidationErrorInfo result, string prefixMessage)
        {
            foreach (var error in result.Errors)
            {
                AddError(string.Format("{0}: {1}", prefixMessage, error));
            }
        }

        public bool IsValid
        {
            get
            {
                return this._errors.Count == 0;
            }
        }

        public IList<string> Errors
        {
            get
            {
                return this._errors.AsReadOnly();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this._errors.ForEach((s) => sb.AppendLine(s));
            return sb.ToString();
        }

    }
}
