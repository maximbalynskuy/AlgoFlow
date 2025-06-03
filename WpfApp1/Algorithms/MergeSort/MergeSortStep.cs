using System.Collections.Generic;
using WpfApp1.ViewModels;

namespace WpfApp1.Algorithms.MergeSort
{
    public class MergeSortStep : IAlgorithmStep
    {
        public string Description { get; }
        public StepType Type { get; }
        public IEnumerable<VisualizationElement> Elements { get; }
        public VisualizationElement? FirstComparisonElement { get; }
        public VisualizationElement? SecondComparisonElement { get; }

        public MergeSortStep(
            string description,
            StepType type,
            IEnumerable<VisualizationElement> elements,
            VisualizationElement? firstComparisonElement = null,
            VisualizationElement? secondComparisonElement = null)
        {
            Description = description;
            Type = type;
            Elements = elements;
            FirstComparisonElement = firstComparisonElement;
            SecondComparisonElement = secondComparisonElement;
        }

        public object GetData()
        {
            return new { Elements, Description };
        }
    }
} 