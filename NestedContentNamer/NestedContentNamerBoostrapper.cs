using DotSee;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace DotSee.NestedContentNamer
{
    public class NestedContentNamerBootstrapper : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
            ContentService.Saving += ContentService_Saving;
        }

        private void ContentService_Saving(IContentService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach (var item in e.SavedEntities)
            {
                //Test comment 1
                NestedContentNamer.Instance.Run(item);
            }
        }
    }
}

