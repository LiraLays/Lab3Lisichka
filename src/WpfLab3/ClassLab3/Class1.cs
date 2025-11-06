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

        /// <summary>
        /// Структура цикла расчёта, содержит в себе все данные, используемые в цикле рассчёта
        /// </summary>
        public struct Cycle
        {
            public bool post;
            public int i;
            public int res;
            public int[] array;
            public TaskType taskType;
            public int T;
        }


        /// <summary>
        /// Метод для инициализации цикла расчёта
        /// </summary>
        /// <param name="arr">Массив целых чисел</param>
        /// <param name="checkedTask">Выбранный тип</param>
        /// <param name="actualT">Текущее значение T</param>
        /// <returns>Структура cycle</returns>
        public static Cycle InitializeCycle(int[] arr, TaskType checkedTask, int actualT)
        {
            Cycle cycle = new Cycle();
            cycle.post = true;
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
        /// Метод, для выполенния операции. Тип операции зависит от TaskType
        /// </summary>
        /// <param name="array">Массив целых чисел</param>
        /// <param name="taskType">Тип операции</param>
        /// <param name="T">Значение для сравнения</param>
        /// <returns>Зачение post и результат в формате Tuple</returns>
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
                // Тело

                switch (taskType)
                {
                    case TaskType.Sum:
                        // Инвариант перед шагом: res = сумма первых i элементов
                        res = IncreaseSum(res, i, array); break;
                        // Инвариант после шага: res = сумма первых i элементов
                    case TaskType.Count:
                        // Инвариант перед шагом: res = количество элементов > T среди первых i 
                        res = IncreaseCount(res, i, array, T); break;
                        // Инвариант после шага: res = количество элементов > T среди первых i 
                    case TaskType.Max:
                        // Инвариант перед шагом: res = максимальный среди первых i элементов
                        res = ChangeMax(res, i, array); break;
                        // Инвариант после шага: res = максимальный среди первых i элементов
                }
                i++;

            }

            // Выход i == j => res = сумма всех элементов / res = количество элементов > T среди первых i / res = максимальный среди первых i элементов
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
        public static (int?, int?, int?, bool) MakeStep(bool post, int i, int res, int[] array, TaskType taskType, int T = 0)
        {
            int j = array.Length;
            (bool result, int resInt, bool postRes) = TryMakeStep(post, i, j, res, array, taskType, T);
            if (result)
                return (i, j, resInt, postRes);
            else
                return (null, null, null, postRes);
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
        private static (bool, int, bool) TryMakeStep(bool post, int i, int j, int res, int[] array, TaskType taskType, int T)
        {
            if (i >= j) return (false, res, true);
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
                return (true, res, false);
            }
        }

        // Методы для изменения Суммы, Количества элементов или максимального элемента 
        private static int IncreaseSum(int res, int i, int[] array) => res += array[i];
        private static int IncreaseCount(int res, int i, int[] array, int T) => res += array[i] > T ? 1 : 0;
        private static int ChangeMax(int res, int i, int[] array) => Math.Max(array[i], res);


        /// <summary>
        /// Метод, возвращающий инвариант для указанного режима
        /// </summary>
        /// <param name="taskType">Тип цикла</param>
        /// <param name="array">Массив целых чисел</param>
        /// <param name="T">Значение для сравнения</param>
        /// <returns>Инвариант в текстовом виде и инвариант в виде формулы в формате Tuple</returns>
        public static (string, string) InvariantType(TaskType taskType, int[] array, int T)
        {
            string invariantText, invariantFormula;
            int j = array.Length;
            switch (taskType)
            {
                case TaskType.Sum:
                    invariantText = $"res = сумма первых {j} элементов массива";
                    invariantFormula = $"res = Σ_{{i = 0}}^{{{j - 1}}} a[i]";
                    break;
                case TaskType.Count:
                    invariantText = $"res = число индексов i < {j}, для которых a[i] > {T}";
                    invariantFormula = $"res = |{{ i ∈ [0..{j - 1}] : a[i] > {T} }}|";
                    break;
                case TaskType.Max:
                    invariantText = $"res = максимальный элемент массива a на промежутке от 0-го до {j - 1}-го элемента";
                    invariantFormula = $"res = max(a[0 .. {j - 1}])";
                    break;
                default:
                    invariantText = "";
                    invariantFormula = "";
                    break;
            }

            return (invariantText, invariantFormula);
        }

        /// <summary>
        /// Метод для вычисления варианта-функции t
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static int VariantFunctionT(int i, int j) => j - i;

    }
}
