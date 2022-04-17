using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using GridMvc.Columns;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GridMvc
{
    public class GridHeaderRenderer : GridStyledRenderer, IGridColumnHeaderRenderer
    {
        private const string ThClass = "grid-header";

        private readonly List<IGridColumnHeaderRenderer> _additionalRenders = new List<IGridColumnHeaderRenderer>();

        public GridHeaderRenderer()
        {
            AddCssClass(ThClass);
        }

        public IHtmlContent Render(IGridColumn column)
        {
            string cssStyles = GetCssStylesString();
            string cssClass = GetCssClassesString();

            if (!string.IsNullOrWhiteSpace(column.Width))
                cssStyles = string.Concat(cssStyles, " width:", column.Width, ";").Trim();

            var builder = new TagBuilder("th");
            if (!string.IsNullOrWhiteSpace(cssClass))
                builder.AddCssClass(cssClass);
            if (!string.IsNullOrWhiteSpace(cssStyles))
                builder.MergeAttribute("style", cssStyles);
            builder.InnerHtml.AppendHtml(RenderAdditionalContent(column));

            return new HtmlString(builder.ToString());
        }

        protected virtual string RenderAdditionalContent(IGridColumn column)
        {
            if (_additionalRenders.Count == 0) return string.Empty;
            var sb = new StringBuilder();
            foreach (IGridColumnHeaderRenderer gridColumnRenderer in _additionalRenders)
            {
                sb.Append(gridColumnRenderer.Render(column));
            }
            return sb.ToString();
        }

        public void AddAdditionalRenderer(IGridColumnHeaderRenderer renderer)
        {
            if (_additionalRenders.Contains(renderer))
                throw new InvalidOperationException("This renderer already exist");
            _additionalRenders.Add(renderer);
        }

        public void InsertAdditionalRenderer(int position, IGridColumnHeaderRenderer renderer)
        {
            if (_additionalRenders.Contains(renderer))
                throw new InvalidOperationException("This renderer already exist");
            _additionalRenders.Insert(position, renderer);
        }
    }
}