using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WpfApp1.ViewModels;

namespace WpfApp1.Algorithms.MergeSort
{
    public class MergeSortAlgorithm : IAlgorithm
    {
        private int[] array;
        private List<IAlgorithmStep> steps;
        private int currentStepIndex;

        public MergeSortAlgorithm()
        {
            array = Array.Empty<int>();
            steps = new List<IAlgorithmStep>();
            currentStepIndex = -1;
        }

        public void Initialize(object input)
        {
            if (input is int[] inputArray)
            {
                array = (int[])inputArray.Clone();
                steps = new List<IAlgorithmStep>();
                currentStepIndex = -1;
            }
            else
            {
                throw new ArgumentException("Input must be an integer array", nameof(input));
            }
        }

        public IEnumerable<IAlgorithmStep> GetSteps()
        {
            steps.Clear();
            currentStepIndex = -1;
            MergeSort(0, array.Length - 1);
            return steps;
        }

        private void MergeSort(int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;

                // Розділення масиву
                var elements = CreateVisualizationElements(array, left, right, mid);
                steps.Add(new MergeSortStep(
                    $"Розділення масиву на дві частини: [{left}..{mid}] та [{mid + 1}..{right}]",
                    StepType.SubProblem,
                    elements
                ));

                MergeSort(left, mid);
                MergeSort(mid + 1, right);
                Merge(left, mid, right);
            }
        }

        private void Merge(int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            int[] leftArray = new int[n1];
            int[] rightArray = new int[n2];

            Array.Copy(array, left, leftArray, 0, n1);
            Array.Copy(array, mid + 1, rightArray, 0, n2);

            // Додаємо крок для показу розділених підмасивів
            var initialElements = CreateVisualizationElements(array, left, right);
            steps.Add(new MergeSortStep(
                $"Підготовка до злиття підмасивів [{left}..{mid}] та [{mid + 1}..{right}]",
                StepType.SubProblem,
                initialElements
            ));

            int i = 0, j = 0, k = left;
            while (i < n1 && j < n2)
            {
                // Створюємо елементи для порівняння з правильними кольорами
                var elements = CreateVisualizationElements(array, left, right);
                var leftElement = new VisualizationElement(leftArray[i], left + i, Brushes.Yellow);
                var rightElement = new VisualizationElement(rightArray[j], mid + 1 + j, Brushes.Orange);

                // Оновлюємо кольори елементів, що порівнюються
                var updatedElements = elements.Select(e => {
                    if (e.Index == left + i)
                        return leftElement;
                    if (e.Index == mid + 1 + j)
                        return rightElement;
                    return e;
                }).ToList();

                steps.Add(new MergeSortStep(
                    $"Порівнюємо елементи: {leftArray[i]} та {rightArray[j]}",
                    StepType.Comparison,
                    updatedElements,
                    leftElement,
                    rightElement
                ));

                if (leftArray[i] <= rightArray[j])
                {
                    array[k] = leftArray[i];
                    i++;
                }
                else
                {
                    array[k] = rightArray[j];
                    j++;
                }

                // Додаємо крок після розміщення елемента
                var mergeElements = CreateVisualizationElements(array, left, right);
                steps.Add(new MergeSortStep(
                    $"Розміщуємо елемент {array[k]} на позицію {k}",
                    StepType.Merge,
                    mergeElements
                ));

                k++;
            }

            // Копіюємо залишки лівого підмасиву
            while (i < n1)
            {
                array[k] = leftArray[i];
                var elements = CreateVisualizationElements(array, left, right);
                steps.Add(new MergeSortStep(
                    $"Копіюємо залишковий елемент {leftArray[i]} з лівого підмасиву",
                    StepType.Merge,
                    elements
                ));
                i++;
                k++;
            }

            // Копіюємо залишки правого підмасиву
            while (j < n2)
            {
                array[k] = rightArray[j];
                var elements = CreateVisualizationElements(array, left, right);
                steps.Add(new MergeSortStep(
                    $"Копіюємо залишковий елемент {rightArray[j]} з правого підмасиву",
                    StepType.Merge,
                    elements
                ));
                j++;
                k++;
            }

            // Додаємо фінальний крок злиття
            var finalElements = CreateVisualizationElements(array, left, right);
            steps.Add(new MergeSortStep(
                $"Завершено злиття підмасивів [{left}..{mid}] та [{mid + 1}..{right}]",
                StepType.Merge,
                finalElements
            ));
        }

        private IEnumerable<VisualizationElement> CreateVisualizationElements(int[] arr, int left, int right, int? mid = null)
        {
            var elements = new List<VisualizationElement>();

            for (int i = 0; i < arr.Length; i++)
            {
                var color = Brushes.White;
                if (i >= left && i <= right)
                {
                    if (mid.HasValue)
                    {
                        if (i <= mid.Value)
                            color = Brushes.LightBlue;
                        else
                            color = Brushes.LightGreen;
                    }
                    else
                    {
                        color = Brushes.LightGray;
                    }
                }
                elements.Add(new VisualizationElement(arr[i], i, color));
            }

            return elements;
        }

        public IAlgorithmStep? NextStep()
        {
            if (IsComplete())
                return null;

            currentStepIndex++;
            return steps[currentStepIndex];
        }

        public IAlgorithmStep? PreviusStep()
        {
            if (!IsStarted())
                return null;

            currentStepIndex--;
            return steps[currentStepIndex];
        }

        public object GetCurrentState()
        {
            return array.Clone();
        }

        public bool IsComplete()
        {
            return currentStepIndex == steps.Count - 1;
        }

        public bool IsStarted()
        {
            return currentStepIndex >= 0;
        }

        public void Reset()
        {
            currentStepIndex = -1;
        }
    }
} 