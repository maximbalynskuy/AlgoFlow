namespace WpfApp1.ViewModels
{
    public enum AlgorithmType
    {
        MergeSort,
        Dijkstra,
        Gauss,
        FloydWarshall,
        TreeTraversal,
        DFS,
        KadaneAlgorithm
    }

    public class AlgorithmViewModelFactory
    {
        public static IAlgorithmViewModel CreateViewModel(AlgorithmType algorithmType)
        {
            return algorithmType switch
            {
                AlgorithmType.MergeSort => new ArrayViewModel(),
                AlgorithmType.KadaneAlgorithm => new ArrayViewModel(),
                AlgorithmType.TreeTraversal => new TreeViewModel(),
                AlgorithmType.DFS => new TreeViewModel(),
                AlgorithmType.Dijkstra => new MatrixViewModel(),
                AlgorithmType.FloydWarshall => new MatrixViewModel(),
                AlgorithmType.Gauss => new MatrixViewModel(),
                _ => throw new ArgumentException("Unknown algorithm type", nameof(algorithmType))
            };
        }
    }
} 