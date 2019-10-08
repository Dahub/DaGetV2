using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DaGetV2.Gui
{
    public static class HtmlHelperExtension
    {
        public static IHtmlContent Euro(this IHtmlHelper helper, decimal amount)
        {
            amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);

            var sb = new StringBuilder("€ ");
            var decimalPassed = false;
            var postDecimalCounter = 0;
            foreach (var c in amount.ToString().Reverse())
            {
                if (c == '-' || c == '+')
                {
                    sb.Append(" ");
                    sb.Append(c);
                    break;
                }
                sb.Append(c);
                if (decimalPassed)
                {
                    postDecimalCounter++;
                }

                if (c == '.' || c == ',')
                {
                    decimalPassed = true;
                }
                if (postDecimalCounter > 0 && postDecimalCounter % 3 == 0)
                {
                    sb.Append(" ");
                }
            }

            return helper.Raw(new string(sb.ToString().Reverse().ToArray()));
        }
    }
}
