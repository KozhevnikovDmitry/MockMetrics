using System;
using System.Collections.Generic;
using GU.DataModel;

namespace GU.BL.Import
{
    public interface IContentImporter
    {
        ContentNode GetContentNode(Content content, string path);
        ContentNode GetContentNode(ContentNode contentNode, string path);

        IList<ContentNode> GetContentNodes(Content content, string path);
        IList<ContentNode> GetContentNodes(ContentNode contentNode, string path);

        bool HasContentNode(ContentNode contentNode, string path);
        bool HasChildContentNodeEndsWith(ContentNode contentNode, string tagEnd);
        bool HasChildContentNodeStartsWith(ContentNode contentNode, string tagStart);
        ContentNode GetChildContentNodeEndsWith(ContentNode contentNode, string tagEnd);
        ContentNode GetChildContentNodeStartsWith(ContentNode contentNode, string tagStart);

        string GetNodeStrValue(Content content, string path);
        string GetNodeStrValue(ContentNode contentNode, string path);

        decimal? GetNodeNumValue(Content content, string path);
        decimal? GetNodeNumValue(ContentNode contentNode, string path);

        DateTime? GetNodeDateValue(Content content, string path);
        DateTime? GetNodeDateValue(ContentNode contentNode, string path);

        bool? GetNodeBoolValue(Content content, string path);
        bool? GetNodeBoolValue(ContentNode contentNode, string path);
    }
}