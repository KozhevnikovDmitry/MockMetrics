using Common.BL.Validation;
using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.ValidationViewModel
{
    /// <summary>
    /// ������� ����� ��� ViewModel'�� � ������������ ����������� ��������� ����� �������� ��������
    /// </summary>
    /// <typeparam name="T">�������� ���</typeparam>
    public abstract class DomainValidateableVM<T> : ValidateableVM, IDomainValidateableVM<T>
        where T : IDomainObject
    {
        /// <summary>
        /// ������������ �������� ������.
        /// </summary>
        public T Entity { get; protected set; }

        /// <summary>
        /// ���� ����������� ��������������.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// ��������� ��������� �������
        /// </summary>
        protected readonly IDomainValidator<T> _domainValidator;

        /// <summary>
        /// ������� ����� ��� ViewModel'�� � ������������ ����������� ��������� ����� �������� ��������
        /// </summary>
        /// <param name="entity">������������ �������� ������</param>
        /// <param name="domainValidator">��������� ��������� �������</param>
        /// <param name="isValidateable">���� ����������� ���������</param>
        protected DomainValidateableVM(T entity, IDomainValidator<T> domainValidator, bool isValidateable = true)
            :base(isValidateable)
        {
            IsEditable = true;
            this.Entity = entity;
            _domainValidator = domainValidator;
            _domainValidator.AllowSinglePropertyValidate = false;
        }

        /// <summary>
        /// ���������� ��������� � ��������� ������ ��������� ��� ���� �������� ���� null, ���� �������� ���� �������. 
        /// </summary>
        /// <param name="columnName">��� ���� ��������</param>
        /// <returns>��������� � ��������� ������ ���������</returns>
        /// <remarks>
        /// columnName ������ ���� ������ �������� ��������� �������. 
        /// �� ���� ���� ��������, ������������ ��������, � ���� �������� ������ ���������� ���������.
        /// </remarks>
        public override string this[string columnName]
        {
            get
            {
                if (!this.AllowValidate)
                {
                    return null;
                }

                return _domainValidator.ValidateProperty(this.Entity, columnName);
            }
        }

        /// <summary>
        /// ���������� ������� ��� ���������� ������������ ����� �������� �������� ����������.
        /// </summary>
        public override void RaiseValidatingPropertyChanged()
        {
            foreach (var validatedName in _domainValidator.ValidatedNames)
            {
                this.RaisePropertyChanged(validatedName);
            }
        }

        /// <summary>
        /// ���������� ���� ���������� ������� Entity.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (!this.AllowValidate)
                {
                    return true;
                }

                return _domainValidator.Validate(this.Entity).IsValid;
            }
        }
    }
}