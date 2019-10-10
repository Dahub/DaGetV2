namespace DaGetV2.Gui
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

    public static class HtmlHelperExtension
    {
        public static IHtmlContent TextBoxForWithFrenchDecimal<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            object htmlAttributes)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var modelExplorer = ExpressionMetadataProvider.FromLambdaExpression(
                                                                        expression, 
                                                                        htmlHelper.ViewData, 
                                                                        htmlHelper.MetadataProvider);

            if (modelExplorer.ModelType.Name.Equals("Decimal"))
            {
                var value = (decimal)modelExplorer.Model;
                return htmlHelper.TextBox(modelExplorer.Metadata.PropertyName, value.ToString().Replace(',', '.'), htmlAttributes);
            }

            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlContent Euro(this IHtmlHelper helper, decimal amount)
        {
            amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
            var stringAmount = amount.ToString().Replace(',', '.');
            var parts = stringAmount.Split('.', StringSplitOptions.RemoveEmptyEntries);

            var firstPart = parts.First();
            string secondPart;
            
            if (parts.Length == 1)
            {
                secondPart = "00";
            }
            else if (parts[1].Length == 1)
            {
                secondPart = string.Concat(parts[1], "0");
            }
            else
            {
                secondPart = parts[1];
            }

            var sb = new StringBuilder();
            var counter = 0;

            foreach (var c in firstPart.Reverse())
            {
                if (c == '-' || c == '+')
                {
                    sb.Append(" ");
                    sb.Append(c);
                    break;
                }
               
                if (counter > 0 && counter % 3 == 0)
                {
                    sb.Append(" ");
                }
                sb.Append(c);
                counter++;
            }

            return helper.Raw($"{new string(sb.ToString().Reverse().ToArray())}.{secondPart} €");
        }
    }
}
