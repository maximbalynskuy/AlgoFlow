using WpfApp1.Views.Visualizations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1.ViewModels
{
    public class TreeNode
    {
        public int Value { get; set; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }
    }

    public class TreeViewModel : BaseAlgorithmViewModel
    {
        private TreeNode? root;
        private bool isVisualizationRunning;

        public TreeViewModel()
        {
            root = null;
            isVisualizationRunning = false;
        }

        protected override void InitializeVisualization()
        {
            if (inputCanvas != null && outputCanvas != null)
            {
                visualization = new TreeVisualization(root, inputCanvas, outputCanvas);
                visualization.Draw();
            }
        }

        public override void Initialize(int dimension)
        {
            Random random = new Random();
            List<int> values = Enumerable.Range(1, dimension)
                                       .Select(_ => random.Next(1, 100))
                                       .ToList();
            root = null;
            foreach (var value in values)
            {
                root = InsertNode(root, value);
            }
            InitializeVisualization();
        }

        private TreeNode? InsertNode(TreeNode? node, int value)
        {
            // Якщо дерево порожнє, створюємо новий вузол
            if (node == null)
            {
                return new TreeNode { Value = value };
            }

            // Отримуємо глибину лівого і правого піддерев
            int leftDepth = GetSubtreeDepth(node.Left);
            int rightDepth = GetSubtreeDepth(node.Right);

            // Перевіряємо чи ліве піддерево повністю заповнене до своєї глибини
            bool isLeftComplete = IsSubtreeComplete(node.Left, leftDepth);

            if (leftDepth == 0)
            {
                // Якщо лівого піддерева немає, додаємо зліва
                node.Left = new TreeNode { Value = value };
            }
            else if (rightDepth < leftDepth)
            {
                // Якщо праве піддерево менше за глибиною, додаємо в праве
                node.Right = InsertNode(node.Right, value);
            }
            else if (!isLeftComplete)
            {
                // Якщо ліве піддерево не повне, додаємо в ліве
                node.Left = InsertNode(node.Left, value);
            }
            else if (rightDepth == leftDepth && !IsSubtreeComplete(node.Right, rightDepth))
            {
                // Якщо праве піддерево такої ж глибини, але не повне, додаємо в праве
                node.Right = InsertNode(node.Right, value);
            }
            else
            {
                // В інших випадках додаємо в ліве піддерево
                node.Left = InsertNode(node.Left, value);
            }

            return node;
        }

        private int GetSubtreeDepth(TreeNode? node)
        {
            if (node == null) return 0;
            return 1 + Math.Max(GetSubtreeDepth(node.Left), GetSubtreeDepth(node.Right));
        }

        private bool IsSubtreeComplete(TreeNode? node, int depth)
        {
            if (depth == 0) return true;
            if (node == null) return false;
            if (depth == 1) return true;

            return IsSubtreeComplete(node.Left, depth - 1) && 
                   IsSubtreeComplete(node.Right, depth - 1);
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
            root = null;
            isVisualizationRunning = false;
            visualization?.Clear();
        }

        public override void StartManualSolution()
        {
            visualization?.Update();
        }
    }
} 