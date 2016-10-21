using System.Collections.Generic;

namespace DotSee.NestedContentNamer
{
    /// <summary>
    /// Summary description for NestedContentNamerRule
    /// </summary>
    public class NestedContentNamerRule
    {
        public string DocumentTypeToCheck { get; set; }
        public string PropertyAlias { get; set; }
        public Stack<string> NcDocProperties { get; set; }
        public string GeneratedPropertyAlias { get; set; }
        public string Delimiter { get; set; }

        public NestedContentNamerRule(string documentTypeToCheck, string propertyAlias, string generatedPropertyAlias="generatedName", string delimiter = " - ")
        {
            NcDocProperties = new Stack<string>();
            DocumentTypeToCheck = documentTypeToCheck;
            PropertyAlias = propertyAlias;
            NcDocProperties = NcDocProperties;
            GeneratedPropertyAlias = generatedPropertyAlias;
            Delimiter = delimiter;
        }

        public void AddNcDocProperty (string propName)
        {
            NcDocProperties.Push(propName);
        }

    }
}
