using System;
using System.Collections.Generic;

namespace CompSys
{
    public class ReversePolishNotation
    {
        private bool _debug = false;
        private List<string> _opers;

        public ReversePolishNotation(bool debug)
        {
            this._debug = debug;
            this._opers = new List<string>(new string[] { "+", "-", "*", "/", "^", "(", ")", "ATAN", "SQRT", "SIN", "COS", "ASIN", "SIN2" });
        }

        private int GetPriority(string c)
        {
            int ret = 4;

            if (c == "(" || c == ")") { ret = 0; }
            if (c == "+" || c == "-") { ret = 1; }
            if (c == "*" || c == "/") { ret = 2; }
            if (c == "^") { ret = 3; }
            if (c == "LOG" || c == "SQRT" || c == "SIN"  || c == "COS"  || c == "TAN" || c == "SIN2") { ret = 3; }
            if (c == "EXP" || c == "ABS"  || c == "ASIN" || c == "ACOS" || c == "ATAN") { ret = 3; }

            return ret;
        }

        private IEnumerable<string> Split(string input)
        {
            int p = 0;

            while (p < input.Length)
            {
                string s = string.Empty + input[p];

                if (!this._opers.Contains(input[p].ToString()))
                {
                    if (Char.IsDigit(input[p]))
                    {
                        for (int i = p + 1; i < input.Length && (Char.IsDigit(input[i]) || input[i] == ',' || input[i] == '.'); i++)
                        {
                            s = s + input[i];
                        }
                    }
                    else if (Char.IsLetter(input[p]) || input[p] == '_')
                    {
                        for (int i = p + 1; i < input.Length && (Char.IsLetter(input[i]) || Char.IsDigit(input[i]) || input[i] == '_'); i++)
                        {
                            s = s + input[i];
                        }
                    }
                }

                yield return s;

                p = p + s.Length;
            }
        }

        public List<string> ConvertToPostfixNotation(string input)
        {
            List<string> ret = new List<string>();

            Stack<string> stack = new Stack<string>();

            foreach (string c in Split(input))
            {
                if (_opers.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                ret.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                        {
                            stack.Push(c);
                        }
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                            {
                                ret.Add(stack.Pop());
                            }

                            stack.Push(c);
                        }
                    }
                    else
                    {
                        stack.Push(c);
                    }
                }
                else
                {
                    ret.Add(c);
                }
            }
            if (stack.Count > 0)
            {
                foreach (string c in stack)
                {
                    ret.Add(c);
                }
            }

            return ret;
        }

        public decimal GetResult(string input, Dictionary<string, string> pars)
        {
            System.Globalization.CultureInfo ci =
                (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
            input = input.Replace(".", ci.NumberFormat.CurrencyDecimalSeparator);

            bool hasOpers = false;
            foreach (var op in this._opers)
            {
                if (input.Contains(op))
                {
                    hasOpers = true;
                }
            }

            if (!hasOpers)
            {
                decimal ret = 0;
                if (pars.ContainsKey(input.Trim())) { input = pars[input.Trim()]; }
                {
                    ret = Convert.ToDecimal(input.Trim());
                }
                return ret;
            }

            List<string> converted_items = ConvertToPostfixNotation(input);

            string converted_items_string = string.Join("|", converted_items);
            if (this._debug)
            {
                SayInfo("DEBUGinfo: input=" + input + ", resut=" + converted_items_string);
            }
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(converted_items);

            string str = queue.Dequeue();

            while (queue.Count >= 0)
            {
                if (!_opers.Contains(str))
                {
                    stack.Push(str);
                    str = queue.Dequeue();
                }
                else
                {
                    decimal summ = 0;

                    try
                    {
                        if (str == "+" ||
                            str == "-" ||
                            str == "*" ||
                            str == "/" ||
                            str == "^")
                        {
                            string str_a = stack.Pop();
                            string str_b = stack.Pop();

                            if (pars.ContainsKey(str_a)) { str_a = pars[str_a]; }
                            if (pars.ContainsKey(str_b)) { str_b = pars[str_b]; }

                            decimal a = Convert.ToDecimal(str_a);
                            decimal b = Convert.ToDecimal(str_b);

                            if (str == "+")
                            {
                                summ = a + b;
                            }

                            if (str == "-")
                            {
                                summ = b - a;
                            }

                            if (str == "*")
                            {
                                summ = b * a;
                            }

                            if (str == "/")
                            {
                                summ = b / a;
                            }

                            if (str == "^")
                            {
                                summ = Convert.ToDecimal(Math.Pow(Convert.ToDouble(b), Convert.ToDouble(a)));
                            }
                        }
                        else
                        {
                            string str_a = stack.Pop();
                            if (pars.ContainsKey(str_a)) { str_a = pars[str_a]; }
                            decimal a = Convert.ToDecimal(str_a);

                            if (str == "ATAN")
                            {
                                summ = Convert.ToDecimal(Math.Atan(Convert.ToDouble(a)));
                            }
                            if (str == "TAN")
                            {
                                summ = Convert.ToDecimal(Math.Tan(Convert.ToDouble(a)));
                            }
                            if (str == "ABS")
                            {
                                summ = Convert.ToDecimal(Math.Abs(Convert.ToDouble(a)));
                            }
                            if (str == "LOG")
                            {
                                summ = Convert.ToDecimal(Math.Log(Convert.ToDouble(a)));
                            }
                            if (str == "EXP")
                            {
                                summ = Convert.ToDecimal(Math.Exp(Convert.ToDouble(a)));
                            }
                            if (str == "SQRT")
                            {
                                summ = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(a)));
                            }
                            if (str == "ASIN")
                            {
                                summ = Convert.ToDecimal(Math.Asin(Convert.ToDouble(a)));
                            }
                            if (str == "SIN")
                            {
                                summ = Convert.ToDecimal(Math.Sin(Convert.ToDouble(a)));
                            }
                            if (str == "SIN2")
                            {
                                summ = Convert.ToDecimal(Math.Sin((Convert.ToDouble(a)/ 180D) * Math.PI));
                            }
                            if (str == "ACOS")
                            {
                                summ = Convert.ToDecimal(Math.Acos(Convert.ToDouble(a)));
                            }
                            if(str == "COS")
                            {
                                summ = Convert.ToDecimal(Math.Cos(Convert.ToDouble(a)));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SayError("" + input + ": " + ex.Message);
                    }

                    stack.Push(summ.ToString());

                    if (queue.Count > 0)
                    {
                        str = queue.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return Convert.ToDecimal(stack.Pop());
        }

        private void SayInfo(string msg)
        {
            DateTime n = DateTime.Now;
            string str =     //   TOOLS
                string.Format("[RPN-PAR-->{0:00}:{1:00}:{2:00}.{3:000}] {4}: {5}",
                                n.Hour,
                                n.Minute,
                                n.Second,
                                n.Millisecond,
                                "    Информация",
                                msg);

            //_log.Info(str);
            Console.WriteLine(str);
        }

        private void SayError(string msg)
        {
            DateTime n = DateTime.Now;
            string str =
                string.Format("[RPN-PAR-->{0:00}:{1:00}:{2:00}.{3:000}] {4}: {5}",
                                n.Hour,
                                n.Minute,
                                n.Second,
                                n.Millisecond,
                                "        Ошибка",
                                msg);

            //_log.Info(str);
            Console.WriteLine(str);
        }
    }
}
