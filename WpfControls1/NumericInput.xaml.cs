using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfControls
{
    /// <summary>
    /// doubleeraction logic for NumericInput.xaml
    /// </summary>
    public partial class NumericInput : UserControl
    {
        public NumericInput()
        {
            InitializeComponent();
        }

        #region Dep prop

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(NumericInput), new PropertyMetadata(Double.MaxValue));


        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(NumericInput), new PropertyMetadata(Double.MinValue));


        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(NumericInput),
            new FrameworkPropertyMetadata(default(double),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                InvalidateValue));


        public static readonly DependencyProperty AddStepProperty = DependencyProperty.Register(
            "AddStep", typeof(double), typeof(NumericInput),
            new FrameworkPropertyMetadata((double)1,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static readonly DependencyProperty CanIncreaseProperty = DependencyProperty.Register(
            "CanIncrease", typeof(bool), typeof(NumericInput), new PropertyMetadata(true));


        private static readonly DependencyProperty CanDecreaseProperty = DependencyProperty.Register(
            "CanDecrease", typeof(bool), typeof(NumericInput), new PropertyMetadata(true));


        #endregion

        #region Prop
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double AddStep
        {
            get { return (double)GetValue(AddStepProperty); }
            set { SetValue(AddStepProperty, value); }
        }

        private bool CanIncrease
        {
            get { return (bool)GetValue(CanIncreaseProperty); }
            set { SetValue(CanIncreaseProperty, value); }
        }

        private bool CanDecrease
        {
            get { return (bool)GetValue(CanDecreaseProperty); }
            set { SetValue(CanDecreaseProperty, value); }
        }

        #endregion

        private static void InvalidateValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericInput input)
            {
                var correct = input.Value;
                var ballast = correct % input.AddStep;
                if (!ballast.Equals(0))
                {
                    correct -= ballast;
                }

                if (correct > input.Maximum)
                    correct = input.Maximum;

                if (correct < input.Minimum)
                    correct = input.Minimum;

                input.Value = correct;

                input.CanIncrease = input.Maximum - input.Value >= input.AddStep;
                input.CanDecrease = input.Value - input.Minimum >= input.AddStep;
            }
        }

        // Ограничиваю копирование
        private void OnPasting(object sender, DataObjectPastingEventArgs args)
        {
            if (!(sender is TextBox box)) return;

            string clipboard = args.DataObject.GetData(typeof(string)) as string;

            Regex nonNumeric = new Regex("\\D");
            string result = nonNumeric.Replace(clipboard, String.Empty);

            int start = box.SelectionStart;
            int length = box.SelectionLength;
            int caret = box.CaretIndex;

            string text = box.Text.Substring(0, start);
            text += box.Text.Substring(start + length);

            string newText = text.Substring(0, box.CaretIndex) + result;
            newText += text.Substring(caret);
            box.Text = newText;
            box.CaretIndex = caret + result.Length;

            args.CancelCommand();
        }

        /// <summary>
        /// Ограничиваю 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeforeKeyDown(object sender, KeyEventArgs e)
        {
            //if (!(sender is TextBox box)) return;

            var c = Helper.GetCharFromKey(e.Key);

            if (Char.IsControl(c) || Char.IsDigit(c))
                return;

            e.Handled = true;
        }

        private void IncreaseValue(object sender, RoutedEventArgs e)
        {
            Value += GetChangedValue();
        }

        private void DecreaseValue(object sender, RoutedEventArgs e)
        {
            Value -= GetChangedValue();
        }

        private double GetChangedValue()
        {
            var modifyers = Keyboard.Modifiers;
            var add = 1;

            if ((modifyers & ModifierKeys.Shift) != 0)
            {
                add *= 10;
            }

            if ((modifyers & ModifierKeys.Control) != 0)
            {
                add *= 100;
            }

            return AddStep * add;
        }
    }
}
