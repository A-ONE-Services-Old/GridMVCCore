using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using GridMvc.Columns;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Razor;

namespace GridMvc.Html
{
    public static class GridExtensions
    {
        internal const string DefaultPartialViewName = "_Grid";

        public static HtmlGrid<T> Grid<T>(this IHtmlHelper helper, IEnumerable<T> items)
            where T : class
        {
            return Grid(helper, items, DefaultPartialViewName);
        }

        public static HtmlGrid<T> Grid<T>(this IHtmlHelper helper, IEnumerable<T> items, string viewName)
            where T : class
        {
            return Grid(helper, items, GridRenderOptions.Create(string.Empty, viewName));
        }

        public static HtmlGrid<T> Grid<T>(this IHtmlHelper helper, IEnumerable<T> items,
                                          GridRenderOptions renderOptions)
            where T : class
        {
            var newGrid = new Grid<T>(items.AsQueryable());
            newGrid.RenderOptions = renderOptions;
            var htmlGrid = new HtmlGrid<T>(newGrid, helper.ViewContext, renderOptions.ViewName);
            return htmlGrid;
        }

        public static HtmlGrid<T> Grid<T>(this IHtmlHelper helper, Grid<T> sourceGrid)
            where T : class
        {
            //wrap source grid:
            var htmlGrid = new HtmlGrid<T>(sourceGrid, helper.ViewContext, DefaultPartialViewName);
            return htmlGrid;
        }

        public static HtmlGrid<T> Grid<T>(this IHtmlHelper helper, Grid<T> sourceGrid, string viewName)
            where T : class
        {
            //wrap source grid:
            var htmlGrid = new HtmlGrid<T>(sourceGrid, helper.ViewContext, viewName);
            return htmlGrid;
        }

        //support IHtmlString in RenderValueAs method
        public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column, Func<T, IHtmlContent> constraint)
        {
            Func<T, string> valueContraint = a => constraint(a).ToString();
            return column.RenderValueAs(valueContraint);
        }

        //support WebPages inline helpers
        public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column,
                                                      Func<T, Func<object, HelperResult>> constraint)
        {
            Func<T, string> valueContraint = a => constraint(a)(null).ToString();
            return column.RenderValueAs(valueContraint);
        }
    }
}