using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace WEB.HtmlHelpers
{
    public class PagingHelpers
    {
        public static string pageUrl(int curPage, string? searchStr = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = curPage.ToString();

            if (!string.IsNullOrWhiteSpace(searchStr))
                query["searchStr"] = searchStr;

            return "?" + query.ToString();
        }

        /// <summary>
        /// This is applicable for Bootstrap version 3.0
        /// </summary>
        public static IHtmlContent BootstrapPageLinks(int CurrentPage, int TotalPages, int TotalResults, string searchStr)
        {
            bool isRtl = true;
            string records = isRtl ? "سجلات" : "records";
            string nextStr = isRtl ? "التالى" : "Next";
            string previousStr = isRtl ? "السابق" : "Previous";
            string firstPage = isRtl ? "أول صفحة" : "First Page";
            string lastPage = isRtl ? "أخر صفحة" : "Last Page";

            const short max = 10;
            double level = Math.Ceiling(CurrentPage / (double)max) * max;

            var list = new TagBuilder("ul");
            if (isRtl)
                list.MergeAttribute("direction", "rtl");

            list.MergeAttribute("class", "pagination");

            int startPage = (int)level - max + 1;

            IHtmlContent TagMaker(string text, string? url, bool isActive)
            {
                var pageNumberTag = new TagBuilder("a");
                pageNumberTag.MergeAttribute("class", "page-link");

                if (!string.IsNullOrWhiteSpace(url))
                    pageNumberTag.MergeAttribute("href", url);

                pageNumberTag.InnerHtml.AppendHtml(new HtmlString(text));

                var listItem = new TagBuilder("li");
                listItem.MergeAttribute("class", "page-item");

                if (isActive)
                    listItem.AddCssClass("active");

                listItem.InnerHtml.AppendHtml(pageNumberTag);
                return listItem;
            }

            IHtmlContent DirectionMaker(string destination, string? url, bool isLink = true)
            {
                var liChildTag = new TagBuilder("a");
                liChildTag.MergeAttribute("class", "page-link");

                if (isLink)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                        liChildTag.MergeAttribute("href", url);
                }
                else
                {
                    liChildTag.AddCssClass("disabled");
                }

                var iconTag = new TagBuilder("span");
                string liClasses = "";

                if (!string.IsNullOrWhiteSpace(destination))
                {
                    iconTag.InnerHtml.AppendHtml(destination);

                    if (destination == nextStr)
                        liClasses += "next";
                    if (destination == previousStr)
                        liClasses += "prev";
                }

                liChildTag.InnerHtml.AppendHtml(iconTag);

                var listItem = new TagBuilder("li");
                listItem.MergeAttribute("class", liClasses, true);
                listItem.InnerHtml.AppendHtml(liChildTag);
                return listItem;
            }

            // Previous and First
            if (TotalPages > 1 && CurrentPage != 1)
            {
                list.InnerHtml.AppendHtml(DirectionMaker(previousStr, pageUrl(CurrentPage - 1, searchStr)));
                list.InnerHtml.AppendHtml(DirectionMaker(firstPage, pageUrl(1, searchStr)));
            }
            else
            {
                list.InnerHtml.AppendHtml(DirectionMaker(previousStr, pageUrl(CurrentPage - 1, searchStr), false));
            }

            // Page Numbers
            for (int i = startPage; i <= level; i++)
            {
                if (i > TotalPages)
                    break;

                if (i == CurrentPage)
                    list.InnerHtml.AppendHtml(TagMaker(i.ToString(), null, true));
                else
                    list.InnerHtml.AppendHtml(TagMaker(i.ToString(), pageUrl(i, searchStr), false));
            }

            // Last and Next
            if (TotalPages > 1 && CurrentPage != TotalPages)
            {
                list.InnerHtml.AppendHtml(DirectionMaker(lastPage, pageUrl(TotalPages, searchStr)));

                if (TotalResults > 1)
                    list.InnerHtml.AppendHtml(TagMaker($"{TotalResults} {records}", null, false));

                list.InnerHtml.AppendHtml(DirectionMaker(nextStr, pageUrl(CurrentPage + 1, searchStr)));
            }
            else
            {
                if (TotalResults > 1)
                    list.InnerHtml.AppendHtml(TagMaker($"{TotalResults} {records}", null, false));

                list.InnerHtml.AppendHtml(DirectionMaker(nextStr, pageUrl(CurrentPage + 1, searchStr), false));
            }

            // Direct Page Input
            if (TotalPages > 1)
            {
                var boxTag = new TagBuilder("input");
                boxTag.MergeAttribute("placeholder", "page");
                boxTag.MergeAttribute("type", "text");
                boxTag.MergeAttribute("style", "width:60px;margin:0;");

                string baseUrl = pageUrl(1, searchStr).Replace("page=1", "page=");

                string jsNavigate = "if (event.keyCode==13){" +
                                    "if(isNaN(this.value) || this.value < 1){alert('Please enter positive number');return;}" +
                                    "if(this.value > " + TotalPages + "){alert('Maximum number allowed is " + TotalPages + "');return;}" +
                                    "var url ='" + baseUrl + "';" +
                                    "url = url.replace('page=', 'page=' + this.value);" +
                                    "window.location = url;" +
                                    "}";

                boxTag.MergeAttribute("onkeypress", jsNavigate);

                var listItem = new TagBuilder("li");
                listItem.MergeAttribute("class", "page-item");

                var wrapper = new TagBuilder("a");
                wrapper.MergeAttribute("class", "page-link");
                wrapper.InnerHtml.AppendHtml(boxTag);
                wrapper.MergeAttribute("style", "padding-top:9px;padding-bottom:9px;");
                listItem.InnerHtml.AppendHtml(wrapper);

                list.InnerHtml.AppendHtml(listItem);
            }

            var result = new TagBuilder("nav");
            result.InnerHtml.AppendHtml(list);
            return result;
        }
    }
}
