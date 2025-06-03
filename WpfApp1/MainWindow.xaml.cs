using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using WpfApp1.ViewModels;
using WpfApp1.Algorithms;
using WpfApp1.Algorithms.MergeSort;
using System.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAlgorithmViewModel? currentViewModel;
        private IAlgorithm? currentAlgorithm;
        private List<IAlgorithmStep> steps;
        private int currentStepIndex;
        private DispatcherTimer autoPlayTimer;
        private bool isAutoPlaying;
        private Random random;
        private int[] currentArray;
        private AlgorithmStepChecker? stepChecker;

        public MainWindow()
        {
            InitializeComponent();
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            steps = new List<IAlgorithmStep>();
            currentStepIndex = -1;
            random = new Random();
            ComboBox_SelectionChanged(null, null);
            prevStepButton.Visibility = Visibility.Collapsed;
            nextStepButton.Visibility = Visibility.Collapsed;
            autoPlayButton.Visibility = Visibility.Collapsed;
            
            autoPlayTimer = new DispatcherTimer();
            autoPlayTimer.Interval = TimeSpan.FromMilliseconds(500);
            autoPlayTimer.Tick += AutoPlayTimer_Tick;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearAll();
            var selectedIndex = comboBox.SelectedIndex;

            var algorithmType = (AlgorithmType)selectedIndex;
            currentViewModel = AlgorithmViewModelFactory.CreateViewModel(algorithmType);
            currentViewModel.SetCanvases(graphCanvasIn, graphCanvasIn);
            
            // Disable visualization buttons
            btnStart.IsEnabled = false;
            btnStartHand.IsEnabled = false;
        }

        private void btnInit_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtDimension1.Text, out int dimension))
            {
                MessageBox.Show("Будь ласка, введіть коректне значення розмірності");
                return;
            }

            if (dimension <= 0 || dimension > 100)
            {
                MessageBox.Show("Розмірність має бути в межах від 1 до 100");
                return;
            }

            if (comboBox.SelectedIndex == 0) // MergeSort
            {
                InitializeMergeSort(dimension);
            }
            else if (currentViewModel != null)
            {
                currentViewModel.Initialize(dimension);
            }

            // Enable visualization buttons after initialization
            btnStart.IsEnabled = true;
            btnStartHand.IsEnabled = true;
        }

        private int[] GenerateRandomArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(1, 100);
            }
            return array;
        }

        private void InitializeMergeSort(int size)
        {
            // Генеруємо випадковий масив
            currentArray = GenerateRandomArray(size);
            
            // Ініціалізуємо ViewModel для відображення масиву
            var arrayViewModel = currentViewModel as ArrayViewModel;
            if (arrayViewModel != null)
            {
                // Створюємо елементи візуалізації з базовим кольором
                var elements = ArrayViewModel.CreateVisualizationElements(currentArray);
                
                // Показуємо початковий масив
                arrayViewModel.Display(graphCanvasIn, elements, "Початковий масив");
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0) // MergeSort
            {
                if (currentArray == null || currentArray.Length == 0)
                {
                    MessageBox.Show("Спочатку згенеруйте масив!");
                    return;
                }

                // Ініціалізуємо алгоритм
                currentAlgorithm = new MergeSortAlgorithm();
                currentAlgorithm.Initialize(currentArray);
                
                // Отримуємо всі кроки
                steps = new List<IAlgorithmStep>(currentAlgorithm.GetSteps());
                currentStepIndex = -1;

                // Показуємо кнопки керування
                prevStepButton.Visibility = Visibility.Visible;
                nextStepButton.Visibility = Visibility.Visible;
                autoPlayButton.Visibility = Visibility.Visible;
                
                // Активуємо кнопки
                prevStepButton.IsEnabled = false;
                nextStepButton.IsEnabled = true;
                autoPlayButton.IsEnabled = true;

                // Скидаємо алгоритм
                currentAlgorithm.Reset();

                if (!isAutoPlaying)
                {
                    OnAutoPlayClick(sender, e);
                }
            }
            else
            {
                currentViewModel?.StartVisualization();
            }
        }

        private void btnStartHand_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0) // MergeSort
            {
                if (currentArray == null || currentArray.Length == 0)
                {
                    MessageBox.Show("Спочатку згенеруйте масив!");
                    return;
                }

                // Ініціалізуємо алгоритм
                currentAlgorithm = new MergeSortAlgorithm();
                currentAlgorithm.Initialize(currentArray);
                
                // Отримуємо всі кроки
                steps = new List<IAlgorithmStep>(currentAlgorithm.GetSteps());

                // Створюємо перевірку кроків
                stepChecker = new AlgorithmStepChecker(steps);
                stepChecker.StepChecked += StepChecker_StepChecked;
                stepChecker.ManualModeCompleted += StepChecker_ManualModeCompleted;
                stepChecker.StartManualMode();

                // Показуємо кнопки ручного режиму
                footer.Visibility = Visibility.Visible;
                manualStepButtons.Visibility = Visibility.Visible;
                
                // Оновлюємо повідомлення
                manualModeMessage.Text = "Виберіть правильний тип кроку";
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0) // MergeSort
            {
                if (isAutoPlaying)
                {
                    OnAutoPlayClick(sender, e);
                }
            }
            else
            {
                currentViewModel?.PauseVisualization();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            // Очищаємо канву
            graphCanvasIn.Children.Clear();
            
            // Скидаємо стан MergeSort
            steps.Clear();
            currentStepIndex = -1;
            currentArray = null;
            
            // Приховуємо кнопки керування
            prevStepButton.Visibility = Visibility.Collapsed;
            nextStepButton.Visibility = Visibility.Collapsed;
            autoPlayButton.Visibility = Visibility.Collapsed;
            
            // Деактивуємо кнопки керування
            prevStepButton.IsEnabled = false;
            nextStepButton.IsEnabled = false;
            autoPlayButton.IsEnabled = false;
            btnStart.IsEnabled = false;
            btnStartHand.IsEnabled = false;
            
            // Очищаємо опис
            stepDescription.Text = "";
            
            // Зупиняємо автоматичне відтворення
            if (isAutoPlaying)
            {
                autoPlayTimer.Stop();
                autoPlayButton.Content = "Авто";
                isAutoPlaying = false;
            }
            
            // Очищаємо поточний ViewModel
            currentViewModel?.Clear();
            
            // Очищаємо ручний режим
            if (stepChecker != null)
            {
                stepChecker.StepChecked -= StepChecker_StepChecked;
                stepChecker.ManualModeCompleted -= StepChecker_ManualModeCompleted;
                stepChecker = null;
            }
            footer.Visibility = Visibility.Visible;
            manualStepButtons.Visibility = Visibility.Collapsed;
            manualModeMessage.Text = "";
        }

        private void OnPrevStepClick(object sender, RoutedEventArgs e)
        {
            if (currentAlgorithm != null)
            {
                var step = currentAlgorithm.PreviusStep();
                if (step != null)
                {
                    ShowStep(step);
                    prevStepButton.IsEnabled = currentStepIndex > 0;
                    nextStepButton.IsEnabled = true;
                }
            }
        }

        private void OnNextStepClick(object sender, RoutedEventArgs e)
        {
            if (currentAlgorithm != null)
            {
                var step = currentAlgorithm.NextStep();
                if (step != null)
                {
                    ShowStep(step);
                    prevStepButton.IsEnabled = true;
                    nextStepButton.IsEnabled = !currentAlgorithm.IsComplete();
                    currentStepIndex++;
                }
            }
        }

        private void OnAutoPlayClick(object sender, RoutedEventArgs e)
        {
            if (isAutoPlaying)
            {
                autoPlayTimer.Stop();
                autoPlayButton.Content = "Авто";
                isAutoPlaying = false;
            }
            else
            {
                if (currentStepIndex < steps.Count - 1)
                {
                    autoPlayTimer.Start();
                    autoPlayButton.Content = "Пауза";
                    isAutoPlaying = true;
                }
            }
        }

        private void AutoPlayTimer_Tick(object sender, EventArgs e)
        {
            if (currentStepIndex < steps.Count - 1)
            {
                OnNextStepClick(null, null);
            }
            else
            {
                autoPlayTimer.Stop();
                autoPlayButton.Content = "Авто";
                isAutoPlaying = false;
            }
        }

        private void ShowStep(IAlgorithmStep step)
        {
            DisplayStep(step);
        }

        private void DisplayStep(IAlgorithmStep step)
        {
            stepDescription.Text = step.Description;
            if (currentViewModel != null)
            {
                if (step is MergeSortStep mergeSortStep)
                {
                    // Створюємо копію списку елементів з посиланням на крок
                    var allElements = mergeSortStep.Elements.Select(e => 
                        new VisualizationElement(e.Value, e.Index, e.Color, mergeSortStep)).ToList();
                    
                    // Відображаємо елементи через ViewModel
                    currentViewModel.Display(graphCanvasIn, allElements, step.Description);
                }
                else
                {
                    currentViewModel.Display(graphCanvasIn, step.Elements, step.Description);
                }
            }
        }

        private void StepChecker_StepChecked(object sender, StepCheckResult e)
        {
            manualModeMessage.Text = e.Message;
            if (e.IsCorrect && e.Step != null)
            {
                ShowStep(e.Step);
            }
        }

        private void StepChecker_ManualModeCompleted(object sender, EventArgs e)
        {
            MessageBox.Show("Вітаємо! Ви успішно виконали всі кроки алгоритму!");
            footer.Visibility = Visibility.Visible;
            manualStepButtons.Visibility = Visibility.Collapsed;
            manualModeMessage.Text = "";
        }

        private void OnManualStepClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string stepTypeStr)
            {
                if (Enum.TryParse<StepType>(stepTypeStr, out var stepType))
                {
                    stepChecker?.CheckStep(stepType);
                }
            }
        }
    }
}