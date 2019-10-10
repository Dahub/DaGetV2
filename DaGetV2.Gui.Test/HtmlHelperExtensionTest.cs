namespace DaGetV2.Gui.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.Unicode;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Antiforgery.Internal;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Xunit;

    public class HtmlHelperExtensionTest
    {
        [Theory]
        [InlineData(0, "0.00 €")]
        [InlineData(0.00, "0.00 €")]
        [InlineData(10.25, "10.25 €")]
        [InlineData(8962.78, "8 962.78 €")]
        [InlineData(12.2, "12.20 €")]
        [InlineData(-89.35, "- 89.35 €")]
        [InlineData(-8692541.12, "- 8 692 541.12 €")]
        [InlineData(8692541.12, "8 692 541.12 €")]
        [InlineData(-47, "- 47.00 €")]
        public void EuroShouldReturnFormattedEuroString(decimal value, string expected)
        {          
            var helper = new FakeHtmlHelper();
            using (var writer = new StringWriter())
            {
                helper.Euro(value).WriteTo(
                    writer,
                    HtmlEncoder.Default);
                var result = writer.ToString();
                Assert.Equal(expected, result);
            }
        }
    }

    public class FakeHtmlHelper : IHtmlHelper
    {
        public IHtmlContent ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostname,
            string fragment, object routeValues, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent AntiForgeryToken()
        {
            throw new NotImplementedException();
        }

        public MvcForm BeginForm(string actionName, string controllerName, object routeValues, FormMethod method, bool? antiforgery,
            object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public MvcForm BeginRouteForm(string routeName, object routeValues, FormMethod method, bool? antiforgery,
            object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent CheckBox(string expression, bool? isChecked, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Display(string expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            throw new NotImplementedException();
        }

        public string DisplayName(string expression)
        {
            throw new NotImplementedException();
        }

        public string DisplayText(string expression)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent DropDownList(string expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Editor(string expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            throw new NotImplementedException();
        }

        public string Encode(object value)
        {
            throw new NotImplementedException();
        }

        public string Encode(string value)
        {
            throw new NotImplementedException();
        }

        public void EndForm()
        {
            throw new NotImplementedException();
        }

        public string FormatValue(object value, string format)
        {
            throw new NotImplementedException();
        }

        public string GenerateIdFromName(string fullName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : struct
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetEnumSelectList(Type enumType)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Hidden(string expression, object value, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public string Id(string expression)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Label(string expression, string labelText, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent ListBox(string expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public string Name(string expression)
        {
            throw new NotImplementedException();
        }

        public Task<IHtmlContent> PartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Password(string expression, object value, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent RadioButton(string expression, object value, bool? isChecked, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Raw(string value)
        {
            return new HtmlContentBuilder().AppendHtml(value);
        }

        public IHtmlContent Raw(object value)
        {
            throw new NotImplementedException();
        }

        public Task RenderPartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent RouteLink(string linkText, string routeName, string protocol, string hostName, string fragment,
            object routeValues, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent TextArea(string expression, string value, int rows, int columns, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent TextBox(string expression, object value, string format, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent ValidationMessage(string expression, string message, object htmlAttributes, string tag)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent ValidationSummary(bool excludePropertyErrors, string message, object htmlAttributes, string tag)
        {
            throw new NotImplementedException();
        }

        public string Value(string expression, string format)
        {
            throw new NotImplementedException();
        }

        public Html5DateRenderingMode Html5DateRenderingMode { get; set; }
        public string IdAttributeDotReplacement { get; }
        public IModelMetadataProvider MetadataProvider { get; }
        public dynamic ViewBag { get; }
        public ViewContext ViewContext { get; }
        public ViewDataDictionary ViewData { get; }
        public ITempDataDictionary TempData { get; }
        public UrlEncoder UrlEncoder { get; }
    }
}
