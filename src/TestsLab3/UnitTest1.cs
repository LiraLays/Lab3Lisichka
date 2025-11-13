using Xunit;
using ClassLab3;
using static ClassLab3.Class1;

namespace TestsLab3
{
    public class Class1Tests
    {
        /// <summary>
        /// Проверяет что массив генерируется с правильным размером
        /// </summary>
        [Fact]
        public void GenerateNSizeArray_ValidParameters_ReturnsCorrectSize()
        {
            // Arrange
            int n = 5;
            int minVal = 0;
            int maxVal = 100;

            // Act
            int[] result = Class1.GenerateNSizeArray(n, minVal, maxVal);

            // Assert
            Assert.Equal(n, result.Length);
        }

        /// <summary>
        /// Проверяет что все элементы массива в заданном диапазоне
        /// </summary>
        [Fact]
        public void GenerateNSizeArray_ValidParameters_ValuesInRange()
        {
            // Arrange
            int n = 10;
            int minVal = 10;
            int maxVal = 20;

            // Act
            int[] result = Class1.GenerateNSizeArray(n, minVal, maxVal);

            // Assert
            Assert.All(result, value => Assert.InRange(value, minVal, maxVal - 1));
        }

        /// <summary>
        /// Проверяет генерацию массива с разными параметрами
        /// </summary>
        /// <param name="n"></param>
        /// <param name="minVal"></param>
        /// <param name="maxVal"></param>
        [Theory]
        [InlineData(1, 0, 10)]
        [InlineData(10, -50, 50)]
        [InlineData(100, 0, 1)]
        public void GenerateNSizeArray_VariousParameters_ReturnsCorrectSize(int n, int minVal, int maxVal)
        {
            // Act
            int[] result = Class1.GenerateNSizeArray(n, minVal, maxVal);

            // Assert
            Assert.Equal(n, result.Length);
        }

        /// <summary>
        /// Проверяет корректную инициализацию цикла для суммы
        /// </summary>
        [Fact]
        public void InitializeCycle_SumTask_CorrectInitialization()
        {
            // Arrange
            int[] array = { 1, 2, 3 };
            TaskType taskType = TaskType.Sum;
            int actualT = 0;

            // Act
            Cycle cycle = Class1.InitializeCycle(array, taskType, actualT);

            // Assert
            Assert.True(cycle.post);
            Assert.Equal(0, cycle.i);
            Assert.Equal(0, cycle.res);
            Assert.Equal(array, cycle.array);
            Assert.Equal(taskType, cycle.taskType);
            Assert.Equal(actualT, cycle.T);
        }

        /// <summary>
        /// Проверяет корректную инициализацию цикла для максимума
        /// </summary>
        [Fact]
        public void InitializeCycle_MaxTask_CorrectInitialization()
        {
            // Arrange
            int[] array = { 1, 2, 3 };
            TaskType taskType = TaskType.Max;
            int actualT = 0;

            // Act
            Cycle cycle = Class1.InitializeCycle(array, taskType, actualT);

            // Assert
            Assert.Equal(int.MinValue, cycle.res);
        }

        /// <summary>
        /// Проверяет правильность результатов для разных типов операций
        /// </summary>
        /// <param name="array"></param>
        /// <param name="taskType"></param>
        /// <param name="T"></param>
        /// <param name="expectedResult"></param>
        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, TaskType.Sum, 0, 15)]
        [InlineData(new int[] { 1, 5, 3, 7, 2 }, TaskType.Count, 3, 2)]
        [InlineData(new int[] { 1, 5, 3, 7, 2 }, TaskType.Max, 0, 7)]
        public void MakeOperation_VariousTasks_ReturnsCorrectResults(int[] array, TaskType taskType, int T, int expectedResult)
        {
            // Act
            var (post, result) = Class1.MakeOperation(array, taskType, T);

            // Assert
            Assert.True(post);
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Проверяет обработку пустого массива для поиска максимума
        /// </summary>
        [Fact]
        public void MakeOperation_EmptyArrayMax_ReturnsMinValue()
        {
            // Arrange
            int[] array = { };
            TaskType taskType = TaskType.Max;

            // Act
            var (post, result) = Class1.MakeOperation(array, taskType);

            // Assert
            Assert.True(post);
            Assert.Equal(int.MinValue, result);
        }

        /// <summary>
        /// Проверяет обработку пустого массива для суммы
        /// </summary>
        [Fact]
        public void MakeOperation_EmptyArraySum_ReturnsZero()
        {
            // Arrange
            int[] array = { };
            TaskType taskType = TaskType.Sum;

            // Act
            var (post, result) = Class1.MakeOperation(array, taskType);

            // Assert
            Assert.True(post);
            Assert.Equal(0, result);
        }

        /// <summary>
        /// Проверяет корректность первого шага для суммы
        /// </summary>
        [Fact]
        public void MakeStep_SumTaskFirstStep_ReturnsCorrectValues()
        {
            // Arrange
            int[] array = { 1, 2, 3 };
            TaskType taskType = TaskType.Sum;
            bool post = true;
            int i = 0;
            int res = 0;

            // Act
            var (newI, newJ, newRes, newPost) = Class1.MakeStep(post, i, res, array, taskType);

            // Assert
            Assert.Equal(0, newI);
            Assert.Equal(3, newJ);
            Assert.Equal(1, newRes); // 0 + 1
            Assert.False(newPost);
        }

        /// <summary>
        /// Проверяет возврат null при завершении цикла
        /// </summary>
        [Fact]
        public void MakeStep_LastStep_ReturnsNulls()
        {
            // Arrange
            int[] array = { 1 };
            TaskType taskType = TaskType.Sum;
            bool post = true;
            int i = 1; // Выход за границы
            int res = 1;

            // Act
            var (newI, newJ, newRes, newPost) = Class1.MakeStep(post, i, res, array, taskType);

            // Assert
            Assert.Null(newI);
            Assert.Null(newJ);
            Assert.Null(newRes);
            Assert.True(newPost);
        }

        /// <summary>
        /// Проверяет возврат корректных строк инвариантов
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="expectedText"></param>
        /// <param name="expectedFormula"></param>
        [Theory]
        [InlineData(TaskType.Sum, "сумма первых", "Σ")]
        [InlineData(TaskType.Count, "число индексов", ">")]
        [InlineData(TaskType.Max, "максимальный элемент", "max")]
        public void InvariantType_VariousTasks_ReturnsCorrectStrings(TaskType taskType, string expectedText, string expectedFormula)
        {
            // Arrange
            int[] array = { 1, 2, 3 };
            int T = 2;

            // Act
            var (text, formula) = Class1.InvariantType(taskType, array, T);

            // Assert
            Assert.Contains(expectedText, text);
            Assert.Contains(expectedFormula, formula);
        }

        /// <summary>
        /// Проверяет вычисление варианта-функции
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(2, 5, 3)]
        [InlineData(0, 10, 10)]
        [InlineData(5, 5, 0)]
        [InlineData(10, 15, 5)]
        public void VariantFunctionT_VariousInputs_ReturnsCorrectValue(int i, int j, int expected)
        {
            // Act
            int result = Class1.VariantFunctionT(i, j);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Проверяет пошаговый подсчет элементов с порогом
        /// </summary>
        [Fact]
        public void MakeStep_CountTaskWithThreshold_CorrectCounting()
        {
            // Arrange
            int[] array = { 1, 5, 3, 7, 2 };
            TaskType taskType = TaskType.Count;
            int T = 3;
            bool post = true;
            int i = 0;
            int res = 0;

            // Act & Assert - шаг 1: 1 > 3? нет
            var step1 = Class1.MakeStep(post, i, res, array, taskType, T);
            Assert.Equal(0, step1.Item1);
            Assert.Equal(0, step1.Item3); // не увеличилось

            // Act & Assert - шаг 2: 5 > 3? да
            var step2 = Class1.MakeStep(false, 1, (int)step1.Item3!, array, taskType, T);
            Assert.Equal(1, step2.Item1);
            Assert.Equal(1, step2.Item3); // увеличилось

            // Act & Assert - шаг 3: 3 > 3? нет
            var step3 = Class1.MakeStep(false, 2, (int)step2.Item3!, array, taskType, T);
            Assert.Equal(2, step3.Item1);
            Assert.Equal(1, step3.Item3); // не увеличилось
        }
    }

    public class ClassFormalRequirementsTests
    {
        /// <summary>
        /// Проверяет что метод возвращает корректную строку с формулой weakest precondition
        /// </summary>
        [Fact]
        public void ShowWpFormula_ReturnsCorrectString()
        {
            // Act
            string result = ClassFormalRequirements.ShowWpFormula();

            // Assert
            Assert.Contains("Inv ∧ B", result);
            Assert.Contains("wp(S, Inv)", result);
        }

        /// <summary>
        /// Проверяет что все типы задач возвращают полный набор компонентов формальных требований
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="expectedPre"></param>
        /// <param name="expectedInv"></param>
        /// <param name="expectedPost"></param>
        [Theory]
        [InlineData(TaskType.Sum, "n >= 0", "сумма", "Σ")]
        [InlineData(TaskType.Max, "a.Length != 0", "максимальный", "max")]
        [InlineData(TaskType.Count, "a != null", "число индексов", "|{ i ∈")]
        public void ShowAllFormalRequirements_VariousTasks_ReturnsAllComponents(
            TaskType taskType, string expectedPre, string expectedInv, string expectedPost)
        {
            // Arrange
            int i = 2;
            int j = 5;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.Equal(5, result.Length);
            Assert.Contains(expectedPre, result[0]);
            Assert.Contains(expectedInv, result[1]);
            Assert.Contains(expectedPost, result[2]);
            Assert.Contains("Pre => Inv", result[3]);
        }

        [Fact]
        public void ShowAllFormalRequirements_CountTask_ReturnsCorrectTFunction()
        {
            // Arrange
            TaskType taskType = TaskType.Count;
            int i = 2;
            int j = 5;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.Equal("3", result[4]); // T = 5 - 2 = 3
        }

        /// <summary>
        /// Проверяет поиск максимума с отрицательными числами
        /// </summary>
        [Fact]
        public void IntegrationTest_MaxOperationWithNegativeNumbers()
        {
            // Arrange
            int[] array = { -5, -2, -10, -1 };
            TaskType taskType = TaskType.Max;

            // Act
            var (post, result) = Class1.MakeOperation(array, taskType);

            // Assert
            Assert.True(post);
            Assert.Equal(-1, result);
        }

        /// <summary>
        /// Проверяет обработку массива из одного элемента
        /// </summary>
        [Fact]
        public void BoundaryTest_SingleElementArray()
        {
            // Arrange
            int[] array = { 42 };
            TaskType taskType = TaskType.Sum;

            // Act
            var (post, result) = Class1.MakeOperation(array, taskType);

            // Assert
            Assert.True(post);
            Assert.Equal(42, result);
        }

        /// <summary>
        /// Проверяет производительность на большом массиве
        /// </summary>
        [Fact]
        public void PerformanceTest_LargeArray()
        {
            // Arrange
            int[] array = Class1.GenerateNSizeArray(1000, 0, 100);
            TaskType taskType = TaskType.Sum;

            // Act
            var (post, result) = Class1.MakeOperation(array, taskType);

            // Assert
            Assert.True(post);
            Assert.InRange(result, 0, 1000 * 99); // Максимально возможная сумма
        }

        /// <summary>
        /// Проверяет что метод возвращает не пустую строку
        /// </summary>
        [Fact]
        public void ShowWpFormula_ReturnsNonEmptyString()
        {
            // Act
            string result = ClassFormalRequirements.ShowWpFormula();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// Проверяет что формула содержит корректные логические выражения
        /// </summary>
        [Fact]
        public void ShowWpFormula_ContainsCorrectFormula()
        {
            // Act
            string result = ClassFormalRequirements.ShowWpFormula();

            // Assert
            Assert.Contains("(Inv ∧ B) => wp(S, Inv)", result);
            Assert.Contains("инвариант", result);
            Assert.Contains("условие продолжения", result);
        }

        /// <summary>
        /// Проверяет что для суммы возвращается 5 элементов
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_SumTask_ReturnsFiveElements()
        {
            // Arrange
            TaskType taskType = TaskType.Sum;
            int i = 2;
            int j = 5;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
        }

        /// <summary>
        /// Проверяет что для максимума возвращается 5 элементов
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_MaxTask_ReturnsFiveElements()
        {
            // Arrange
            TaskType taskType = TaskType.Max;
            int i = 0;
            int j = 10;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
        }

        /// <summary>
        /// Проверяет что для подсчета возвращается 5 элементов
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_CountTask_ReturnsFiveElements()
        {
            // Arrange
            TaskType taskType = TaskType.Count;
            int i = 3;
            int j = 8;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
        }

        /// <summary>
        /// Проверяет содержимое формальных требований для суммы
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_SumTask_CorrectContent()
        {
            // Arrange
            TaskType taskType = TaskType.Sum;
            int i = 2;
            int j = 5;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.Contains("n >= 0", result[0]); // Pre
            Assert.Contains("сумма", result[1]); // Inv
            Assert.Contains("Σ", result[2]); // Post
            Assert.Contains("Pre => Inv", result[3]); // VC
        }

        /// <summary>
        /// Проверяет содержимое формальных требований для максимума
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_MaxTask_CorrectContent()
        {
            // Arrange
            TaskType taskType = TaskType.Max;
            int i = 1;
            int j = 6;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.Contains("a.Length != 0", result[0]); // Pre
            Assert.Contains("максимальный", result[1]); // Inv
            Assert.Contains("max", result[2]); // Post
            Assert.Contains("Inv ∧ B", result[3]); // VC
        }

        /// <summary>
        /// Проверяет содержимое формальных требований для подсчета
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_CountTask_CorrectContent()
        {
            // Arrange
            TaskType taskType = TaskType.Count;
            int i = 2;
            int j = 7;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.Contains("a != null", result[0]); // Pre
            Assert.Contains("число индексов", result[1]); // Inv
            Assert.Contains("|{ i ∈", result[2]); // Post
            Assert.Contains("wp(S, Inv)", result[3]); // VC
        }

        /// <summary>
        /// Проверяет корректное вычисление T-функции для подсчета
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_CountTask_ReturnsCorrectTFunctionValue()
        {
            // Arrange
            TaskType taskType = TaskType.Count;
            int i = 3;
            int j = 8;
            int expectedT = 5; // 8 - 3 = 5

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.Equal(expectedT.ToString(), result[4]);
        }

        /// <summary>
        /// Проверяет что для суммы T-функция возвращает null
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_SumTask_ReturnsNullForTFunction()
        {
            // Arrange
            TaskType taskType = TaskType.Sum;
            int i = 3;
            int j = 8;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.Null(result[4]);
        }

        /// <summary>
        /// Проверяет что для максимума T-функция возвращает null
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_MaxTask_ReturnsNullForTFunction()
        {
            // Arrange
            TaskType taskType = TaskType.Max;
            int i = 3;
            int j = 8;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.Null(result[4]);
        }

        /// <summary>
        /// Проверяет что условия верификации содержат все необходимые элементы
        /// </summary>
        [Fact]
        public void VerificationConditions_AlwaysContainsRequiredElements()
        {
            // Arrange
            TaskType taskType = TaskType.Sum;
            int i = 2;
            int j = 5;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);
            string vc = result[3];

            // Assert
            Assert.Contains("Pre => Inv", vc);
            Assert.Contains("(Inv ∧ B) => wp(S, Inv)", vc);
            Assert.Contains("(Inv ∧ !B) => Post", vc);
            Assert.Contains("Inv ∧ B => t>=0", vc);
            Assert.Contains("Inv ∧ B => wp(S, t' < t)", vc);
        }

        /// <summary>
        /// Проверяет интеграцию формальных требований с реальным циклом
        /// </summary>
        [Fact]
        public void Integration_FormalRequirementsWithActualCycle()
        {
            // Arrange
            int[] array = { 1, 2, 3, 4, 5 };
            TaskType taskType = TaskType.Sum;
            Cycle cycle = Class1.InitializeCycle(array, taskType, 0);

            // Act
            string[] formalReqs = ClassFormalRequirements.ShowAllFormalRequirements(
                cycle.taskType, cycle.i, cycle.array.Length);

            // Assert
            Assert.NotNull(formalReqs);
            Assert.Equal(5, formalReqs.Length);
            Assert.Contains("n >= 0", formalReqs[0]);
        }

        /// <summary>
        /// Проверяет формальные требования для массива нулевой длины
        /// </summary>
        [Fact]
        public void Boundary_ZeroLengthArrayFormalRequirements()
        {
            // Arrange
            TaskType taskType = TaskType.Sum;
            int i = 0;
            int j = 0;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
            Assert.Contains("n >= 0", result[0]); // Pre-condition для пустого массива
        }

        /// <summary>
        /// Проверяет формальные требования для массива из одного элемента
        /// </summary>
        [Fact]
        public void Boundary_SingleElementArrayFormalRequirements()
        {
            // Arrange
            TaskType taskType = TaskType.Max;
            int i = 0;
            int j = 1;

            // Act
            string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
            Assert.Contains("a.Length != 0", result[0]); // Pre-condition
            Assert.Contains("максимальный", result[1]); // Invariant
        }

        /// <summary>
        /// Проверяет согласованность поведения для разных индексов
        /// </summary>
        [Fact]
        public void ShowAllFormalRequirements_WithDifferentIndices_ConsistentBehavior()
        {
            // Arrange
            TaskType taskType = TaskType.Count;
            int[][] testCases = {
            new[] { 0, 10 },  // начало массива
            new[] { 5, 10 },  // середина массива  
            new[] { 9, 10 },  // конец массива
            new[] { 10, 10 }  // граничный случай
        };

            foreach (var testCase in testCases)
            {
                int i = testCase[0];
                int j = testCase[1];

                // Act
                string[] result = ClassFormalRequirements.ShowAllFormalRequirements(taskType, i, j);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(5, result.Length);

                if (taskType == TaskType.Count)
                {
                    int expectedT = j - i;
                    Assert.Equal(expectedT.ToString(), result[4]);
                }
            }
        }
    }
}


