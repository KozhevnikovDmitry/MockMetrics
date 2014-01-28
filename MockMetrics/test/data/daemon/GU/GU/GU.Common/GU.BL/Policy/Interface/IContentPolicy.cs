using Common.BL.Validation;

using GU.DataModel;

namespace GU.BL.Policy.Interface
{
    public interface IContentPolicy
    {
        Content CreateDefault(Spec spec);
        ContentNode CreateDefaultNode(SpecNode specNode);

        ContentNode AddChildNode(ContentNode node, SpecNode childSpecNode, int? nodeNum = null);
        void AddChildNode(ContentNode node, ContentNode childNode, int? nodeNum = null);
        void RemoveNode(ContentNode node);

        void SwitchChoice(ContentNode node, SpecNode childSpecNode);

        ValidationErrorInfo Validate(ContentNode contentNode);
        ValidationErrorInfo Validate(Content content);

        Content CreateEmpty(Spec spec);

        ContentNode CreateEmptyNode(SpecNode specNode);

        bool HasValue(ContentNode contentNode);
    }
}
