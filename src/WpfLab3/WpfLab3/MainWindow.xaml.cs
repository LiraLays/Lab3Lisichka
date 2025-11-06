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
using ClassLab3;

namespace WpfLab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int arraySize;
        public int[] actualArray;
        public Class1.TaskType checkedTaskType = Class1.TaskType.None;
        public Class1.Cycle ActualCycle;
        public int actualT;
        public bool CycleStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            SliderSizeArray.ValueChanged += SliderSizeArray_Changed;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void ParseArrayAndT()
        {
            actualArray = TextBoxArrayElems.Text.Trim().Split(", ").Select(x => int.Parse(x)).ToArray();
            actualT = checkedTaskType == Class1.TaskType.Count ? int.Parse(TextBoxT.Text.Trim()) : 0; // Добавить провеки
            //MessageBox.Show($"{actualT} > {actualArray[0]} = {actualT > actualArray[0]}");
        }

        private void ButtonMakeStep_Click(object sender, RoutedEventArgs e)
        {
            if (!CycleStarted)
            {
                ParseArrayAndT();
                if (checkedTaskType == Class1.TaskType.None)
                {
                    MessageBox.Show("Выберите тип операции с помощью RadioButton");
                    return;
                }
                ActualCycle = Class1.InitializeCycle(actualArray, checkedTaskType, actualT);
                CycleStarted = true;
            }
            int? i, j, res;
            (i, j, res) = Class1.MakeStep(ActualCycle.i, ActualCycle.res, ActualCycle.array ,ActualCycle.taskType, ActualCycle.T);
            if (i == null && j == null && res == null)
            {
                MessageBox.Show($"Цикл закончен! \n Значение res: {ActualCycle.res}");
                return;
            }
            else
            {
                ActualCycle.i++;
                ActualCycle.res = (int)res;
                TextBoxArrayChanges.Text = $"Изменяемая ячейка: {ActualCycle.array[(int) i]}\n" +
                    $"Индекс j: {j}\n" +
                    $"Значение res: {ActualCycle.res}";
            }
        }

        public void ChangeStackPanelVisibilty(bool visibilty)
        {
            StackPanelTInfo.Visibility = visibilty ? Visibility.Visible : Visibility.Hidden; 
        }

        private void RadioButtonSum_Checked(object sender, RoutedEventArgs e)
        {
            ChangeStackPanelVisibilty(false);
            checkedTaskType = Class1.TaskType.Sum;
        }
        private void RadioButtonMax_Checked(object sender, RoutedEventArgs e)
        {
            ChangeStackPanelVisibilty(false);
            checkedTaskType = Class1.TaskType.Max;
        }
        private void RadioButtonCount_Checked(object sender, RoutedEventArgs e)
        {
            ChangeStackPanelVisibilty(true);
            checkedTaskType = Class1.TaskType.Count;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            ParseArrayAndT();
            (bool post, int res) = Class1.MakeOperation(actualArray, checkedTaskType, actualT);
            TextBoxArrayChanges.Text = $"Значение POST: {post}\n" +
                $"Значение res: {res}";
        }
    }
}