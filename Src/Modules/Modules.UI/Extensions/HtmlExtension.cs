namespace Modules.UI.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using Common.Extensions;

    public static class HtmlExtension
    {
        public static string DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

            return metadata.Description;
        }

        public static IDictionary<string, object> Disabled(this object obj, bool disabled)
        {
            if (obj == null)
                return null;

            return disabled ? obj.AddProperty("disabled", "disabled") : obj.ToDictionary();
        }

        public static string DisplayName(this Enum value)
        {
            if (value == null)
                return null;

            var enumType = value.GetType();

            var enumValue = Enum.GetName(enumType, value);

            var member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);

            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }
    }
}