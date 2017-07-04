using System;
using System.Globalization;
using System.Windows.Data;
/// <summary>
/// Converts null to boolean. 
/// </summary>
/// <remarks>
/// If parameter is not specified or specified with null:
/// if passed value is null then true is returned, otherwise false. 
/// If parameter is specified then inversion is done:
/// if passed value is null then false is returned, otherwise true.
/// </remarks>

namespace OchUploader.FrontEnd.WPF.Converter
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class NullBoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value == null ? false : true;
            if (parameter != null)
                return !result;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
