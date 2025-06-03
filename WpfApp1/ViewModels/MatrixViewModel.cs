using WpfApp1.Views.Visualizations;

namespace WpfApp1.ViewModels
{
    public class MatrixViewModel : BaseAlgorithmViewModel
    {
        private int[,] matrix;
        private bool isVisualizationRunning;

        public MatrixViewModel()
        {
            matrix = new int[0, 0];
            isVisualizationRunning = false;
        }

        protected override void InitializeVisualization()
        {
            if (inputCanvas != null && outputCanvas != null)
            {
                visualization = new MatrixVisualization(matrix, inputCanvas, outputCanvas);
                visualization.Draw();
            }
        }

        public override void Initialize(int dimension)
        {
            // By default create a square matrix, specific algorithms will override dimensions as needed
            matrix = new int[dimension, dimension];
            Random random = new Random();
            
            // Initialize matrix with random values
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    matrix[i, j] = random.Next(1, 100);
                }
            }
            InitializeVisualization();
        }

        public override void StartVisualization()
        {
            isVisualizationRunning = true;
            visualization?.Update();
        }

        public override void PauseVisualization()
        {
            isVisualizationRunning = false;
        }

        public override void Clear()
        {
            matrix = new int[0, 0];
            isVisualizationRunning = false;
            visualization?.Clear();
        }

        public override void StartManualSolution()
        {
            // Implement manual solution logic
            visualization?.Update();
        }

        // Protected method for derived classes to resize matrix as needed
        protected void ResizeMatrix(int rows, int cols)
        {
            matrix = new int[rows, cols];
            Random random = new Random();
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = random.Next(1, 100);
                }
            }
            InitializeVisualization();
        }
    }
} 