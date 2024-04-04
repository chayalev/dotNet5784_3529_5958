using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL;
/// <summary>
/// converter to Add and update buttons
/// </summary>
internal class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// convert the enabled- when we cant chage the data 
/// </summary>
internal class IsEnableConverter : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)value != 0)
        {
            return false; //Visibility.Collapsed;
        }
        else
        {
            return true;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// Converts the visibility
/// </summary>
public class VisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        if ((int)value != 0)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// Converts the cell colors background in the Gantt chart
/// </summary>
class ConvertTaskStatusToBackgroundColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        switch (status)
        {
            case "Unscheduled":
                return Brushes.White;
            case "Scheduled":
                return Brushes.Pink;
            case "Available":
                return Brushes.Orange;
            case "OnTrack":
                return Brushes.Yellow;
            case "Done":
                return Brushes.Green;
            case "InJeopardy":
                return Brushes.Red;
            default:
                return Brushes.White;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// Converts the cell colors foreground in the Gantt chart
/// </summary>
class ConvertTaskStatusToForegroundColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        switch (status)
        {
            case "Unscheduled":
                return Brushes.White;
            case "Scheduled":
                return Brushes.Pink;
            case "Available":
                return Brushes.Orange;
            case "OnTrack":
                return Brushes.Yellow;
            case "Done":
                return Brushes.Green;
            case "InJeopardy":
                return Brushes.Red;
            default:
                return Brushes.Black;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

