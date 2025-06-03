using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

namespace WpfApp1.ViewModels
{
    public interface IAlgorithmViewModel : INotifyPropertyChanged
    {
        void Initialize(int dimension);
        void SetCanvases(Canvas inputCanvas, Canvas outputCanvas);
        void StartVisualization();
        void StartManualSolution();
        void PauseVisualization();
        void Clear();
        void Display(Canvas canvas, IEnumerable<VisualizationElement> elements, string description);
    }
} 