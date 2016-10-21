This package requires you to have the Nested Content package already installed. What it does is that it lets you allow one or more properties that Nested Content items will get their name from, including MultiNode TreePicker properties (their first selected node's value is used).

In order to use this, you will have to have a Label property in document types used by Nested Content (i.e., used as blueprints to define data for Nested Content).

This is a workaround to Nested Content's restriction where the names of the nodes that are created must be derived from a single property (or a set of properties), but cannot easily be derived from the value of a MultiNode TreePicker (MNTP).

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

The constructor takes 4 arguments:
"myDocTypeAlias" is the document type alias for the document containing the Nested Content.
"myNestedContentPropertyAlias" is the alias of the property used for Nested Content entries
"generatedName" is the default name of the Label field inside each document type you use as a bluepring for Nested Content. You can change this name to whatever you are using. This parameter is optional.
" - " is the default delimiter in case you have two or more properties that will be used in the name. This parameter is optional.

So if I have a document of type "ChristmasTree" which uses a NestedContent property with alias "decorations", which in turn uses a "DecorationItem" document type as a blueprint containing the properties "shape" (MNTP), "title" (Textbox), and "generatedName" (Label) which I want to both use as an item title, my rule would be something like the following:


```csharp

            NestedContentNamerRule r = new NestedContentNamerRule("ChristmasTree", "decorations", "generatedName", " - "));
            r.AddNcDocProperty("title");
            r.AddNcDocProperty("shape");
            NestedContentNamer.Instance.RegisterRule(r);
```

So a decoration with title "Red Ornament" and an MNTP choice of a "Spherical" node would appear as "Red Ornament - Spherical" in the item's title.



