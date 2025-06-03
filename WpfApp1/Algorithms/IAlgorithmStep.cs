using System.Collections.Generic;
using WpfApp1.ViewModels;

namespace WpfApp1.Algorithms
{
    public interface IAlgorithmStep
    {
        string Description { get; }  // Опис кроку для відображення користувачу
        StepType Type { get; }       // Тип кроку (порівняння, обмін, тощо)
        IEnumerable<VisualizationElement> Elements { get; } // Елементи для візуалізації
        object GetData();            // Дані, які потрібно візуалізувати на цьому кроці
    }

    public enum StepType
    {
        Greater,       // Перший елемент більший за другий
        Less,         // Перший елемент менший або рівний другому
        Swap,          // Обмін елементів
        Selection,     // Вибір елемента
        Calculation,   // Розрахунок
        SubProblem,    // Розділення на підзадачі
        Merge,         // Злиття результатів
        Path,          // Шлях у графі/дереві
        Final,         // Фінальний результат
        Comparison     // Порівняння елементів
    }
} 