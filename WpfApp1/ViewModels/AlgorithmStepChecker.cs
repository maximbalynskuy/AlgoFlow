using System;
using System.Collections.Generic;
using WpfApp1.Algorithms;
using WpfApp1.Algorithms.MergeSort;

namespace WpfApp1.ViewModels
{
    public class AlgorithmStepChecker
    {
        private readonly List<IAlgorithmStep> steps;
        private int currentStepIndex;
        private bool isManualMode;

        public event EventHandler<StepCheckResult> StepChecked;
        public event EventHandler ManualModeCompleted;

        public AlgorithmStepChecker(IEnumerable<IAlgorithmStep> algorithmSteps)
        {
            steps = new List<IAlgorithmStep>(algorithmSteps);
            currentStepIndex = -1;
            isManualMode = false;
        }

        public void StartManualMode()
        {
            currentStepIndex = -1;
            isManualMode = true;
        }

        public void StopManualMode()
        {
            isManualMode = false;
            currentStepIndex = -1;
        }

        public StepCheckResult CheckStep(StepType stepType)
        {
            if (!isManualMode || currentStepIndex >= steps.Count - 1)
                return new StepCheckResult(false, null, "Ручний режим не активний або всі кроки завершено");

            var nextStep = steps[currentStepIndex + 1];
            bool isCorrect = false;
            string message = "";

            // Перевірка кроків порівняння
            if (stepType == StepType.Greater || stepType == StepType.Less)
            {
                if (nextStep is MergeSortStep mergeSortStep && 
                    mergeSortStep.FirstComparisonElement != null && 
                    mergeSortStep.SecondComparisonElement != null)
                {
                    bool isActuallyGreater = mergeSortStep.FirstComparisonElement.Value > mergeSortStep.SecondComparisonElement.Value;
                    
                    if ((stepType == StepType.Greater && isActuallyGreater) ||
                        (stepType == StepType.Less && !isActuallyGreater))
                    {
                        isCorrect = true;
                        message = "Правильне порівняння!";
                    }
                    else
                    {
                        message = "Неправильне порівняння. Спробуйте ще раз.";
                    }
                }
            }
            else
            {
                // Перевірка інших типів кроків
                isCorrect = nextStep.Type == stepType;
                message = isCorrect ? "Правильний крок!" : "Неправильний крок. Спробуйте ще раз.";
            }

            if (isCorrect)
            {
                currentStepIndex++;
                if (currentStepIndex == steps.Count - 1)
                {
                    isManualMode = false;
                    ManualModeCompleted?.Invoke(this, EventArgs.Empty);
                }
            }

            var result = new StepCheckResult(isCorrect, nextStep, message);
            StepChecked?.Invoke(this, result);
            return result;
        }

        public bool IsManualModeActive => isManualMode;
        public int TotalSteps => steps.Count;
        public int CurrentStep => currentStepIndex + 1;
    }

    public class StepCheckResult
    {
        public bool IsCorrect { get; }
        public IAlgorithmStep Step { get; }
        public string Message { get; }

        public StepCheckResult(bool isCorrect, IAlgorithmStep step, string message)
        {
            IsCorrect = isCorrect;
            Step = step;
            Message = message;
        }
    }
} 