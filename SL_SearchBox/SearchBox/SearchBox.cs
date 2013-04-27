using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SL_SearchBox.SearchBox
{
    [TemplatePart(Name = ClearButton, Type = typeof(Button))]
    [TemplatePart(Name = WatermarkTextBlock, Type = typeof(TextBlock))]
    public class SearchBox : TextBox
    {
        #region TemplateParts

        private const string ClearButton = "ClearButton";
        private const string WatermarkTextBlock = "WatermarkTextBlock";

        private Button _clearButton;
        private TextBlock _watermarkTextBlock;
        private bool _needsFocus;

        #endregion

        #region Control Instantiation

        public SearchBox()
        {
            DefaultStyleKey = typeof(SearchBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clearButton = GetTemplateChild(ClearButton) as Button;
            _watermarkTextBlock = GetTemplateChild(WatermarkTextBlock) as TextBlock;

            if (_clearButton != null) _clearButton.Click += ClearButtonOnClick;
            TextChanged += OnTextChanged;
            UpdateClearButtonState();
        }

        #endregion

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            _needsFocus = FocusManager.GetFocusedElement() == _clearButton;
            if (string.IsNullOrEmpty(Text)) _watermarkTextBlock.Visibility = Visibility.Visible;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            _watermarkTextBlock.Visibility = Visibility.Collapsed;
        }

        #region ClearSearchButtonContent (DependencyProperty)

        public object ClearSearchButtonContent
        {
            get { return GetValue(ClearSearchButtonContentProperty); }
            set { SetValue(ClearSearchButtonContentProperty, value); }
        }
        public static readonly DependencyProperty ClearSearchButtonContentProperty =
            DependencyProperty.Register("ClearSearchButtonContent", typeof(object), typeof(SearchBox),
                new PropertyMetadata(null)
                );

        #endregion

        #region SearchButtonContent (DependencyProperty)

        public object SearchButtonContent
        {
            get { return GetValue(SearchButtonContentProperty); }
            set { SetValue(SearchButtonContentProperty, value); }
        }
        public static readonly DependencyProperty SearchButtonContentProperty =
            DependencyProperty.Register("SearchButtonContent", typeof(object), typeof(SearchBox),
                new PropertyMetadata(null)
                );

        #endregion

        #region Watermark (DependencyProperty)

        new public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        new public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(SearchBox),
                new PropertyMetadata(null)
                );

        #endregion

        private void OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            UpdateClearButtonState();
        }

        private void ClearButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Text = string.Empty;
            if (_needsFocus) Focus();
        }

        private void UpdateClearButtonState()
        {
            if (string.IsNullOrEmpty(Text))
            {
                _clearButton.IsEnabled = false;
                _clearButton.Content = SearchButtonContent;
                if (FocusManager.GetFocusedElement() != this) _watermarkTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                _clearButton.IsEnabled = true;
                _clearButton.Content = ClearSearchButtonContent;
                _watermarkTextBlock.Visibility = Visibility.Collapsed;
            }
        }
    }
}
