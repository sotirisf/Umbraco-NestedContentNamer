using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace DotSee.NestedContentNamer
{
    /// <summary>
    /// Creates new nodes under a newly created node, according to a set of rules
    /// </summary>
    public sealed class NestedContentNamer
    {

        #region Private Members
        /// <summary>
        /// Lazy singleton instance member
        /// </summary>
        private static readonly Lazy<NestedContentNamer> _instance = new Lazy<NestedContentNamer>(() => new NestedContentNamer());

        /// <summary>
        /// The list of rule objects
        /// </summary>
        private List<NestedContentNamerRule> _rules;

        #endregion

        #region Constructors

        /// <summary>
        /// Returns a (singleton) NestedContentNamer instance
        /// </summary>
        public static NestedContentNamer Instance { get { return _instance.Value; } }


        /// <summary>
        /// Private constructor for Singleton
        /// </summary>
        private NestedContentNamer()
        {
            _rules = new List<NestedContentNamerRule>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers a new rule object 
        /// </summary>
        /// <param name="rule">The rule object</param>
        public void RegisterRule(NestedContentNamerRule rule)
        {
            _rules.Add(rule);
        }

        /// <summary>
        /// Applies all rules on creation of a node. 
        /// </summary>
        /// <param name="node">The newly created node we need to apply rules for</param>
        public void Run(IContent item)
        {
            //Loop through all rules
            foreach (NestedContentNamerRule r in _rules)
            {
                //Exit if no matching doctype or rule property names empty
                if (r.DocumentTypeToCheck != item.ContentType.Alias || r.NcDocProperties == null) continue;

                IContentService cs = ApplicationContext.Current.Services.ContentService;

                //Get the JSON value from the NC field
                JObject[] ncFieldData = JsonConvert.DeserializeObject<JObject[]>(item.GetValue(r.PropertyAlias).ToString());

                //Get a reference to ContentService
                IContentTypeService cts = ApplicationContext.Current.Services.ContentTypeService;

                //Loop through all NC nodes
                foreach (JObject t in ncFieldData)
                {
                    Stack<string> allPropVals = new Stack<string>();

                    //Loop through all propeties to be used in title (for each item)
                    foreach (string propName in r.NcDocProperties)
                    {
                        IContentType ct = cts.GetContentType(t.GetValue("ncContentTypeAlias").ToString());
                        PropertyType pt = ct.CompositionPropertyTypes.Where(x => x.Alias == propName).First();
                        string propVal = "";
                        switch (pt.PropertyEditorAlias)
                        {
                            case "Umbraco.Textbox":
                                propVal = t.GetValue(propName).ToString();
                                break;
                            case "Umbraco.MultiNodeTreePicker":
                                string tempId = (t.GetValue(propName) == null) ? "" : t.GetValue(propName).ToString().Split(',').First();
                                propVal = ContentHelper.GetHelper().TypedContent(tempId).Name;
                                break;
                            default:
                                propVal = string.Empty;
                                break;
                        }

                        if (propVal == "") continue;
                        allPropVals.Push(propVal);
                    }

                    StringBuilder finalGeneratedName = new StringBuilder(string.Empty);
                    int propValsTotal = allPropVals.Count();
                    int cnt = 1;

                    foreach (string s in allPropVals)
                    {
                        finalGeneratedName.Append(s);
                        if (cnt < propValsTotal)
                        {
                            finalGeneratedName.Append(r.Delimiter);
                        }
                        cnt++;
                    }

                    //Change the property in the JSON data
                    t.Properties().Where(x => x.Name == r.GeneratedPropertyAlias).FirstOrDefault().Value = finalGeneratedName.ToString();

                    //Reencode as JSON
                    item.Properties[r.PropertyAlias].Value = JsonConvert.SerializeObject(ncFieldData);

                    //cs.Save(item, 0, false);
                }

            }

            #endregion

        }
    }
}