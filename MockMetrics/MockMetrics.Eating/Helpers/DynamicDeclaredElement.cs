using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util.DataStructures;

namespace MockMetrics.Eating.Helpers
{
    public class DynamicDeclaredElement : IDeclaredElement
    {
        public IPsiServices GetPsiServices()
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public IList<IDeclaration> GetDeclarations()
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public DeclaredElementType GetElementType()
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public XmlNode GetXMLDoc(bool inherit)
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public XmlNode GetXMLDescriptionSummary(bool inherit)
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public bool IsValid()
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public bool IsSynthetic()
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public HybridCollection<IPsiSourceFile> GetSourceFiles()
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
        {
            throw new ItIsDynamicDeclaredElementException();
        }

        public string ShortName { get { return "Dynamic declared element"; } }

        public bool CaseSensistiveName
        {
            get
            {
                throw new ItIsDynamicDeclaredElementException();
            }
        }

        public PsiLanguageType PresentationLanguage
        {
            get
            {
                throw new ItIsDynamicDeclaredElementException();
            }
        }
    }

    public class ItIsDynamicDeclaredElementException : ApplicationException
    {
        
    }
}
