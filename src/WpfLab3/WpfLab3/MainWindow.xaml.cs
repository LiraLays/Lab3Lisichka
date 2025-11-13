using ClassLab3;
using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfLab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SolidColorBrush RED = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA33D3D"));
        private SolidColorBrush GREEN = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3DA354"));

        private int arraySize;
        private int[] actualArray;
        private Class1.TaskType checkedTaskType = Class1.TaskType.None;
        private Class1.Cycle ActualCycle;
        private int actualT;
        private bool CycleStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            SliderSizeArray.ValueChanged += SliderSizeArray_Changed;
        }

        private void ButtonGenerateArray_Click(object sender, RoutedEventArgs e)
        {
            int[] array = Class1.GenerateNSizeArray(arraySize, 0, 100);
            TextBoxArrayElems.Text = string.Join(", ", array);
        }

        private void SliderSizeArray_Changed(object sender, RoutedEventArgs e) 
        {
            LabelSizeArray.Content = (int) SliderSizeArray.Value;
            arraySize = (int) SliderSizeArray.Value;
        }

        private bool ParseArrayAndT()
        {
            if (TextBoxT.Text == string.Empty && checkedTaskType == Class1.TaskType.Count)
            {
                MessageBox.Show("Для начала работы введите значение в поле T");
                return false;
            }
            try
            {
                actualArray = TextBoxArrayElems.Text.Trim().Split(", ").Select(x => int.Parse(x)).ToArray();
                actualT = checkedTaskType == Class1.TaskType.Count ? int.Parse(TextBoxT.Text.Trim()) : 0; // Добавить провеки
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void ButtonMakeStep_Click(object sender, RoutedEventArgs e)
        {
            
            if (!CycleStarted)
            {
                if (!ParseArrayAndT()) return;
                ShowWp();
                ProgressBar.Maximum = actualArray.Length;
                ProgressBar.Value = actualArray.Length;
                if (checkedTaskType == Class1.TaskType.None)
                {
                    MessageBox.Show("Выберите тип операции с помощью RadioButton");
                    return;
                }
                ActualCycle = Class1.InitializeCycle(actualArray, checkedTaskType, actualT);
                CycleStarted = true;
                string InvariantText, InvariantFormula;
                (InvariantText, InvariantFormula) = Class1.InvariantType(ActualCycle.taskType, ActualCycle.array, ActualCycle.T);
                InvariantTextBox.Text = $"{InvariantText}\n" +
                    $"{InvariantFormula}";
            }
            int? i, j, res; bool post;
            (i, j, res, post) = Class1.MakeStep(ActualCycle.post, ActualCycle.i, ActualCycle.res, ActualCycle.array ,ActualCycle.taskType, ActualCycle.T);
            if (i == null && j == null && res == null)
            {
                LoopIndicator.Fill = post ? GREEN : RED;
                TextBoxArrayChanges.Text = $"Цикл закончен!\n" +
                    $"Значение POST: {post}\n" +
                    $"Значение res: {ActualCycle.res}";
                MessageBox.Show($"Цикл закончен! \n Значение res: {ActualCycle.res}");
                CycleStarted = false;
            }
            else
            {
                ActualCycle.i++;
                ActualCycle.res = (int) res;
                TextBoxArrayChanges.Text = $"Изменяемая ячейка: {ActualCycle.array[(int) i]}\n" +
                    $"Индекс j: {j}\n" +
                    $"Значение POST: {post}\n" +
                    $"Значение res: {ActualCycle.res}";
                LoopIndicator.Fill = post ? GREEN : RED;
                InvariantIndicator.Fill = GREEN;
                ProgressBar.Value = Class1.VariantFunctionT(ActualCycle.i, ActualCycle.array.Length);
            }
        }

        public void ChangeStackPanelVisibilty(bool visibilty)
        {
            StackPanelTInfo.Visibility = visibilty ? Visibility.Visible : Visibility.Hidden; 
        }

        private void RadioButtonSum_Checked(object sender, RoutedEventArgs e)
        {
            ChangeStackPanelVisibilty(false);
            TextBoxT.Visibility = Visibility.Hidden;
            checkedTaskType = Class1.TaskType.Sum;
        }
        private void RadioButtonMax_Checked(object sender, RoutedEventArgs e)
        {
            ChangeStackPanelVisibilty(false);
            TextBoxT.Visibility = Visibility.Hidden;
            checkedTaskType = Class1.TaskType.Max;
        }
        private void RadioButtonCount_Checked(object sender, RoutedEventArgs e)
        {
            ChangeStackPanelVisibilty(true);
            TextBoxT.Text = "";
            TextBoxT.Visibility = Visibility.Visible;
            checkedTaskType = Class1.TaskType.Count;
        }

        private void ShowWp()
        {
            FormulaTextBox.Text = ClassFormalRequirements.ShowWpFormula();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (!ParseArrayAndT()) return;
            ShowWp();
            ProgressBar.Maximum = actualArray.Length;
            ProgressBar.Value = actualArray.Length;
            string InvariantText, InvariantFormula;
            (InvariantText, InvariantFormula) = Class1.InvariantType(checkedTaskType, actualArray, actualT);
            InvariantTextBox.Text = $"{InvariantText}\n" +
                    $"{InvariantFormula}";
            (bool post, int res) = Class1.MakeOperation(actualArray, checkedTaskType, actualT);
            TextBoxArrayChanges.Text = $"Значение POST: {post}\n" +
                $"Значение res: {res}";
            LoopIndicator.Fill = post ? GREEN : RED;
            InvariantIndicator.Fill = GREEN;
            ProgressBar.Value = 0;
            CycleStarted = false;
        }

        private void ButtonShowFormal_Click(object sender, RoutedEventArgs e)
        {
            if (!CycleStarted)
            {
                MessageBox.Show("Для отображения формальных требований начните цикл вычисления по шагам");
                return;
            }
            else
            {
                int actualSize = ActualCycle.array.Length;
                string[] strings = ClassFormalRequirements.ShowAllFormalRequirements(ActualCycle.taskType, ActualCycle.i, actualSize);
                MessageBox.Show(String.Join("\n", strings));
            }
        }

        private void ButtonReference_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.Owner = this;
            helpWindow.ShowDialog();
        }
    }
}