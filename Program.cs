using System;
using University;

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

            else
            {
                Console.WriteLine("Не правильно, попробуй еще раз");
            }
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
}
