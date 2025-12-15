using System;
using System.Collections.Generic;
using System.Text;
namespace TuringMachine
{
    public class Program()
    {
        public static void Main()
        {

            TuringMachine tm = new TuringMachine();

            Console.WriteLine("ВАЖНО! Задача реализована для показателей степеней больше или равных 1.");
            Console.WriteLine("Цель: 1^k 0^m 1^n 0 1^l -> 1^l 0^k 1^3 0 1^n");
            Console.WriteLine("Как видно, в строке, которую нам нужно получить, показатель m не участвует (нет необходимости " +
                "строить последовательность длины m)");

            tm.Comparison(1, 1, 1, 1);

            tm.Comparison(1, 1, 1, 2);
            tm.Comparison(1, 1, 1, 3);

            tm.Comparison(1, 1, 2, 1);
            tm.Comparison(1, 1, 3, 1);
            tm.Comparison(1, 1, 3, 2);
            tm.Comparison(1, 1, 3, 3);

            tm.Comparison(1, 3, 1, 1);
            tm.Comparison(1, 3, 1, 3);
            tm.Comparison(1, 3, 2, 3);
            tm.Comparison(1, 3, 3, 3);

            tm.Comparison(3, 1, 1, 1);
            tm.Comparison(3, 1, 1, 3);
            tm.Comparison(3, 1, 3, 1);
            tm.Comparison(3, 1, 3, 3);

            tm.Comparison(3, 2, 1, 1);

            tm.Comparison(3, 4, 3, 3);

            tm.Comparison(5, 10, 10, 4);

            tm.Comparison(5, 7, 4, 4);
            tm.Comparison(5, 4, 7, 4);

            tm.Comparison(3, 5, 3, 2);
        }
    }
}