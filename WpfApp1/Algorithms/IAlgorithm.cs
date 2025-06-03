using System.Collections.Generic;

namespace WpfApp1.Algorithms
{
    public interface IAlgorithm
    {
        // Ініціалізація алгоритму з вхідними даними
        void Initialize(object input);
        
        // Отримати всі кроки виконання алгоритму
        IEnumerable<IAlgorithmStep> GetSteps();
        
        // Виконати один крок алгоритму
        IAlgorithmStep? NextStep();

        // Повернутися на один крок алгоритму

        IAlgorithmStep? PreviusStep();

        bool IsComplete();

        bool IsStarted();


        // Скинути алгоритм до початкового стану
        void Reset();
    }
} 