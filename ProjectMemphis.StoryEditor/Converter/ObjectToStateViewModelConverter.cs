using System;
using Windows.UI.Xaml.Data;

namespace ProjectMemphis.StoryEditor.Converter
{
    public class ObjectToStateViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
