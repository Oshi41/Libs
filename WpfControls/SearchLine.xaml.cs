﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for SearchLine.xaml
    /// </summary>
    public partial class SearchLine : UserControl
    {
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(SearchLine), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchLine), new PropertyMetadata(default(ICommand)));

        public SearchLine()
        {
            InitializeComponent();
        }

        public string SearchText
        {
            get { return (string) GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public ICommand SearchCommand
        {
            get { return (ICommand) GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        private void CancelSearch(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                OnCancelSearch(sender, e);
            }
        }

        private void OnCancelSearch(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            
            SearchText = "";
            SearchCommand?.Execute("");
        }
    }
}
