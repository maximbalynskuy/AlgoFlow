using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1.Views.Visualizations
{
    public class MatrixVisualization : IVisualization
    {
        public Canvas InputCanvas { get; set; }
        public Canvas OutputCanvas { get; set; }
        private readonly int[,] matrix;
        private const int CELL_SIZE = 40;
        private const int SPACING = 5;

        public MatrixVisualization(int[,] matrix, Canvas inputCanvas, Canvas outputCanvas)
        {
            this.matrix = matrix;
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
            DrawMatrix(InputCanvas, matrix);
        }

        public void Update()
        {
            DrawMatrix(OutputCanvas, matrix);
        }

        private void DrawMatrix(Canvas canvas, int[,] matrix)
        {
            canvas.Children.Clear();
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double totalWidth = cols * (CELL_SIZE + SPACING) - SPACING;
            double totalHeight = rows * (CELL_SIZE + SPACING) - SPACING;
            double startX = (canvas.ActualWidth - totalWidth) / 2;
            double startY = (canvas.ActualHeight - totalHeight) / 2;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Create cell rectangle
                    Rectangle rect = new Rectangle
                    {
                        Width = CELL_SIZE,
                        Height = CELL_SIZE,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };

                    // Create text
                    TextBlock text = new TextBlock
                    {
                        Text = matrix[i, j].ToString(),
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = CELL_SIZE,
                        Height = CELL_SIZE
                    };

                    // Position elements
                    double x = startX + j * (CELL_SIZE + SPACING);
                    double y = startY + i * (CELL_SIZE + SPACING);
                    Canvas.SetLeft(rect, x);
                    Canvas.SetTop(rect, y);
                    Canvas.SetLeft(text, x);
                    Canvas.SetTop(text, y);

                    // Add to canvas
                    canvas.Children.Add(rect);
                    canvas.Children.Add(text);

                    // Add row/column indices
                    if (j == 0) // Row indices
                    {
                        TextBlock rowIndex = new TextBlock
                        {
                            Text = i.ToString(),
                            TextAlignment = TextAlignment.Center,
                            Width = 20
                        };
                        Canvas.SetLeft(rowIndex, startX - 25);
                        Canvas.SetTop(rowIndex, y + CELL_SIZE/2 - 10);
                        canvas.Children.Add(rowIndex);
                    }
                    if (i == 0) // Column indices
                    {
                        TextBlock colIndex = new TextBlock
                        {
                            Text = j.ToString(),
                            TextAlignment = TextAlignment.Center,
                            Width = CELL_SIZE
                        };
                        Canvas.SetLeft(colIndex, x);
                        Canvas.SetTop(colIndex, startY - 25);
                        canvas.Children.Add(colIndex);
                    }
                }
            }
        }
    }
} 