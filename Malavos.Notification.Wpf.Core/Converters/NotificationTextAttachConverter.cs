using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Malavos.Notification.Wpf.Core.Converters
{
    [ValueConversion(typeof(NotificationTextTrimType), typeof(bool)), MarkupExtensionReturnType(typeof(NotificationTextAttachConverter))]
    internal class NotificationTextAttachConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (!(v is NotificationTextTrimType type)) return false;
            return type == NotificationTextTrimType.Attach;
        }
    }
}