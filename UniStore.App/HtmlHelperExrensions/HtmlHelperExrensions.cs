namespace UniStore.App.HtmlHelperExrensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Models;

    public static class HtmlHelperExrensions
    {
        public static MvcHtmlString Image(
            this HtmlHelper helper,
            string imageUrl,
            string @class = null,
            string alt = null,
            string width = null,
            string height = null)
        {
            return ImageTag(imageUrl, @class, alt, width, height);
        }

        public static MvcHtmlString ImageFromFile(
            this HtmlHelper helper,
            string imageFileName,
            string @class = null,
            string alt = null,
            string width = null,
            string height = null)
        {
            var fullFileName= string.IsNullOrWhiteSpace(imageFileName)
                ? Path.Combine("~", Constants.DefaultImage)
                : Path.Combine("~", imageFileName);

            return ImageTag(fullFileName, @class, alt, width, height);
        }

        private static MvcHtmlString ImageTag(
            string imageUrl,
            string @class = null,
            string alt = null,
            string width = null,
            string height = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);

            if (@class != null)
            {
                builder.MergeAttribute("class", @class);
            }

            if (width != null)
            {
                builder.MergeAttribute("width", width);
            }

            if (height != null)
            {
                builder.MergeAttribute("height", height);
            }
            if (alt != null)
            {
                builder.MergeAttribute("alt", alt);
            }

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString YouTube(this HtmlHelper helper, string videoId, int width = 600, int height = 400)
        {
            var builder = new TagBuilder("iframe");
            builder.MergeAttribute("width", $"{width}");
            builder.MergeAttribute("height", $"{height}");
            builder.MergeAttribute("src", $"https://www.youtube.com/embed/{videoId}");
            builder.MergeAttribute("frameborder", "0");
            builder.MergeAttribute("allowfullscreen", "allowfullscreen");


            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString Table<T>(this HtmlHelper helper, IEnumerable<T> models, params string[] cssClasses)
        {
            var table = new TagBuilder("table");
            var tableInnerHtml = new StringBuilder();
            var propertyNames = typeof(T).GetProperties().Select(info => info.Name).ToArray();
            foreach (var cssClass in cssClasses)
            {
                table.AddCssClass(cssClass);
            }

            var tableHeaderRow = new TagBuilder("tr");
            var tableHeaderInnerHtml = new StringBuilder();
            foreach (var propertyName in propertyNames)
            {
                var tableData = new TagBuilder("th");
                tableData.InnerHtml = propertyName;
                tableHeaderInnerHtml.Append(tableData);
            }

            tableHeaderRow.InnerHtml = tableHeaderInnerHtml.ToString();
            tableInnerHtml.Append(tableHeaderRow);

            foreach (var model in models)
            {
                var tableDataRow = new TagBuilder("tr");
                var tableDataRowInnerHtml = new StringBuilder();
                foreach (var propertyName in propertyNames)
                {
                    var tableData = new TagBuilder("td");
                    tableData.InnerHtml = typeof(T).GetProperty(propertyName).GetValue(model).ToString();
                    tableDataRowInnerHtml.Append(tableData);
                }

                tableDataRow.InnerHtml = tableDataRowInnerHtml.ToString();
                tableInnerHtml.Append(tableDataRow);
            }

            table.InnerHtml = tableInnerHtml.ToString();

            return new MvcHtmlString(table.ToString(TagRenderMode.Normal));
        }
    }
}