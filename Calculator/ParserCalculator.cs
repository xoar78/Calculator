using System;
using System.Linq;

namespace Calculator
{
    public class ParserCalculator
    {
        public static string Calculate(string exp)
        {
            exp = RemoveBrackets(exp);
            int[] arrayIndexOp = FillArrayOfSymbols(exp, new char[] { '-', '+', '*', '/' });
            var arrValue = exp.Split(new char[] { '-', '+', '*', '/' });
            string result = arrValue[0];
            for (int i = 0; i < arrValue.Length; i++)
            {
                int num; double double_num;
                if (Int32.TryParse(arrValue[i], out num) == false && Double.TryParse(arrValue[i], out double_num) == false)
                    return "Error";
            }
            result = SolveExp(arrayIndexOp, arrValue, exp);
            return result;
        }

        private static string RemoveBrackets(string exp)
        {
            int countStartBracket = exp.Split('(').Length - 1;
            int countEndBracket = exp.Split(')').Length - 1;
            if (countStartBracket != countEndBracket) return "Error";
            if ((countStartBracket + countEndBracket) == 0) return exp;
            int[] arrIndexBrackets = FillArrayOfSymbols(exp, new char[] { '(', ')' });
            int queue = 0;
            int startBracket = -1;
            for (int i = 0; i < arrIndexBrackets.Length; i++)
            {
                char bracket = exp[arrIndexBrackets[i]];
                if (queue < 0) return "Error";
                if (bracket == '(' && queue == 0)
                    startBracket = arrIndexBrackets[i];
                if (bracket == ')' && queue == 1)
                {
                    int endBracket = arrIndexBrackets[i];
                    string subexp = exp.Substring(startBracket + 1, endBracket - startBracket - 1);
                    string result = Calculate(subexp);
                    if (result == "Error") return "Error";
                    exp = exp.Remove(startBracket, endBracket - startBracket + 1);
                    exp = exp.Insert(startBracket, result);
                    arrIndexBrackets = FillArrayOfSymbols(exp, new char[] { '(', ')' });
                    i = -1;
                    queue = 0;
                }
                else if (bracket == ')')
                    queue--;
                else if (bracket == '(')
                    queue++;
            }
            if (queue != 0)
                return "Error";
            return exp;
        }

        private static string SolveExp(int[] arrOp, string[] arrValue, string exp)
        {
            bool div = false;
            bool raz = false;
            for (int i = 0; i < arrOp.Length; i++)
            {
                string value = "";
                if (exp[arrOp[i]] == '*')
                    value = Convert.ToString(Double.Parse(arrValue[i]) * Double.Parse(arrValue[i + 1]));
                else if (exp[arrOp[i]] == '/')
                {
                    value = Convert.ToString(Double.Parse(arrValue[i]) / Double.Parse(arrValue[i + 1]));
                    div = true;
                }
                if (value != "")
                {
                    arrValue[i] = value;
                    Array.Clear(arrValue, i + 1, 1);
                    for (int index = i + 1; index + 1 < arrValue.Length; index++)
                        arrValue[index] = arrValue[index + 1];
                    Array.Resize(ref arrValue, arrValue.Length - 1);
                    Array.Clear(arrOp, i, 1);
                    for (int index = i; index + 1 < arrOp.Length; index++)
                        arrOp[index] = arrOp[index + 1];
                    Array.Resize(ref arrOp, arrOp.Length - 1);
                    i--;
                }
            }
            string result = arrValue[0];
            for (int i = 0; i < arrOp.Length; i++)
            {
                if (exp[arrOp[i]] == '+')
                    result = Convert.ToString(Double.Parse(result) + Double.Parse(arrValue[i + 1]));
                if (exp[arrOp[i]] == '-')
                {
                    result = Convert.ToString(Double.Parse(result) - Double.Parse(arrValue[i + 1]));
                    raz = true;
                }    
            }
            if ((div == true || raz == true) && result.IndexOf(',') != -1)
                result = string.Format("{0:N3}", Convert.ToDouble(result));
            return result;
        }

        private static int[] FillArrayOfSymbols(string exp, char[] symbols)
        {
            int[] arrOfIndexSymbols = new int[100];
            int size = 0;
            for (int i = 0; i < symbols.Length; i++)
            {
                int indexSymbol = -1;
                char symbol = symbols[i];
                while (exp.Contains(symbol))
                {
                    indexSymbol = exp.IndexOf(symbol);
                    arrOfIndexSymbols[size++] = indexSymbol;
                    exp = exp.Remove(indexSymbol, 1);
                    exp = exp.Insert(indexSymbol, " ");
                }
            }
            Array.Resize(ref arrOfIndexSymbols, size);
            Array.Sort(arrOfIndexSymbols);
            return arrOfIndexSymbols;
        }

        public static bool Valid(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return true;
            if ("/+*-".Contains(expression[expression.Length - 1]) || Equals(expression[expression.Length - 1], ',') ||
                expression.Length > 1 && "(".Contains(expression[expression.Length - 1]) && "+-/*(".Contains(expression[expression.Length - 2]) == false ||
                expression.Length > 1 && "(".Contains(expression[expression.Length - 1]) && "+-/*(".Contains(expression[expression.Length - 2]) == false ||
                expression.Count(x => x == '(') != expression.Count(x => x == ')') || expression.Contains('(') == false && expression.Contains(')'))
            {
                return false;
            }
            else
                return true;
        }
    }
}
