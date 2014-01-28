using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util.DataStructures;

namespace MockMetrics.Eating.Helpers
{
    public class NullDeclaredElement : IDeclaredElement
    {
        public IPsiServices GetPsiServices()
        {
            throw new ItIsNullDeclaredElementException();
        }

        public IList<IDeclaration> GetDeclarations()
        {
            throw new ItIsNullDeclaredElementException();
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            throw new ItIsNullDeclaredElementException();
        }

        public DeclaredElementType GetElementType()
        {
            throw new ItIsNullDeclaredElementException();
        }

        public XmlNode GetXMLDoc(bool inherit)
        {
            throw new ItIsNullDeclaredElementException();
        }

        public XmlNode GetXMLDescriptionSummary(bool inherit)
        {
            throw new ItIsNullDeclaredElementException();
        }

        public bool IsValid()
        {
            throw new ItIsNullDeclaredElementException();
        }

        public bool IsSynthetic()
        {
            throw new ItIsNullDeclaredElementException();
        }

        public HybridCollection<IPsiSourceFile> GetSourceFiles()
        {
            throw new ItIsNullDeclaredElementException();
        }

        public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
        {
            throw new ItIsNullDeclaredElementException();
        }

        public string ShortName { get { return "Dynamic declared element"; } }

        public bool CaseSensistiveName
        {
            get
            {
                throw new ItIsNullDeclaredElementException();
            }
        }

        public PsiLanguageType PresentationLanguage
        {
            get
            {
                throw new ItIsNullDeclaredElementException();
            }
        }
    }

    public class ItIsNullDeclaredElementException : ApplicationException
    {
        
    }
}
