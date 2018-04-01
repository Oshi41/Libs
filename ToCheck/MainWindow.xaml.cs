using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using ProbabiltyMath;
using ToCheck.Annotations;

namespace ToCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _number;

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public MainWindow()
        {
            InitializeComponent();

            //Func<double, double> func = x =>
            //{
            //    var result = Math.Pow(x, 1.666) / 1.666;
            //    return result;
            //};

            //var diff = Math.Round(func(100) / func(1), 4);
            //MessageBox.Show(diff + "");

            //List<double> list = new List<double>();
            //for (int i = 0; i < 20; i++)
            //{
            //    list.Add(SomeTest());
            //}

            //MessageBox.Show(list.Aggregate(String.Empty, (s, d) => s += d + "\n"));
        }

        private double SomeTest()
        {
            var prob = new ProbabilityWorker{ExtremelyRandom = false};
            var totalAmount = 1000 * 1000 * 1;

            var list = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(0);
            }

            double min = 0;
            double max = 101;

            for (int i = 0; i < totalAmount; i++)
            {
                //var rand = prob.ChooseCloserToEnd(min, max);
                //var rand = prob.ChooseCloserToEndByMultiplier(min, max, 40);
                //var index = (int)(rand / 5);
                //if (index >= list.Count)
                //    index--;

                //list[index]++;
            }

            var probList = new List<double>();


            string s = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                int temp1 = i * 5;
                int temp2 = (i + 1) * 5;

                var probability = list[i] / (double)totalAmount * 100;
                probability = Math.Round(probability, 4);
                probList.Add(probability);

                s += $"Вероятность от {temp1} до {temp2} - {probability}%\n";
            }

            var f = probList.FirstOrDefault();
            var l = probList.LastOrDefault();
            double r;
            if (f > l)
            {
                r = Math.Round(f / l, 2);
                s += $"Выбираем ближе к началу с вероятностью выше в {r} раз(a)";
            }
            else
            {
                r = Math.Round(l / f, 2);
                s += $"Выбираем ближе к концу с вероятностью выше в {r} раз(a)";
            }

            //s += $"Разница между первым и последним - {Math.Round( / , 4)}\n";
            s += $"\nОбщая сумма вероятностей: {probList.Sum(x => x)}";

            //MessageBox.Show(s);
            return r;
        }

        public string Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
