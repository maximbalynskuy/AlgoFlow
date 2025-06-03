using System.Windows.Controls;

namespace WpfApp1.Views.Visualizations
{
    public interface IVisualization
    {
        Canvas InputCanvas { get; set; }
        Canvas OutputCanvas { get; set; }
        void Clear();
        void Draw();
        void Update();
    }
} 