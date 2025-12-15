using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringMachine
{
    public class TuringMachine
    {
        private int head = 0;
        private string state = "q0";

        // Переходы: (state, read) → (newState, write, direction: 'L','R','N')
        private Dictionary<(string, char), (string, char, char)> delta = new();

        public TuringMachine()
        {

            void Add(string q, char c, string nq, char w, char m) => delta[(q, c)] = (nq, w, m);

            // === ФАЗА 1 ===
            // === Делаем смещение l единиц справа в самое начало так, чтобы между ними и k единицами был разделительный 0 ===

            // Идея: пользуемся тем, что у нас перед l единицами стоит 0, это очень использовать для того, чтобы перетащить
            // все единицы из конца в начало. Ведь если мы из этого самого нуля сдвинемся вправо и попадём в 0, значит, все
            // единицы из правой части уже передвинуты в самый левый край.

            Add("q0", '1', "q0", '1', 'R');
            Add("q0", '0', "q1", '0', 'R');
            Add("q1", '0', "q1", '0', 'R');
            Add("q1", '1', "q2", '1', 'R');
            Add("q2", '1', "q2", '1', 'R');
            Add("q2", '0', "q3", '0', 'R');
            Add("q3", '1', "q4", '1', 'R');
            Add("q3", '0', "q12", '0', 'L');
            Add("q4", '1', "q4", '1', 'R');
            Add("q4", '0', "q5", '0', 'L');
            Add("q5", '1', "q6", '0', 'L');
            Add("q5", '0', "q6", '0', 'L');
            Add("q6", '0', "q7", '0', 'L');
            Add("q6", '1', "q6", '1', 'L');
            Add("q7", '1', "q7", '1', 'L');
            Add("q7", '0', "q8", '0', 'L');
            Add("q8", '0', "q8", '0', 'L');
            Add("q8", '1', "q9", '1', 'L');
            Add("q9", '0', "q10", '0', 'L');
            Add("q9", '1', "q9", '1', 'L');
            Add("q10", '0', "q11", '1', 'R');
            Add("q10", '1', "q10", '1', 'L');
            Add("q11", '1', "q11", '1', 'R');
            Add("q11", '0', "q0", '0', 'R');

            // === ФАЗА 2 ===
            // === Запишем k нулей после l единиц ===

            // Идея: после l единиц мы получим 0 (который использовали ранее как разделительный) и сразу после него k других
            // единиц. Надо просто занулить k-1 единиц из имеющихся k. Для этого пойдём справа налево по строке из k единиц,
            // первую единицу мы пропустим, а остальные зануляем до тех пор, пока не попадётся тот самый разделительный 0.

            Add("q12", '0', "q13", '0', 'L');
            Add("q13", '1', "q13", '1', 'L');
            Add("q13", '0', "q14", '0', 'L');
            Add("q14", '0', "q14", '0', 'L');
            Add("q14", '1', "q15", '1', 'L');
            Add("q15", '1', "q15", '0', 'L');
            Add("q15", '0', "q16", '0', 'R');
            Add("q16", '0', "q16", '0', 'R');
            Add("q16", '1', "q17", '1', 'R');

            // === ФАЗА 3 ===
            // === Создадим 3 единицы после k нулей ===

            // Идея: пользуясь разными состояниями, мы будем добавлять по единице справа от k нулей, а всё остальное просто
            // передвинем вправо. Одна единица после k нулей уже будет гарантированно, поскольку после k нулей идёт 
            // последовательность из n единиц (одна единица будет точно). Значит, добавим справа от неё сначала одну, потом другую
            // единицу (каждое добавление единицы сопровождается смещением оставшейся правой части в правую сторону).

            Add("q16", '1', "q17", '1', 'R');
            Add("q17", '0', "q18", '1', 'R');
            Add("q18", '0', "q18", '0', 'R');
            Add("q18", '1', "q19", '0', 'R');
            Add("q19", '1', "q19", '1', 'R');
            Add("q19", '0', "q20", '1', 'L'); // Создали 11

            Add("q20", '1', "q20", '1', 'L');
            Add("q20", '0', "q21", '0', 'L');
            Add("q21", '0', "q21", '0', 'L');
            Add("q21", '1', "q22", '1', 'R');
            Add("q22", '0', "q23", '1', 'R'); // Создали 111

            // === ФАЗА 4 ===
            // === Сдвиг всего после 111 вправо ===

            // Идея: на этом этапе важно учесть случай, когда m = 1. В этом случае при сдвиге вправо n единиц уже окажутся
            // на своём месте, поэтому можно будет завершить работу.

            Add("q23", '1', "q24", '0', 'R'); //    \
            Add("q24", '1', "q24", '1', 'R'); //     | Если m = 1
            Add("q24", '0', "qf", '1', 'R');  //    /

            Add("q23", '0', "q25", '0', 'R');
            Add("q25", '0', "q25", '0', 'R');
            Add("q25", '1', "q26", '0', 'R');
            Add("q26", '1', "q26", '1', 'R');
            Add("q26", '0', "q27", '1', 'R');

            // === ФАЗА 5 ===
            // === Перемещаем n единиц в нужное место ===

            // Идея: справа мы имеем n единиц. Берём самую правую единицу и перетаскиваем её в начало последовательности из
            // этих самых n единиц. Эту процедуру мы повторяем до тех пор, пока между тремя единицами левее и n единицами
            // справа не окажется ровно один 0.

            Add("q27", '0', "q28", '0', 'L');
            Add("q28", '1', "q29", '0', 'L');
            Add("q29", '1', "q29", '1', 'L');
            Add("q29", '0', "q30", '1', 'L');
            Add("q30", '0', "q31", '0', 'L');
            Add("q31", '1', "qf", '1', 'R');
            Add("q31", '0', "q32", '0', 'R');
            Add("q32", '0', "q33", '0', 'R');
            Add("q33", '1', "q33", '1', 'R');
            Add("q33", '0', "q28", '0', 'L');

        }

        public StringBuilder Run(int k, int m, int n, int l)
        {
            head = 0;
            state = "q0";


            StringBuilder tape = GenerateTape(k, m, n, l);

            //Console.WriteLine(tape);

            int startOfData = IndexOf(tape, '1');

            head = IndexOf(tape, '1');

            while(state != "qf")
            {
                char current = tape[head];

                if(delta.TryGetValue((state, current), out var value))
                {

                    string newState = value.Item1;
                    char write = value.Item2;
                    char move = value.Item3;

                    tape[head] = write;

                    state = newState;

                    if (move == 'R') head++;
                    else if (move == 'L') head--;
                }

            }

            //Console.WriteLine(tape);
            //Console.WriteLine($"head: {head} state: {state}");


            return tape;
        }

        private StringBuilder GenerateInputString(int k, int m, int n, int l)
        {
            if (k * m * n * l <= 0)
            {
                throw new Exception("Numbers must be non-negative integers");
            }

            StringBuilder tapeString = new StringBuilder();
            tapeString.Append('1', k);
            tapeString.Append('0', m);
            tapeString.Append('1', n);
            tapeString.Append('0');
            tapeString.Append('1', l);

            return tapeString;
        }

        public StringBuilder GenerateTape(int k, int m, int n, int l)
        {
            StringBuilder tape = new StringBuilder();
            int borderBlankSigns = k + m + n + l;

            tape.Append('0', borderBlankSigns);

            tape.Append(GenerateInputString(k, m, n, l));

            tape.Append('0', borderBlankSigns);

            return tape;
        }

        private int IndexOf(StringBuilder sb, char value)
        {
            for (int i = 0; i < sb.Length; i++)
                if (sb[i] == value)
                    return i;
            return -1;
        }

        private StringBuilder TapeData(StringBuilder sb, int length)
        {
            int startPosition = IndexOf(sb, '1');

            StringBuilder result = new StringBuilder();

            for(int i = startPosition; i < startPosition + length; i++)
            {
                result.Append(sb[i]);
            }

            return result;
        }

        public void Comparison(int k, int m, int n, int l)
        {
            StringBuilder inputString = new StringBuilder();
            StringBuilder outputTape = new StringBuilder();
            StringBuilder outputString = new StringBuilder();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("=============================================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Исходная строка: ");
            Console.ResetColor();
            Console.WriteLine($"k:{k} m:{m} n:{n} l:{l}");
            inputString = GenerateInputString(k, m, n, l);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(inputString);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Полученная строка: ");
            Console.ResetColor();
            Console.WriteLine($"l:{l} k:{k} n:{n}");
            outputTape = Run(k, m, n, l);
            outputString = TapeData(outputTape, l + k + n + 4);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(outputString);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("=============================================================");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
