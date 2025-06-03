using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApp1.ViewModels;
using System.Collections.Generic;

namespace WpfApp1.Views.Visualizations
{
    public class TreeVisualization : IVisualization
    {
        public Canvas InputCanvas { get; set; }
        public Canvas OutputCanvas { get; set; }
        private readonly TreeNode? root;
        private const int NODE_SIZE = 40;
        private const int VERTICAL_SPACING = 60;
        private const int MIN_HORIZONTAL_SPACING = 50;
        private const double OFFSET_Y = 50;
        private const double CANVAS_MARGIN = 20;

        private class NodeInfo
        {
            public double X { get; set; }
            public double Y { get; set; }
            public int Level { get; set; }
            public double LeftBound { get; set; }
            public double RightBound { get; set; }
        }

        public TreeVisualization(TreeNode? root, Canvas inputCanvas, Canvas outputCanvas)
        {
            this.root = root;
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
            if (root != null)
            {
                DrawTree(InputCanvas, root);
            }
        }

        public void Update()
        {
            if (root != null)
            {
                DrawTree(OutputCanvas, root);
            }
        }

        private void DrawTree(Canvas canvas, TreeNode root)
        {
            canvas.Children.Clear();
            var nodePositions = new Dictionary<TreeNode, NodeInfo>();
            var nodes = new List<TreeNode>();

            // Перший прохід: визначаємо кількість вузлів на кожному рівні та порядок вузлів
            var levelCounts = new Dictionary<int, int>();
            CollectNodesInOrder(root, 0, levelCounts, nodes);

            // Другий прохід: розраховуємо позиції
            var rootInfo = CalculateNodePositions(root, 0, CANVAS_MARGIN, nodePositions, levelCounts);

            // Знаходимо межі дерева
            double minX = nodePositions.Values.Min(p => p.X);
            double maxX = nodePositions.Values.Max(p => p.X);
            double treeWidth = maxX - minX + NODE_SIZE;
            double maxY = nodePositions.Values.Max(p => p.Y) + NODE_SIZE;

            // Розраховуємо масштаб та зміщення для центрування
            double availableWidth = canvas.ActualWidth - (2 * CANVAS_MARGIN);
            double availableHeight = canvas.ActualHeight - (2 * CANVAS_MARGIN);
            
            double scaleX = availableWidth / treeWidth;
            double scaleY = availableHeight / maxY;
            double scale = Math.Min(scaleX, scaleY);

            // Розраховуємо зміщення для центрування
            double offsetX = (canvas.ActualWidth - (treeWidth * scale)) / 2;

            // Спочатку малюємо всі лінії у правильному порядку
            foreach (var node in nodes)
            {
                var info = nodePositions[node];
                double x = (info.X - minX) * scale + offsetX;
                double y = info.Y + OFFSET_Y;

                if (node.Left != null && nodePositions.ContainsKey(node.Left))
                {
                    var leftInfo = nodePositions[node.Left];
                    DrawConnectionLine(canvas, 
                        x, y,
                        (leftInfo.X - minX) * scale + offsetX,
                        leftInfo.Y + OFFSET_Y);
                }

                if (node.Right != null && nodePositions.ContainsKey(node.Right))
                {
                    var rightInfo = nodePositions[node.Right];
                    DrawConnectionLine(canvas,
                        x, y,
                        (rightInfo.X - minX) * scale + offsetX,
                        rightInfo.Y + OFFSET_Y);
                }
            }

            // Потім малюємо всі вузли у тому ж порядку
            foreach (var node in nodes)
            {
                var info = nodePositions[node];
                double x = (info.X - minX) * scale + offsetX;
                double y = info.Y + OFFSET_Y;
                DrawSingleNode(canvas, node, x, y);
            }
        }

        private void CollectNodesInOrder(TreeNode? node, int level, Dictionary<int, int> levelCounts, List<TreeNode> nodes)
        {
            if (node == null) return;

            // Спочатку обробляємо ліве піддерево
            CollectNodesInOrder(node.Left, level + 1, levelCounts, nodes);

            // Додаємо поточний вузол
            if (!levelCounts.ContainsKey(level))
                levelCounts[level] = 0;
            levelCounts[level]++;
            nodes.Add(node);

            // Потім обробляємо праве піддерево
            CollectNodesInOrder(node.Right, level + 1, levelCounts, nodes);
        }

        private NodeInfo CalculateNodePositions(TreeNode? node, int level, double offset, 
            Dictionary<TreeNode, NodeInfo> positions, Dictionary<int, int> levelCounts)
        {
            if (node == null) return null;

            var nodeInfo = new NodeInfo { Level = level, Y = level * VERTICAL_SPACING };
            double spacing = Math.Max(MIN_HORIZONTAL_SPACING, NODE_SIZE * 1.5);

            // Спочатку обчислюємо позиції для лівого піддерева
            var leftInfo = CalculateNodePositions(node.Left, level + 1, offset, positions, levelCounts);
            
            // Потім для правого піддерева
            var rightInfo = CalculateNodePositions(node.Right, level + 1, 
                leftInfo?.RightBound ?? offset + spacing, positions, levelCounts);

            // Визначаємо межі поточного вузла
            if (leftInfo == null && rightInfo == null)
            {
                // Лист дерева
                nodeInfo.LeftBound = offset;
                nodeInfo.RightBound = offset + spacing;
                nodeInfo.X = offset + spacing / 2;
            }
            else if (leftInfo != null && rightInfo != null)
            {
                // Вузол з двома дітьми
                nodeInfo.LeftBound = leftInfo.LeftBound;
                nodeInfo.RightBound = rightInfo.RightBound;
                nodeInfo.X = (leftInfo.X + rightInfo.X) / 2;
            }
            else if (leftInfo != null)
            {
                // Тільки ліва дитина
                nodeInfo.LeftBound = leftInfo.LeftBound;
                nodeInfo.RightBound = leftInfo.RightBound + spacing;
                nodeInfo.X = leftInfo.X + spacing / 2;
            }
            else
            {
                // Тільки права дитина
                nodeInfo.LeftBound = rightInfo.LeftBound - spacing;
                nodeInfo.RightBound = rightInfo.RightBound;
                nodeInfo.X = rightInfo.X - spacing / 2;
            }

            positions[node] = nodeInfo;
            return nodeInfo;
        }

        private void DrawConnectionLine(Canvas canvas, double x1, double y1, double x2, double y2)
        {
            // Розрахунок точок перетину лінії з колами
            double angle1 = Math.Atan2(y2 - y1, x2 - x1);
            double angle2 = Math.Atan2(y1 - y2, x1 - x2);

            // Радіус кола
            double radius = NODE_SIZE / 2;

            // Знаходимо точки на межі кіл
            double startX = x1 + radius * Math.Cos(angle1);
            double startY = y1 + radius * Math.Sin(angle1);
            double endX = x2 + radius * Math.Cos(angle2);
            double endY = y2 + radius * Math.Sin(angle2);

            Line line = new Line
            {
                X1 = startX,
                Y1 = startY,
                X2 = endX,
                Y2 = endY,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            // Встановлюємо Z-Index для лінії нижче ніж у вузлів
            Panel.SetZIndex(line, 0);
            canvas.Children.Add(line);
        }

        private void DrawSingleNode(Canvas canvas, TreeNode node, double x, double y)
        {
            Ellipse circle = new Ellipse
            {
                Width = NODE_SIZE,
                Height = NODE_SIZE,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            TextBlock text = new TextBlock
            {
                Text = node.Value.ToString(),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = NODE_SIZE,
                Height = NODE_SIZE
            };

            Canvas.SetLeft(circle, x - NODE_SIZE / 2);
            Canvas.SetTop(circle, y - NODE_SIZE / 2);
            Canvas.SetLeft(text, x - NODE_SIZE / 2);
            Canvas.SetTop(text, y - NODE_SIZE / 2);

            // Встановлюємо Z-Index для вузлів вище ніж у ліній
            Panel.SetZIndex(circle, 1);
            Panel.SetZIndex(text, 2);

            canvas.Children.Add(circle);
            canvas.Children.Add(text);
        }

        private int GetTreeDepth(TreeNode? node)
        {
            if (node == null) return 0;
            int leftDepth = GetTreeDepth(node.Left);
            int rightDepth = GetTreeDepth(node.Right);
            return Math.Max(leftDepth, rightDepth) + 1;
        }
    }
}
