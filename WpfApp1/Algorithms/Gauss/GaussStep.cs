using System.Collections.Generic;
using WpfApp1.ViewModels;

namespace WpfApp1.Algorithms.Gauss
{
    public class GaussStep : IAlgorithmStep
    {
        public string Description { get; }
        public StepType Type { get; }
        public IEnumerable<VisualizationElement> Elements { get; }

        public GaussStep(string description, StepType type, IEnumerable<VisualizationElement> elements)
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