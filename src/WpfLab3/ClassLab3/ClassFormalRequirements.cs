using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLab3.Class1;

namespace ClassLab3
{
    public class ClassFormalRequirements
    {
        /// <summary>
        /// Метод для вывода предусловия
        /// </summary>
        /// <param name="taskType">Тип цикла</param>
        /// <returns>Предусловие</returns>
        private static string PreCalculation(TaskType taskType)
        {
            string pre;
            switch (taskType)
            {
                case TaskType.Sum:
                    pre = "n >= 0 и n != null";
                    break;
                case TaskType.Max:
                    pre = "a.Length != 0";
                    break;
                case TaskType.Count:
                    pre = "a != null";
                    break;
                default:
                    pre = string.Empty;
                    break;
            }

            return pre;
        }

        /// <summary>
        /// Метод для вывода инварианта
        /// </summary>
        /// <param name="taskType">Тип цикла</param>
        /// <returns>Инвариант</returns>
        private static string InvCalculation(TaskType taskType)
        {
            string inv;
            switch (taskType)
            {
                case TaskType.Sum:
                    inv = $"res = сумма первых j элементов массива";
                    break;
                case TaskType.Max:
                    inv = $"res = максимальный элемент массива a на промежутке от 0-го до (j - 1)-го элемента";
                    break;
                case TaskType.Count:
                    inv = $"res = число индексов i < j, для которых a[i] > T";
                    break;
                default:
                    inv = string.Empty;
                    break;
            }

            return inv;
        }

        /// <summary>
        /// Метод для вывода предусловия
        /// </summary>
        /// <param name="taskType">Тип цикла</param>
        /// <returns>Постусловие</returns>
        private static string PostCalculation(TaskType taskType)
        {
            string post;
            switch (taskType)
            {
                case TaskType.Sum:
                    post = $"res = Σ_{{i = 0}}^{{j - 1}} a[i]";
                    break;
                case TaskType.Max:
                    post = $"res = max(a[0 .. j - 1])";
                    break;
                case TaskType.Count:
                    post = $"res = |{{ i ∈ [0..j - 1] : a[i] > T }}|";
                    break;
                default:
                    post = string.Empty;
                    break;
            }

            return post;
        }

        /// <summary>
        /// Метод для расчёта t функции
        /// </summary>
        /// <param name="taskType">Тип цикла</param>
        /// <param name="i">Итератор</param>
        /// <param name="j">Длинна массива</param>
        /// <returns>Значение t функции в текстовом формате</returns>
        private static string TCalculation(TaskType taskType, int i, int j) => taskType == TaskType.Count? $"{VariantFunctionT(i, j)}" : null;

        /// <summary>
        /// Метод для вывода всех формальных требований
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static string[] ShowAllFormalRequirements(TaskType taskType, int i, int j)
        {
            string pre = PreCalculation(taskType);
            string inv = InvCalculation(taskType);
            string post = PostCalculation(taskType);
            string T = TCalculation(taskType, i, j);
            string VC = "1. Pre => Inv" +
                "2. (Inv ∧ B) => wp(S, Inv)" +
                "3. (Inv ∧ !B) => Post" +
                "4. Inv ∧ B => t>=0 && Inv ∧ B => wp(S, t' < t)";
            return [pre, inv, post, VC, T];
        }
    }
}
