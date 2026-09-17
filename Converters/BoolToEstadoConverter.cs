using System.Globalization;
using System.Windows.Data;

namespace WPF_SP.Converters
{
    public class BoolToEstadoConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool descontinuado)
                return descontinuado ? "Descontinuado" : "Activo";
            return "Activo";
        }

        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string texto)
                return texto == "Descontinuado";
            return false;
        }
    }
}
