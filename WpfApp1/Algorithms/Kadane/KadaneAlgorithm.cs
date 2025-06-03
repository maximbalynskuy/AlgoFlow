using System;
using System.Collections.Generic;
using WpfApp1.ViewModels;

namespace WpfApp1.Algorithms.Kadane
{
    public class KadaneAlgorithm : IAlgorithm
    {
        private List<IAlgorithmStep> steps;
        private int currentStepIndex;

        public KadaneAlgorithm()
        {
            steps = new List<IAlgorithmStep>();
            currentStepIndex = -1;
        }

        public void Initialize(object input)
        {
            steps.Clear();
            currentStepIndex = -1;
        }

        public IEnumerable<IAlgorithmStep> GetSteps()
        {
            return steps;
        }

        public IAlgorithmStep? NextStep()
        {
            return null;
        }

        public IAlgorithmStep? PreviusStep()
        {
            return null;
        }

        public object GetCurrentState()
        {
            return null;
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