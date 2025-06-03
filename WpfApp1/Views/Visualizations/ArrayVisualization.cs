using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1.Views.Visualizations
{
    public class ArrayVisualization : IVisualization
    {
        public Canvas InputCanvas { get; set; }
        public Canvas OutputCanvas { get; set; }
        private int[] array;
        private const int RECT_WIDTH = 50;
        private const int RECT_HEIGHT = 30;
        private const int SPACING = 10;

        public ArrayVisualization(int[] array, Canvas inputCanvas, Canvas outputCanvas)
        {
            this.array = array;
            InputCanvas = inputCanvas;
            OutputCanvas = outputCanvas;
        }

        public void Clear()
        {
            InputCanvas.Children.Clear();
            OutputCanvas.Children.Clear();
        }

        public void Draw()
        {
            Clear();
            DrawArray(InputCanvas, array);
        }

        public void Update()
        {
            DrawArray(OutputCanvas, array);
        }

        public void UpdateOutput(int[] newArray)
        {
            array = newArray;
            DrawArray(OutputCanvas, array);
        }

        private void DrawArray(Canvas canvas, int[] arr)
        {
            canvas.Children.Clear();
            double startX = (canvas.ActualWidth - (arr.Length * (RECT_WIDTH + SPACING))) / 2;
            double startY = canvas.ActualHeight / 2 - RECT_HEIGHT;

            for (int i = 0; i < arr.Length; i++)
            {
                // Create rectangle
                Rectangle rect = new Rectangle
                {
                    Width = RECT_WIDTH,
                    Height = RECT_HEIGHT,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                // Create text
                TextBlock text = new TextBlock
                {
                    Text = arr[i].ToString(),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = RECT_WIDTH,
                    Height = RECT_HEIGHT
                };

                // Position elements
                Canvas.SetLeft(rect, startX + i * (RECT_WIDTH + SPACING));
                Canvas.SetTop(rect, startY);
                Canvas.SetLeft(text, startX + i * (RECT_WIDTH + SPACING));
                Canvas.SetTop(text, startY);

                // Add to canvas
                canvas.Children.Add(rect);
                canvas.Children.Add(text);

                // Add index below
                TextBlock indexText = new TextBlock
                {
                    Text = i.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Width = RECT_WIDTH
                };
                Canvas.SetLeft(indexText, startX + i * (RECT_WIDTH + SPACING));
                Canvas.SetTop(indexText, startY + RECT_HEIGHT + 5);
                canvas.Children.Add(indexText);
            }
        }
    }
} 