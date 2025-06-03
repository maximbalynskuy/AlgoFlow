using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Collections.Generic;
using WpfApp1.Views.Visualizations;

namespace WpfApp1.ViewModels
{
    public abstract class BaseAlgorithmViewModel : IAlgorithmViewModel
    {
        protected IVisualization? visualization;
        protected Canvas? inputCanvas;
        protected Canvas? outputCanvas;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void SetCanvases(Canvas input, Canvas output)
        {
            inputCanvas = input;
            outputCanvas = output;
            InitializeVisualization();
        }

        protected abstract void InitializeVisualization();

        public abstract void Initialize(int dimension);
        public abstract void StartVisualization();
        public abstract void PauseVisualization();
        public abstract void Clear();
        public abstract void StartManualSolution();

        public virtual void Display(Canvas canvas, IEnumerable<VisualizationElement> elements, string description)
        {
            // Base implementation that can be overridden by derived classes if needed
            canvas.Children.Clear();
            
            // Default visualization logic can be implemented here if needed
            // Derived classes can override this method to provide specific visualization logic
        }
    }
} 