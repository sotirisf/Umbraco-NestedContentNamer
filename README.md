This is a small Umbraco package to deal with the popular Nested Content package's restriction where the names of the nodes that are created must be derived from a single property (or a set of properties), but cannot be derived from the value of a MultiNode TreePicker (MNTP).

Of course, you can easily make it name your items anything you want instead of "Item 1", "Item 2" etc., but you have to know Angular to use the node names form MNTP selections - so this package exists only to facilitate those who don't know Angular that well.

This package allows for specifying rules for each Nested Content property inside a document, where you can define how the nodes created will be named - and yes, you can include MNTP nodes in the name - the package will look for the first selected node's name and use it when naming the Nested Content node.


To create a rule, you just have to do something like the following on the ApplicationStarted event. 


```csharp
 public class CustomEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
            
            NestedContentNamerRule r = new NestedContentNamerRule("myDocTypeAlias", "myNestedContentPropertyAlias", "generatedName", " - "));
            r.AddNcDocProperty("textProperty1");
            r.AddNcDocProperty("mntpProperty2");
            NestedContentNamer.Instance.RegisterRule(r);
        }
    }
```

Of course, you can add as many properties to the rule as you like. The package takes TextBox and MNTP properties under consideration only.

