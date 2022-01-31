using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.OnlineClient.Pagination.Models;
using System;
using System.IO;
using System.Text.Encodings.Web;

namespace OnlineShop.OnlineClient.App_Code
{
    public static class PaginationHelpers
    {
        public static HtmlString PageLinks(this IHtmlHelper html, PageInfoModel pageInfo, Func<int, string> pageUrl)
        {
            var writer = new StringWriter();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                TagBuilder tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttribute("href", pageUrl(i));
                tagBuilder.InnerHtml.AppendHtml(i.ToString());

                if (i == pageInfo.PageNumber)
                {
                    tagBuilder.AddCssClass("selected");
                    tagBuilder.AddCssClass("btn-primary");
                }

                tagBuilder.AddCssClass("btn btn-default");
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
            }

            return new HtmlString(writer.ToString());
        }
    }
}
