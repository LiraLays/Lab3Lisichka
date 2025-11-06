using System.Diagnostics;
using static ClassLab3.Class1;

namespace ClassLab3
{
    public class Class1
    {
        // Перечисление для классификации типа теста для TryMakeStep
        public enum TaskType
        {
            Sum,
            Count,
            Max,
            None
        }

        public struct Cycle
        {
            public int i;
            public int res;
            public int[] array;
            public TaskType taskType;
            public int T;
        }

        public static Cycle InitializeCycle(int[] arr, TaskType checkedTask, int actualT)
        {
            Cycle cycle = new Cycle();
            cycle.i = 0;
            cycle.array = arr;
            cycle.taskType = checkedTask;
            cycle.res = checkedTask == TaskType.Max ? int.MinValue : 0;
            cycle.T = actualT;
            return cycle;
        }

        /// <summary>
        /// Метод для генерации случайного массива из n элементов
        /// </summary>
        /// <param name="n">Количество элементов</param>
        /// <param name="minVal">Минимальное значение элемента</param>
        /// <param name="maxVal">Максимальное значение элемента</param>
        /// <returns>Массив элементов</returns>
        public static int[] GenerateNSizeArray(int n, int minVal, int maxVal)
        {
            Random random = new Random();

            int[] newArray = new int[n];

            for (int i = 0; i < n; i++)
                newArray[i] = random.Next(minVal, maxVal);

            return newArray;
        }

        /// <summary>
        /// Метод, для расчёта суммы массива
        /// </summary>
        /// <param name="array">Массив целых чисел</param>
        /// <returns>Возвращает значения post и суммы массива в формате Tuple</returns>
        public static (bool, int) PrefixSum(int[] array)
        {
            bool post = true; // Выполнение постусловия
            int res = 0; // Результат
            int j = array.Length; // Длина масива
            int i = 0;

            // Проверка инварианта на верность перед первой итерацией
            Debug.Assert(res == 0);

            while (i < j)
            { 
                // Инвариант перед шагом: res = сумма первых i элементов
                // Тело
               
                res += array[i];
                i++;

                // Инвариант после шага: res = сумма первых i элементов
            }

            // Выход i == j => res = сумма всех элементов
            return (post, res); 
        }

        ///// <summary>
        ///// Метод, для расчёта суммы массива
        ///// </summary>
        ///// <param name="array">Массив целых чисел</param>
        ///// <returns>Возвращает значения post и суммы массива в формате Tuple</returns>
        //public static (bool, int) PrefixSumBySteps(int[] array)
        //{
        //    bool post = true; // Выполнение постусловия
        //    int res = 0; // Результат
        //    int j = array.Length; // Длина масива
        //    int i = 0;

        //    // Проверка инварианта на верность перед первой итерацией
        //    Debug.Assert(res == 0);

        //    bool result = TryMakeStep(i, j, res, array, TaskType.Sum);

        //    // Выход i == j => res = сумма всех элементов
        //    return (post, res);
        //}

        public static (bool, int) MakeOperation(int[] array, TaskType taskType, int T = 0)
        {
            bool post = true; // Выполнение постусловия
            int res = taskType == TaskType.Max ? int.MinValue : 0; // Результат
            int j = array.Length; // Длина масива
            int i = 0;

            // Проверка инварианта на верность перед первой итерацией
            Debug.Assert(res == (taskType == TaskType.Max ? int.MinValue : 0));

            while (i < j)
            {
                // Инвариант перед шагом: res = сумма первых i элементов
                // Тело

                switch (taskType)
                {
                    case TaskType.Sum:
                        res = IncreaseSum(res, i, array); break;
                    case TaskType.Count:
                        res = IncreaseCount(res, i, array, T); break;
                    case TaskType.Max:
                        res = ChangeMax(res, i, array); break;
                }

                i++;

                // Инвариант после шага: res = сумма первых i элементов
            }

            // Выход i == j => res = сумма всех элементов
            return (post, res);
        }

        /// <summary>
        /// Метод, для выполнения шага
        /// </summary>
        /// <param name="i">Итератор</param>
        /// <param name="j">Длина массива</param>
        /// <param name="res">Результат</param>
        /// <param name="array">Массив значений</param>
        /// <param name="taskType">Тип операции</param>
        /// <param name="T">Значение для сравнения</param>
        /// <returns>Возвращает значение итератора, максимальное значение, результат на данном шаге
        ///          В случае если цикл закончился возвращает все значения null
        /// </returns>
        public static (int?, int?, int?) MakeStep(int i, int res, int[] array, TaskType taskType, int T = 0)
        {
            int j = array.Length;
            (bool result, int resInt) = TryMakeStep(i, j, res, array, taskType, T);
            if (result)
                return (i, j, resInt);
            else
                return (null, null, null);
        }
    
        /// <summary>
        /// Попытка сделать ход. Проверяет не вышло ли значение i за границы j.
        /// </summary>
        /// <param name="i">Итератор</param>
        /// <param name="j">Длина массива</param>
        /// <param name="res">Результат</param>
        /// <param name="array">Массив значений</param>
        /// <param name="taskType">Тип операции</param>
        /// <param name="T">Значение для сравнения</param>
        /// <returns>true - если изменение прошло успешно, false - в случае окончания цикла</returns>
        public static (bool, int) TryMakeStep(int i, int j, int res, int[] array, TaskType taskType, int T)
        {
            if (i >= j) return (false, res);
            else
            {
                switch (taskType)
                {
                    case TaskType.Sum:
                        res = IncreaseSum(res, i, array);
                        break;
                    case TaskType.Count:
                        res = IncreaseCount(res, i, array, T);
                        break;
                    case TaskType.Max:
                        res = ChangeMax(res, i, array);
                        break;
                }
                return (true, res);
            }
        }

        // Методы для изменения Суммы, Количества элементов или максимального элемента 
        private static int IncreaseSum(int res, int i, int[] array) => res += array[i];
        private static int IncreaseCount(int res, int i, int[] array, int T) => res += array[i] > T ? 1 : 0;
        private static int ChangeMax(int res, int i, int[] array) => Math.Max(array[i], res);

        /// <summary>
        /// Метод, для расчёта количества элементов > целого T
        /// </summary>
        /// <param name="array">Массив целых чисел</param>
        /// <param name="T">Значение, с которым сравнивается каждый элемент массива</param>
        /// <returns>Возвращает значения post и количества элементов в массиве > целого T в формате Tuple</returns>
        public static (bool, int) CountGreaterThanT(int[] array, int T)
        {
            bool post = true; // Выполнение постусловия
            int res = 0; // Результат
            int j = array.Length; // Длина массива
            int i = 0;

            // Проверка инварианта на верность перед первой итерацией
            Debug.Assert(res == 0);

            while (i < j)
            {
                // Инвариант перед шагом: res = количество элементов > T среди первых i 
                res += array[i] > T ? 1 : 0;
                i++;

                // Инвариант после шага: res = количество элементов > T среди первых i 
            }

            // Выход i == j => res = количество элементов > T
            return (post, res);
        }

        /// <summary>
        /// Метод, для расчёта максимального элемента в массиве
        /// </summary>
        /// <param name="array">Массив целых чисел</param>
        /// <returns>Возвращает значения post и максимального элемента в массиве в формате Tuple</returns>
        public static (bool, int) PrefixMax(int[] array)
        {
            bool post = true; // Выполнение постусловия
            int res = 0; // Результат
            int j = array.Length; // Длина массива
            int i = 0;

            // Проверка инварианта на верность перед первой итерацией
            Debug.Assert(res == 0);

            while (i < j)
            {
                // Инвариант перед шагом: res = максимальный среди первых i элементов
                res = Math.Max(array[i], res);
                i++;

                // Инвариант после шага: res = максимальный среди первых i элементов
            }

            // Выход i == j => res = максимальный элемент массива
            return (post, res);
        }
    }
}
