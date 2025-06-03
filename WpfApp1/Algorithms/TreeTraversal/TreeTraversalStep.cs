using System.Collections.Generic;
using WpfApp1.ViewModels;

namespace WpfApp1.Algorithms.TreeTraversal
{
    public class TreeTraversalStep : IAlgorithmStep
    {
        public string Description { get; }
        public StepType Type { get; }
        public IEnumerable<VisualizationElement> Elements { get; }

        public TreeTraversalStep(string description, StepType type, IEnumerable<VisualizationElement> elements)
        {
            Description = description;
            Type = type;
            Elements = elements;
        }

        public object GetData()
        {
            return new { Elements, Description };
        }
    }
} 