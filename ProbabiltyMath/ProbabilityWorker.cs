using System;
using System.Collections.Generic;
using System.Linq;

namespace ProbabiltyMath
{
    public class ProbabilityWorker
    {
        private Random _random = new Random();

        /// <summary>
        /// Если True, пересоздаем генератор случайных чисел в каждом вызове случайного числа
        /// <para>Внимание, может понизить (точно понизит) производительность </para>
        /// </summary>
        public bool ExtremelyRandom { get; set; }

        /// <summary>
        /// Возвращает случайное число в интервале [min. max) ближе к концу
        /// <para>Выбирает последние 5% элементов примерно в 10 раз чаще, чем первые 5%</para>
        /// </summary>
        /// <param name="min">Включаемый минимум</param>
        /// <param name="max">Невключаемый максимум</param> 
        /// <returns></returns>
        public int ChooseCloserToEnd(double min, double max)
        {
            var coefficient = 0.666;
            return (int)ChooseElementByCoefficient(min, max, coefficient);
        }

        /// <summary>
        /// Возвращает случайное число в интервале [min. max) ближе к началу
        /// <para>Выбирает первые 5% элементов примерно в 10 раз чаще, чем последние 5%</para>
        /// </summary>
        /// <param name="min">Включаемый минимум</param>
        /// <param name="max">Невключаемый максимум</param> 
        /// <returns></returns>
        public int ChooseCloserToBegining(double min, double max)
        {
            var coefficient = 2.41;
            return (int)ChooseElementByCoefficient(min, max, coefficient);
        }

        /// <summary>
        /// Выбирает случайный элемент в массиве ближе к концу
        /// <para>Выбирает последние 5% элементов примерно в 10 раз чаще, чем первые 5%</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public T ChooseElementCloserToEnd<T>(IEnumerable<T> sequence)
        {
            if (sequence == null || !sequence.Any())
                return default(T);

            var index = ChooseCloserToEnd(0, sequence.Count());
            T result = sequence.ElementAtOrDefault(index);
            return result;
        }

        /// <summary>
        /// Выбирает случайный элемент в массиве ближе к началу
        /// <para>Выбирает первые 5% элементов примерно в 10 раз чаще, чем последние 5%</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public T ChooseElementCloserToBeggining<T>(IEnumerable<T> sequence)
        {
            if (sequence == null || !sequence.Any())
                return default(T);

            var index = ChooseCloserToBegining(0, sequence.Count());
            T result = sequence.ElementAtOrDefault(index);
            return result;
        }

        ///// <summary>
        ///// Возвращает случайное число в интервале [min. max) ближе к концу
        ///// </summary>
        ///// <param name="min">Включаемый минимум</param>
        ///// <param name="max">Невключаемый максимум</param>
        ///// <param name="coefficient">
        ///// <para>Степень функции выборки. Строго больше нуля, но не больше единицы!</para>
        ///// <para>По умолчанию 0.666, выбирает конечные элементы где-то в 10 раз чаще</para>
        ///// </param>
        ///// <returns></returns>
        //public double ChooseCloserToEnd(double min, double max, 
        //    double coefficient = 0.666)
        //{
        //    // ограничиваю выборку
        //    if (coefficient > 1)
        //        coefficient = 1;
        //    if (coefficient <= 0)
        //        coefficient = 1;


        //    return ChooseElementByCoefficient(min, max, coefficient);
        //}

        ///// <summary>
        ///// Возвращает случайное число в интервале [min. max) ближе к началу
        ///// </summary>
        ///// <param name="min">Вклчаемый минимум</param>
        ///// <param name="max">Невключаемый максимум</param>
        ///// <param name="coefficient">Степень функции выборки. Строго больше единицы!
        ///// <para>По умолчанию 2.41, что приводит к выбору первых элементов где-то в 10 раз чаще, чем последних</para>
        ///// </param>
        ///// <returns></returns>
        //public double ChooseCloserToBegining(double min, double max,
        //    double coefficient = 2.41)
        //{
        //    // ограничиваю выборку
        //    if (coefficient < 1)
        //        coefficient = 1;

        //    return ChooseElementByCoefficient(min, max, coefficient);
        //}

        ///// <summary>
        ///// Возвращает случайное число в интервале [min. max) ближе к концу 
        ///// </summary>
        ///// <param name="min"></param>
        ///// <param name="max"></param>
        ///// <param name="multiplier">Во сколько раз последние элементы будут выбираться чаще</param>
        ///// <returns></returns>
        //public double ChooseCloserToEndByMultiplier(double min, double max, double multiplier)
        //{
        //    if (multiplier < 1)
        //        multiplier = 1;

        //    var y = 2.41;
        //    var x = 10;

        //    var y1 = 3.35;
        //    var x1 = 20;

        //    var y2 = 4.201;
        //    var x2 = 30;

        //    var pow = Math.Log(y, x) + Math.Log(y1, x1) + Math.Log(y2, x2);
        //    pow /= 3;

        //    var coefficient = Math.Pow(multiplier, pow);
        //    return ChooseElementByCoefficient(min, max, coefficient);
        //}


        #region Private methods

        /// <summary>
        /// Инициализирую рандом для !!!РАЗНЫХ!!! значений
        /// </summary>
        private void InitRandom()
        {
            if (!ExtremelyRandom)
                return;

            // перешел на более быстрый вариант инициализации
            var seed = _random.Next(int.MaxValue);
            _random = new Random(seed);

            //var seed = (int) DateTime.Now.Ticks & 0x0000FFFF;
            //_random = new Random(seed);
        }

        /// <summary>
        /// Возвращает ошибки в границах выборки
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private bool HasBorderErrors(double min, double max)
        {
            var result = double.IsNaN(max)
                         || double.IsNaN(min)
                         || max < min;

            return result;
        }

        /// <summary>
        /// Выбирает случайное число
        /// </summary>
        /// <param name="min">Включаемый минимум</param>
        /// <param name="max">Невключаемый минимум</param>
        /// <param name="coefficient">Степень функции выборки.
        /// <para>При значении больше 1, вероятность сдвигается к начальным элементам</para>
        /// <para>При значении меньше 1, вероятность сдвигается к конечным элементам</para>
        /// </param>
        /// <returns></returns>
        private double ChooseElementByCoefficient(double min, double max, double coefficient)
        {
            // ошибка в границах, выходим
            if (HasBorderErrors(min, max))
                return double.NaN;

            // не из чего выбирать, выходим
            if (min.Equals(max))
                return min;

            // обновляю рандом
            InitRandom();

            var result = min + (max - min) * Math.Pow(_random.NextDouble(), coefficient);

            // Возвращаю округлённое значение
            return Math.Round(result);
        }

        ///// <summary>
        ///// Build a function by three points
        ///// </summary>
        ///// <param name="p1"></param>
        ///// <param name="p2"></param>
        ///// <param name="p3"></param>
        ///// <param name="mu">Точность алгоритма</param>
        ///// <returns></returns>
        //private Func<double, double> Function(Point p1, Point p2, Point p3, double mu = 0.05)
        //{
        //    return x =>
        //    {
        //        var x1 = p1.X;
        //        var x2 = p2.X;
        //        var x3 = p3.X;

        //        var y1 = p1.Y;
        //        var y2 = p2.Y;
        //        var y3 = p3.Y;

        //        for (double i = 0; i <= 1; i += 0.05)
        //        {
        //            y = 
        //        }
        //    };
        //}

        #endregion
    }
}

