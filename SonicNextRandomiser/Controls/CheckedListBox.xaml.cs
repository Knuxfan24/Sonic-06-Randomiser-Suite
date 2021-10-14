using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SonicNextRandomiser
{
    public class CheckedListBoxItem
    {
        public string? DisplayName { get; set; }

        public string? Tag { get; set; }

        public bool Checked { get; set; }

        public override string ToString() => DisplayName;
    }

    [ValueConversion(typeof(bool), typeof(ScrollBarVisibility))]
    public class Boolean2MultiColumnOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? Orientation.Horizontal : Orientation.Vertical;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (Orientation)value == Orientation.Horizontal ? true : false;
    }

    [ValueConversion(typeof(bool), typeof(ScrollBarVisibility))]
    public class Boolean2MultiColumnHorizontalScrollBarVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? ScrollBarVisibility.Visible : ScrollBarVisibility.Disabled;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (ScrollBarVisibility)value == ScrollBarVisibility.Visible ? true : false;
    }

    [ValueConversion(typeof(bool), typeof(ScrollBarVisibility))]
    public class Boolean2MultiColumnVerticalScrollBarVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (ScrollBarVisibility)value == ScrollBarVisibility.Visible ? false : true;
    }

    /// <summary>
    /// Interaction logic for CheckedListBox.xaml
    /// </summary>
    public partial class CheckedListBox : UserControl
    {
        public static readonly DependencyProperty MultiColumnProperty = DependencyProperty.Register
        (
            nameof(MultiColumn),
            typeof(bool),
            typeof(CheckedListBox),
            new PropertyMetadata(false)
        );

        public bool MultiColumn
        {
            get => (bool)GetValue(MultiColumnProperty);
            set => SetValue(MultiColumnProperty, value);
        }

        public ObservableCollection<CheckedListBoxItem> Items { get; set; } = new();

        public CheckedListBox()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
