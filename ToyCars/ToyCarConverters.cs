using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ToyCars
{
    public class ToyCarLinearGradientBrushConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = (bool)value;

            if (val)
            {
                LinearGradientBrush lgb = new LinearGradientBrush();
                lgb.GradientStops.Add(new GradientStop(Colors.LightGreen, 0));
                lgb.GradientStops.Add(new GradientStop(Colors.DarkGreen, 1));

                lgb.StartPoint = new System.Windows.Point(1, 0);
                lgb.EndPoint = new System.Windows.Point(1, 1);

                return lgb;
            }

            LinearGradientBrush lgb2 = new LinearGradientBrush();
            lgb2.GradientStops.Add(new GradientStop(Colors.Orange, 0));
            lgb2.GradientStops.Add(new GradientStop(Colors.DarkRed, 1));

            lgb2.StartPoint = new System.Windows.Point(1, 0);
            lgb2.EndPoint = new System.Windows.Point(1, 1);
            return lgb2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ToyCarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = (bool)value;

            if (val)
            {
                return Colors.Green;
            }
            return Colors.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ToyCarTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TimeSpan ts = (TimeSpan)value;
            return string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
