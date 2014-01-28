using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.Exceptions
{
    public class DomainBLLException : BLLException
    {
        private readonly IDictionary<string, IDomainObject> _objects;

        #region Constructors IDictionary<string,IDomainObject>

        public DomainBLLException(IDictionary<string, IDomainObject> domainObjects)
        {
            _objects = domainObjects;
        }

        public DomainBLLException(string message, IDictionary<string, IDomainObject> domainObjects)
            : base(message)
        {
            _objects = domainObjects;
        }


        public DomainBLLException(Exception innerException, IDictionary<string, IDomainObject> domainObjects)
            : base(innerException.Message, innerException)
        {
            _objects = domainObjects;
        }

        public DomainBLLException(string message, Exception innerException, IDictionary<string, IDomainObject> domainObjects)
            : base(message, innerException)
        {
            _objects = domainObjects;
        }

        #endregion

        #region Constructors params IDomainObject[]

        public DomainBLLException(params IDomainObject[] domainObjects)
        {
            _objects = ObjectsToDict(domainObjects);
        }

        public DomainBLLException(string message, params IDomainObject[] domainObjects)
            : base(message)
        {
            _objects = ObjectsToDict(domainObjects);
        }


        public DomainBLLException(Exception innerException, params IDomainObject[] domainObjects)
            : base(innerException.Message, innerException)
        {
            _objects = ObjectsToDict(domainObjects);
        }

        public DomainBLLException(string message, Exception innerException, params IDomainObject[] domainObjects)
            : base(message, innerException)
        {
            _objects = ObjectsToDict(domainObjects);
        }
        
        #endregion

        #region Property

        public IEnumerable<KeyValuePair<string, IDomainObject>> DomainObjects
        {
            get
            {
                return _objects.AsEnumerable();
            }
        }

        public override string Message
        {
            get
            {
                return FormatMessage(_objects, base.Message);
            }
        }

        #endregion

        #region Other

        protected Dictionary<string, IDomainObject> ObjectsToDict(IDomainObject[] domainObjects)
        {
            var objects = new Dictionary<string, IDomainObject>();
            if (domainObjects != null)
            {
                for (var i = 0; i < domainObjects.Length; i++)
                    objects.Add("Object " + i.ToString(), domainObjects[i]);
            }
            return objects;
        }

        protected static string FormatMessage(IEnumerable<KeyValuePair<string, IDomainObject>> objects, string message)
        {
            var sb = new StringBuilder();

            sb.AppendLine(!String.IsNullOrEmpty(message) ? message : "Ошибка при работе с объектом");

            if (objects != null)
            {
                foreach (var obj in objects)
                {
                    if (obj.Value != null)
                    {
                        sb.AppendLine(String.Format(
                            "{0} {1} {{Id = {2}, Description = {3}}}",
                            String.IsNullOrEmpty(obj.Key) ? "Объект" : obj.Key,
                            obj.Value.GetType(),
                            obj.Value.GetKeyValue(),
                            obj.Value
                            ));
                    }
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
