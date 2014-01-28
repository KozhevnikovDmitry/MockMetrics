using Common.BL.Validation;
using Common.Types;

using GU.BL.Policy;
using GU.BL.Policy.Interface;
using GU.DataModel;

namespace GU.BL.DomainValidation
{
    public class ContentNodeValidator : AbstractDomainValidator<ContentNode>
    {
        private readonly IContentPolicy _contentPolicy;

        public ContentNodeValidator()
        {
            _contentPolicy = new ContentPolicy();
            var foo = ContentNode.CreateInstance();
            _validationActions[Util.GetPropertyName(() => foo.StrValue)] = this.ValidateContentNode;
            _validationActions[Util.GetPropertyName(() => foo.NumValue)] = this.ValidateContentNode;
            _validationActions[Util.GetPropertyName(() => foo.BoolValue)] = this.ValidateContentNode;
            _validationActions[Util.GetPropertyName(() => foo.DateValue)] = this.ValidateContentNode;
            _validationActions[Util.GetPropertyName(() => foo.DictKey)] = this.ValidateContentNode;
            _validationActions[Util.GetPropertyName(() => foo.BlobName)] = this.ValidateContentNode;
        }

        private string ValidateContentNode(ContentNode contentNode)
        {
            var errorInfo = _contentPolicy.Validate(contentNode);
            if (errorInfo.IsValid)
            {
                return null;
            }

            return errorInfo.Errors.FirstOrNull();
        }

        public override ValidationErrorInfo Validate(ContentNode domainObject)
        {
            var result = _contentPolicy.Validate(domainObject);
            AllowSinglePropertyValidate = true;
            return result;
        }
    }
}
