using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.ComponentModel;
using WpfApp1.Algorithms.MergeSort;

namespace WpfApp1.ViewModels
{
    public class ArrayViewModel : BaseAlgorithmViewModel
    {
        private int[] array;
        private bool isVisualizationRunning;

        // Константи для розмірів елементів
        private const double ELEMENT_SIZE = 40;
        private const double SPACING = 5;
        private const double COMPARISON_SIZE = 50;
        private const double COMPARISON_SPACING = 20;

        public ArrayViewModel()
        {
            array = Array.Empty<int>();
            isVisualizationRunning = false;
        }

        public override void Initialize(int dimension)
        {
            array = new int[dimension];
            Random random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(1, 100);
            }
            
            if (inputCanvas != null)
            {
                var elements = CreateVisualizationElements(array);
                Display(inputCanvas, elements, "Початковий масив");
            }
        }

        protected override void InitializeVisualization()
        {
            if (inputCanvas != null)
            {
                var elements = CreateVisualizationElements(array);
                Display(inputCanvas, elements, "Початковий масив");
            }
        }

        public override void Display(Canvas canvas, IEnumerable<VisualizationElement> elements, string description)
        {
            if (canvas == null) return;

            canvas.Children.Clear();

            double canvasWidth = canvas.ActualWidth;
            double canvasHeight = canvas.ActualHeight;
            
            var elementsList = elements.ToList();
            int totalElements = elementsList.Count;
            
            // Розрахунок розмірів та відступів для основного масиву
            double totalWidth = totalElements * (ELEMENT_SIZE + SPACING) - SPACING;
            double startX = (canvasWidth - totalWidth) / 2;
            double startY = canvasHeight - ELEMENT_SIZE - 50; // Зміщуємо основний масив нижче

            // Відображаємо основний масив
            foreach (var element in elementsList)
            {
                DisplayArrayElement(canvas, element, startX, startY);
            }

            // Перевіряємо, чи є елементи для порівняння
            if (elements is IEnumerable<VisualizationElement> visualElements)
            {
                var step = visualElements.FirstOrDefault()?.Step;
                if (step is MergeSortStep mergeSortStep && 
                    mergeSortStep.FirstComparisonElement != null && 
                    mergeSortStep.SecondComparisonElement != null)
                {
                    DisplayComparisonElements(canvas, mergeSortStep.FirstComparisonElement, mergeSortStep.SecondComparisonElement);
                }
            }
        }

        private void DisplayArrayElement(Canvas canvas, VisualizationElement element, double startX, double startY)
        {
            // Створюємо прямокутник для елемента
            var rect = new Rectangle
            {
                Width = ELEMENT_SIZE,
                Height = ELEMENT_SIZE,
                Fill = element.Color,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            // Додаємо анімацію для плавної зміни кольору
           

            Canvas.SetLeft(rect, startX + element.Index * (ELEMENT_SIZE + SPACING));
            Canvas.SetTop(rect, startY);
            canvas.Children.Add(rect);

            // Додаємо текст значення
            var text = new TextBlock
            {
                Text = element.Value.ToString(),
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                Width = ELEMENT_SIZE,
                TextWrapping = TextWrapping.Wrap
            };

            Canvas.SetLeft(text, startX + element.Index * (ELEMENT_SIZE + SPACING));
            Canvas.SetTop(text, startY + (ELEMENT_SIZE - text.FontSize) / 2);
            canvas.Children.Add(text);
        }

        private void DisplayComparisonElements(Canvas canvas, VisualizationElement first, VisualizationElement second)
        {
            double canvasWidth = canvas.ActualWidth;
            const double COMPARISON_Y_OFFSET = 100; // Відступ зверху для елементів порівняння

            // Розраховуємо позиції для елементів порівняння
            double totalWidth = 2 * ELEMENT_SIZE + SPACING;
            double startX = (canvasWidth - totalWidth) / 2;
            double startY = COMPARISON_Y_OFFSET;

            // Відображаємо перший елемент порівняння
            var firstRect = new Rectangle
            {
                Width = ELEMENT_SIZE,
                Height = ELEMENT_SIZE,
                Fill = first.Color,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(firstRect, startX);
            Canvas.SetTop(firstRect, startY);
            canvas.Children.Add(firstRect);

            var firstText = new TextBlock
            {
                Text = first.Value.ToString(),
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                Width = ELEMENT_SIZE,
                TextWrapping = TextWrapping.Wrap
            };
            Canvas.SetLeft(firstText, startX);
            Canvas.SetTop(firstText, startY + (ELEMENT_SIZE - firstText.FontSize) / 2);
            canvas.Children.Add(firstText);

            // Відображаємо другий елемент порівняння
            var secondRect = new Rectangle
            {
                Width = ELEMENT_SIZE,
                Height = ELEMENT_SIZE,
                Fill = second.Color,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(secondRect, startX + ELEMENT_SIZE + SPACING);
            Canvas.SetTop(secondRect, startY);
            canvas.Children.Add(secondRect);

            var secondText = new TextBlock
            {
                Text = second.Value.ToString(),
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                Width = ELEMENT_SIZE,
                TextWrapping = TextWrapping.Wrap
            };
            Canvas.SetLeft(secondText, startX + ELEMENT_SIZE + SPACING);
            Canvas.SetTop(secondText, startY + (ELEMENT_SIZE - secondText.FontSize) / 2);
            canvas.Children.Add(secondText);

        }

        public override void StartVisualization()
        {
            isVisualizationRunning = true;
        }

        public override void PauseVisualization()
        {
            isVisualizationRunning = false;
        }

        public override void Clear()
        {
            array = Array.Empty<int>();
            isVisualizationRunning = false;
            if (inputCanvas != null)
                inputCanvas.Children.Clear();
            if (outputCanvas != null)
                outputCanvas.Children.Clear();
        }

        public override void StartManualSolution()
        {
            // Implementation for manual solution if needed
        }

        public static IEnumerable<VisualizationElement> CreateVisualizationElements(int[] array)
        {
            return array.Select((value, index) => new VisualizationElement(value, index));
        }
    }
} 