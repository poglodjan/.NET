using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{
    public class Enemy
    {
        public Point Position {  get; private set; }
        private readonly List<Point> PatrolPath;    // Lista punktów patrolowych
        private int CurrentIndex;

        public Enemy(IEnumerable<Point> points)
        {
            // Inicjalizacja ścieżki patrolowej
            PatrolPath = new List<Point>(points);
            if (PatrolPath.Count == 0)
                throw new ArgumentException("Patrol path cannot be empty.");

            // Ustawienie początkowej pozycji na pierwszy punkt ścieżki
            Position = PatrolPath[0];
            CurrentIndex = 0;
        }


        public void MoveNext()
        {
            // Przesunięcie indeksu do następnego punktu
            CurrentIndex = (CurrentIndex + 1) % PatrolPath.Count;

            // Aktualizacja pozycji wroga
            Position = PatrolPath[CurrentIndex];
        }
        //end task 4
    }
}
