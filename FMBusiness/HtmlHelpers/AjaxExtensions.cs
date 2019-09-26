using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace FMBusiness.AjaxExtensions
{
    public static class AjaxExtensions
    {
        public static IHtmlString ActionLinkInnerHtml(
            this AjaxHelper ajaxHelper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            AjaxOptions ajaxOptions,
            IDictionary<string, object> htmlAttributes = null)
        {
            System.Web.Routing.RouteValueDictionary routeVals = new System.Web.Routing.RouteValueDictionary(routeValues);
            var targetUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, routeVals, ajaxHelper.RouteCollection, ajaxHelper.ViewContext.RequestContext, true);
            return MvcHtmlString.Create(ajaxHelper.GenerateLink(linkText, targetUrl, ajaxOptions ?? new AjaxOptions(), htmlAttributes));
        }

        private static string GenerateLink(
            this AjaxHelper ajaxHelper,
            string linkText,
            string targetUrl,
            AjaxOptions ajaxOptions,
            IDictionary<string, object> htmlAttributes
        )
        {
            var a = new TagBuilder("a")
            {
                InnerHtml = linkText
            };
            
            a.MergeAttributes<string, object>(htmlAttributes);
            a.MergeAttribute("href", targetUrl);
            a.MergeAttributes<string, object>(ajaxOptions.ToUnobtrusiveHtmlAttributes());
            return a.ToString(TagRenderMode.Normal);
        }
    }
}
