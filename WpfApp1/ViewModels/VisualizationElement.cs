using System.Windows.Media;
using WpfApp1.Algorithms;

namespace WpfApp1.ViewModels
{
    public class VisualizationElement
    {
        public int Value { get; }
        public int Index { get; }
        public Brush Color { get; }
        public IAlgorithmStep? Step { get; }

        public VisualizationElement(int value, int index, Brush? color = null, IAlgorithmStep? step = null)
        {
            Value = value;
            Index = index;
            Color = color ?? Brushes.White;
            Step = step;
        }
    }
} 