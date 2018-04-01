﻿using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WpfControls.Controls.Base;

namespace WpfControls.Controls
{
    [TemplatePart(Name = "ButtonUp", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "ButtonDown", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "Box", Type = typeof(TextBox))]
    public class NumericInput : BaseThematisedControl<NumericInput>
    {
        #region Dependency Property

        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
            "Number", typeof(double?), typeof(NumericInput),
            new FrameworkPropertyMetadataNew<NumericInput>(null,
                NumberChanged));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(NumericInput),
            new FrameworkPropertyMetadataNew<NumericInput>(double.MinValue,
                MinimumChanged));


        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(NumericInput),
            new FrameworkPropertyMetadataNew<NumericInput>(double.MaxValue,
                MaximumChanged));

        public static readonly DependencyProperty AddStepProperty = DependencyProperty.Register(
            "AddStep", typeof(double), typeof(NumericInput),
            new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty RoundValueProperty = DependencyProperty.Register(
            "RoundValue", typeof(bool), typeof(NumericInput),
            new FrameworkPropertyMetadataNew<NumericInput>(true,
                OnRoundChange));

        #endregion

        #region Properties
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        public double? Number
        {
            get => (double?)GetValue(NumberProperty);
            set => SetValue(NumberProperty, value);
        }
        public double AddStep
        {
            get => (double)GetValue(AddStepProperty);
            set => SetValue(AddStepProperty, value);
        }
        public bool RoundValue
        {
            get => (bool)GetValue(RoundValueProperty);
            set => SetValue(RoundValueProperty, value);
        }

        public RepeatButton ButtonUpPart => TryFindTemplatePart<RepeatButton>("ButtonUp");
        public RepeatButton ButtonDownPart => TryFindTemplatePart<RepeatButton>("ButtonDown");
        public TextBox BoxPart => TryFindTemplatePart<TextBox>("Box");

        protected virtual double? GetValidValue
        {
            get
            {
                if (!Number.HasValue)
                    return null;

                var corect = Number.Value;
                var ballast = corect % AddStep;

                // убрали излишек
                if (!ballast.Equals(0))
                {
                    corect -= ballast;
                }

                // задали границы
                if (corect > Maximum)
                    corect = Maximum;
                if (corect < Minimum)
                    corect = Minimum;

                // округлили, если нужно
                if (RoundValue)
                    corect = Math.Round(corect);

                return corect;
            }
        }

        #endregion

        protected override void Subscribe()
        {
            base.Subscribe();

            ButtonUpPart.Click += (s, args) => AddValue();
            ButtonDownPart.Click += (s, args) => AddValue(true);

            BoxPart.PreviewKeyDown += RestrictNotDigital;
            BoxPart.AddHandler(DataObject.PastingEvent, new DataObjectPastingEventHandler(OnPasting));
            BoxPart.TextChanged += OnTextChange;

            InvalidateValue();
        }

        #region Executors
        private static void MaximumChanged(NumericInput d, DependencyPropertyChangedEventArgs e)
        {
            d.InvalidateValue();
        }
        private static void MinimumChanged(NumericInput d, DependencyPropertyChangedEventArgs e)
        {
            d.InvalidateValue();
        }
        private static void NumberChanged(NumericInput d, DependencyPropertyChangedEventArgs e)
        {
            d.InvalidateValue();
            d.InvalidateButtons();
        }
        private static void OnRoundChange(NumericInput d, DependencyPropertyChangedEventArgs e)
        {
            d.InvalidateValue();
        }

        /// <summary>
        /// Проверяю значение на правильность
        /// </summary>
        protected void InvalidateValue()
        {
            var corrected = GetValidValue;

            BoxPart.TextChanged -= OnTextChange;
            BoxPart.Text = corrected?.ToString()
                           ?? string.Empty;
            BoxPart.TextChanged += OnTextChange;
        }

        /// <summary>
        /// Обновляю значение кнопок
        /// </summary>
        /// <param name="modifier">Используемый модификатор, который увеличивает значеие</param>
        private void InvalidateButtons(double modifier = 1)
        {
            var currentNumber = Number ?? 0;

            var canAdd = Maximum - currentNumber >= AddStep * modifier;
            var canDecrease = currentNumber - Minimum >= AddStep * modifier;

            ButtonUpPart.IsEnabled = canAdd;
            ButtonDownPart.IsEnabled = canDecrease;
        }

        /// <summary>
        /// Добавляет значение к свойству Number
        /// </summary>
        /// <param name="isNegative">Прибавляет или отнимает число</param>
        protected virtual void AddValue(bool isNegative = false)
        {
            var modifier = isNegative
                ? -1
                : 1;

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
                modifier *= 10;
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
                modifier *= 100;

            if (Number.HasValue)
            {
                Number += AddStep * modifier;
            }
            else
            {
                Number = AddStep * modifier;
            }
        }

        /// <summary>
        /// Запрещаю ввод символов. Использую WinAPI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RestrictNotDigital(object sender, KeyEventArgs e)
        {
            var c = Helper.Helper.GetCharFromKey(e.Key);

            if (Char.IsControl(c)
                || Char.IsDigit(c)
                || c == '.')
                return;

            e.Handled = true;
        }
        // Ограничиваю копирование и сохраняю выделенную позицию
        protected virtual void OnPasting(object sender, DataObjectPastingEventArgs args)
        {
            if (!(sender is TextBox box)) return;

            string clipboard = args.DataObject.GetData(typeof(string)) as string;
            if (string.IsNullOrWhiteSpace(clipboard))
                return;

            // ввожу только цифры или точку
            Regex nonNumeric = new Regex(@"[\d\.]");
            string result = nonNumeric.Matches(clipboard)
                .OfType<Match>()
                .Aggregate(String.Empty,
                    (s, match) => s += match.Value);

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
        /// Пытаюсь пробросить значение текстбокса в Number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextChange(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(BoxPart.Text, NumberStyles.Any, null, out var result))
            {
                Number = result;
            }
            else
            {
                Number = null;
            }
        }

        #endregion
    }
}
