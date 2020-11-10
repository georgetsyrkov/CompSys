using System;
using University;
using System.Threading;
using System.Collections.Generic;

namespace CompSys
{
    class Program
    {
        static void Main(string[] args)
        {
            string mode = "laba1";

            if (args.Length > 0)
            {
                mode = args[0];
            }

            if (mode == "laba1")
            {
                Console.WriteLine("Hello World");
            }

            else if (mode == "laba2")
            {

                int a = 34;
                float b = 3.14f;
                string c = "Вычислительные системы";
                char d = 'Й';

                double ResultAB = a / b;
                float ResultABasFloat = a / b;
                string ResultCD = c + d;

                Console.WriteLine(ResultAB);
                Console.WriteLine(ResultABasFloat);
                Console.WriteLine(ResultCD);


                // Последовательность выполнения
                int A = 15;
                Console.WriteLine("шаг 1: A=" + A);
                A = A - 3;
                Console.WriteLine("шаг 2: A=" + A);


                // Ветвление
                Console.WriteLine("Перед вами камень, укажите куда идти (налево, направо):");
                ConsoleKeyInfo inputKey = Console.ReadKey();

                if (inputKey.Key == ConsoleKey.LeftArrow)
                {
                    Console.WriteLine("Вы потеряли коня :-(");
                }
                else if (inputKey.Key == ConsoleKey.RightArrow)
                {
                    Console.WriteLine("Вы потеряли жизнь :-(");
                }
                else
                {
                    Console.WriteLine("Вы пошли вперед и убились об камень Х_Х");
                }


                // Цикл
                int count = 5;

                while (count > 0)
                {
                    Console.WriteLine($"Осталось циклов: {count}");
                    count = count - 1;
                }

                ClassExample();
                GC.Collect();
            }
        
            else if (mode == "laba3")
            {
                Console.WriteLine("Введите выражение:");
                string inputData = Console.ReadLine();
                Console.WriteLine($"Вычисляем: {inputData}");

                ReversePolishNotation rpn = new ReversePolishNotation(true);
                decimal result = rpn.GetResult(inputData, new System.Collections.Generic.Dictionary<string, string>());

                string outputData = result.ToString();
                Console.WriteLine($"Результат: {outputData}");
            }

            else if (mode == "laba4")
            {
                DateTime file_start_time = DateTime.Now;
                DateTime file_end_time = file_start_time;

                string[] input_lines = System.IO.File.ReadAllLines("input.txt");

                file_end_time = DateTime.Now;

                TimeSpan file_delta = file_end_time - file_start_time;
                Console.WriteLine($"Время чтения данных: {file_delta}");



                DateTime array_start_time = DateTime.Now;
                DateTime array_end_time = array_start_time;

                int all_threads = 8;
                int all_lines_count = input_lines.Length;
                int small_lines_count = all_lines_count / all_threads;
                
                List<string>[] input_array = new List<string>[all_threads];

                for (int i = 0; i < all_threads; i++)
                {
                    input_array[i] = new List<string>();
                }

                int arrayNum = 0;
                int local_count = 0;

                foreach(string str in input_lines)
                {
                    if (local_count < small_lines_count)
                    {
                        input_array[arrayNum].Add(str);
                        local_count++;
                    }
                    else
                    {
                        local_count = 0;
                        arrayNum = arrayNum + 1;
                    }
                }

                array_end_time = DateTime.Now;
                TimeSpan array_delta = array_end_time - array_start_time;
                Console.WriteLine($"Время реорганизации данных: {array_delta}");


                for (int i = 0; i < all_threads; i++)
                {
                    Thread thread = new Thread(new ParameterizedThreadStart(CountArray));
                    inputData data = new inputData();
                    data.inputLines = input_array[i];
                    data.threadNumber = i;

                    thread.Start(data);
                }
            }
            else
            {
                Console.WriteLine("Не правильно, попробуй еще раз");
            }
            Console.WriteLine("Конец программы");
        }


        static void CountArray(object input_object)// input_lines)
        {
            inputData input_data = (inputData)input_object;

            ReversePolishNotation rpn = new ReversePolishNotation(false);

            DateTime start_time = DateTime.Now;
            DateTime end_time = start_time;

            for (int i = 0; i < input_data.inputLines.Count; i++)
            {
                decimal result = rpn.GetResult(input_data.inputLines[i], new System.Collections.Generic.Dictionary<string, string>());
            }

            end_time = DateTime.Now;

            TimeSpan delta = end_time - start_time;
            Console.WriteLine($"[Поток: {input_data.threadNumber}] Время расчета: {delta}");            
        }

        static void PrintSkillsData(Student s)
        {
            Console.WriteLine($"Навыки у {s.Name}: ");
            foreach (string skill in s.Skills)
            {
                Console.WriteLine(skill);
            }
        }

        static void ClassExample()
        {
            string text = "";

            Student stud = new Student();

            text = stud.GetInfo();
            Console.WriteLine(text);

            PrintSkillsData(stud);

            string[] newSkills = {
                "Разрабатывать ПО",
                "Классифицировать ВС",
                "Переустанавливать Windows"};

            stud.Skills = newSkills;

            PrintSkillsData(stud);

            stud.Name = "Новый студент";

            text = stud.GetInfo();
            Console.WriteLine(text);

            stud = null;
        }
    }

    public class inputData
    {
        public List<string> inputLines = new List<string>();
        public int threadNumber = 0;
    }
}
