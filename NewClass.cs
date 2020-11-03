using System;

namespace University
{
    public class Student
    {
        public string Name;
        private int Age;

        private string[] _skills;
        public string[] Skills 
        {
            get  { return _skills; }

            set
            {
                _skills = new string[value.Length];

                for (int i = 0; i < _skills.Length; i++)
                {
                    _skills[i] = "Приобретенное извне " + value[i];
                }
            }
        }

        public Student()
        {
            this.Name = "Вася Пупкин";
            this.Age = 21;
            this._skills = new string[5];

            this._skills[0] = "Есть";
            this._skills[1] = "Пить";
            this._skills[2] = "Общаться";
            this._skills[3] = "Списывать";
            this._skills[4] = "Прогуливать";
        }

        ~Student()
        {
            Console.WriteLine("Вызывался деструктор для " + Name);
        }


        public string GetInfo()
        {
            string message = "Class name: " + this.Name + ", age: " + this.Age + ".";
            return message;
        }
    }
}