using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Malavos.Notification.Wpf.Core.Converters
{
    [ValueConversion(typeof(bool), typeof(bool)), MarkupExtensionReturnType(typeof(BoolInverse))]
    internal class BoolInverse : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is null ? false : (object)!(bool)v;
    }
}