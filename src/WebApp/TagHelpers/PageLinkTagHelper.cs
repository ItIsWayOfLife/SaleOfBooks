using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp.Models.Book;

namespace WebApp.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public ListBookViewModel ListBookViewModel { get; set; }
        public string PageAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "span";

            // a set of links will represent a list ul
            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            // we form three links -to the current, previous and next
            TagBuilder currentItem = CreateTag(ListBookViewModel.PageViewModel.PageNumber, urlHelper);

            // create a link to the previous page, if any
            if (ListBookViewModel.PageViewModel.HasPreviousPage)
            {
                TagBuilder prevItem = CreateTag(ListBookViewModel.PageViewModel.PageNumber - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            tag.InnerHtml.AppendHtml(currentItem);
            // create a link to the next page, if any
            if (ListBookViewModel.PageViewModel.HasNextPage)
            {
                TagBuilder nextItem = CreateTag(ListBookViewModel.PageViewModel.PageNumber + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }
            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");

            if (pageNumber == this.ListBookViewModel.PageViewModel.PageNumber)
            {
                item.AddCssClass("active");
            }
            else
            {
                link.Attributes["href"] = urlHelper.Action(PageAction, new
                {
                    page = pageNumber,
                    sortString = ListBookViewModel.SortString,
                    stringGenre = ListBookViewModel.StringGenre,
                    searchFor = ListBookViewModel.SearchFor,
                    nameSearch = ListBookViewModel.NameSearch,
                    isActive = ListBookViewModel.IsActive
                });
            }

            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);

            return item;
        }
    }
}
