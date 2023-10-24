using Kalkulator_ASP.Net.Models;
using System;
using System.Drawing;
using System.Text;
using Kalkulator_ASP.Net.Libraries.DatabaseLibrary;

namespace Kalkulator_ASP.Net.Libraries.CalculatorLib
{
    public static class CalculatorInput
    {
        public static void registerInput(string btn, HomeModel calc)
        {
            string Racun2 = calc.Racun1;
            string Racun1 = calc.Racun2;
            string Racun3 = calc.Racun3;
            string scientificMode = calc.ScientificMode;
            double mem = calc.Mem;
            string isScientific = calc.IsScientific;
            string isHistory = calc.isHistory;

            //za izbjegavanje errora i rusenja aplikacije
            if (Racun1 == null)
            {
                Racun1 = "";
            }
            if (Racun2 == null)
            {
                Racun2 = "";
            }

            string zadnjigumb = calc.Zadnjigumb;

            //numbers
            if (btn == "btn_0" || btn == "btn_1" || btn == "btn_2" || btn == "btn_3" || btn == "btn_4" || btn == "btn_5" || btn == "btn_6" || btn == "btn_7" || btn == "btn_8" || btn == "btn_9")
            {
                btn = btn.Substring(4);
                if (btn != "0")
                {
                    if (!isLastClosedParenthesis(Racun2))
                    {
                        if (zadnjigumb == "=" || zadnjigumb == "scientificBtn")
                        {
                            Racun2 = "";
                            Racun1 = "";
                            Racun3 = "";
                        }
                        Racun1 += btn;
                        Racun2 += btn;
                        zadnjigumb = btn;
                    }
                }

                else if (btn == "0")
                {
                    if (Racun1 != "")
                    {
                        if (zadnjigumb != "=")
                        {
                            if (zadnjigumb == "=" || zadnjigumb == "scientificBtn")
                            {
                                Racun2 = "";
                                Racun1 = "";
                                Racun3 = "";
                            }
                            Racun1 += 0;
                            Racun2 += 0;
                            zadnjigumb = "0";
                        }
                    }
                }
            }

            //operators
            else if (btn == "btn_plus" || btn == "btn_minus" || btn == "btn_multiply" || btn == "btn_divide")
            {
                switch (btn)
                {
                    case "btn_plus":
                        btn = "+";
                        break;
                    case "btn_minus":
                        btn = "-";
                        break;
                    case "btn_multiply":
                        btn = "*";
                        break;
                    case "btn_divide":
                        btn = "/";
                        break;
                }

                if (Racun2.Contains('='))
                {
                    Racun2 = Racun1;
                    Racun2 += $" {btn} ";
                    Racun3 = "";
                    Racun1 = "";
                    zadnjigumb = btn;
                }

                else if (Racun2 != "" && zadnjigumb != "." && zadnjigumb != "(" && !canSwitchOperator(Racun2))
                {
                    Racun2 += $" {btn} ";
                    Racun1 = "";
                    zadnjigumb = btn;
                }

                else if (Racun2 != "" && canSwitchOperator(Racun2))
                {
                    Racun2 = Racun2.Remove(Racun2.Length - 3);
                    Racun2 += $" {btn} ";
                    zadnjigumb = btn;
                }
            }
            else if (btn == "btn_equals")
            {
                try
                {
                    Racun1 = Calculator.CalculateExpression(Racun2).ToString();
                    Racun2 += " = ";
                    Database.AddEntry(Racun2, DateTime.Now);
                    zadnjigumb = "=";
                }
                catch
                {

                }
            }

            //delete
            else if (btn == "btn_clear")
            {
                Racun1 = "";
                Racun2 = "";
                Racun3 = "";
            }
            else if (btn == "btn_ce")
            {
                try
                {
                    if (Racun2.Contains("="))
                    {
                        Racun2 = "";
                    }

                    else if (canSwitchOperator(Racun2))
                    {
                        Racun2 = Racun2.Remove(Racun2.Length - 3);
                    }

                    else if (Racun2.Length >= 2 && (Racun2.Substring(Racun2.Length - 2).Contains("( ") || Racun2.Substring(Racun2.Length - 2).Contains(" )")))
                    {
                        Racun2 = Racun2.Remove(Racun2.Length - 2);
                    }

                    else if (isLastNumber(Racun2))
                    {
                        while (Racun2 != "" && (isLastNumber(Racun2) || Racun2[Racun2.Length - 1] == '.' || Racun2[Racun2.Length - 1] == '-'))
                        {
                            Racun2 = Racun2.Remove(Racun2.Length - 1);
                        }
                    }
                    zadnjigumb = "ce";
                    Racun1 = "";
                    Racun3 = "";

                }
                catch
                {

                }
            }
            else if (btn == "btn_delete")
            {
                if (Racun2.Contains("="))
                {
                    Racun2 = "";
                }

                else if (canSwitchOperator(Racun2))
                {
                    Racun2 = Racun2.Remove(Racun2.Length - 3);
                }

                else if (Racun2.Length >= 2 && (Racun2.Substring(Racun2.Length - 2).Contains("( ") || Racun2.Substring(Racun2.Length - 2).Contains(" )")))
                {
                    Racun2 = Racun2.Remove(Racun2.Length - 2);
                }

                else if (Racun2.Length >= 1)
                {
                    Racun2 = Racun2.Remove(Racun2.Length - 1);
                }

                if (Racun1.Length >= 1)
                {
                    Racun1 = Racun1.Remove(Racun1.Length - 1);
                }
                Racun3 = "";
                zadnjigumb = "del";
            }

            //tocka
            else if (btn == "btn_dot")
            {
                if (!isLastOperator(zadnjigumb) && zadnjigumb != "." && !Racun1.Contains(".") && Racun1 != "")
                {
                    Racun1 += ".";
                    Racun2 += ".";
                    zadnjigumb = ".";
                }
                else if (Racun1 == "")
                {
                    Racun2 += "0.";
                    Racun1 += "0.";
                    zadnjigumb = ".";
                }
            }

            //changePrefix
            else if (btn == "btn_changeprefix")
            {
                try
                {
                    if (Racun1 != "" && isLastNumber(Racun2))
                    {
                        string temp = Racun1;
                        int index = Racun2.LastIndexOf(temp);
                        if (Racun1[0] != '-')
                        {
                            Racun2 = InsertCharacter(Racun2, '-', index);
                            Racun1 = InsertCharacter(Racun1, '-', 0);
                        }
                        else
                        {
                            Racun2 = RemoveCharacter(Racun2, index);
                            Racun1 = RemoveCharacter(Racun1, 0);
                        }
                    }
                }
                catch
                {

                }
            }

            //memory
            else if (btn == "btn_memclear")
            {
                mem = 0;
            }
            else if (btn == "btn_memrecall")
            {
                Racun1 = "";
                Racun1 += mem;
                Racun2 += mem;
            }
            else if (btn == "memplus_btn")
            {
                if (Racun1 != "")
                {
                    mem += Convert.ToDouble(Racun1);
                }
            }
            else if (btn == "memreduce_btn")
            {
                if (Racun1 != "")
                {
                    mem -= Convert.ToDouble(Racun1);
                }
            }


            else if (btn == "btn_modeChange")
            {
                if (isScientific == "no")
                {
                    isScientific = "yes";
                }
                else if (isScientific == "yes")
                {
                    isScientific = "no";
                }
            }




            //SCIENTIFIC
            else if (btn == "btn_rad")
            {
                scientificMode = "rad";
                zadnjigumb = "scientificBtn";
            }

            else if (btn == "btn_deg")
            {
                scientificMode = "deg";
                zadnjigumb = "scientificBtn";
            }

            else if (btn == "btn_sin" || btn == "btn_cos" || btn == "btn_tan" || btn == "btn_ctg")
            {
                double tempnum = 0;
                string op = btn.Substring(4);
                if (zadnjigumb != "scientificBtn" && !isLastOperator(zadnjigumb) && zadnjigumb != "." && Racun1 != "")
                {
                    if (scientificMode == "rad")
                    {
                        tempnum = float.Parse(Racun1);
                        Racun3 = $"{op}(" + tempnum + "rad)";
                        switch (op)
                        {
                            case "sin":
                                Racun1 = Math.Sin(tempnum).ToString();
                                break;
                            case "cos":
                                Racun1 = Math.Cos(tempnum).ToString();
                                break;
                            case "tan":
                                Racun1 = Math.Tan(tempnum).ToString();
                                break;
                            case "ctg":
                                Racun1 = (1 / Math.Tan(tempnum)).ToString();
                                break;
                        }
                        zadnjigumb = "scientificBtn";
                    }
                    else if (scientificMode == "deg")
                    {
                        tempnum = float.Parse(Racun1);
                        Racun3 = $"{op}(" + tempnum + "deg)";
                        switch (op)
                        {
                            case "sin":
                                Racun1 = Math.Sin(tempnum * (Math.PI / 180)).ToString();
                                break;
                            case "cos":
                                Racun1 = Math.Cos(tempnum * (Math.PI / 180)).ToString();
                                break;
                            case "tan":
                                Racun1 = Math.Tan(tempnum * (Math.PI / 180)).ToString();
                                break;
                            case "ctg":
                                Racun1 = (1 / Math.Tan(tempnum * (Math.PI / 180))).ToString();
                                break;
                        }
                        zadnjigumb = "scientificBtn";
                    }
                    if (!containsOperator(Racun2))
                    {
                        Racun2 = Racun2.Replace(tempnum.ToString(), Racun1);
                    }
                    else
                    {
                        Racun2 = Racun2.Substring(0, operatorIndex(Racun2) + 2);
                        Racun2 = Racun2 + Racun1;
                    }
                }
            }

            else if (btn == "btn_sin1" || btn == "btn_cos1" || btn == "btn_tan1" || btn == "btn_ctg1")
            {
                double tempnum = 0;
                string op = "";

                //op = (btn == "btn_sin1") ? "Asin" : btn;
                //op = (btn == "btn_cos1") ? "Acos" : btn;
                //op = (btn == "btn_tan1") ? "Actg" : btn;
                //op = (btn == "btn_ctg1") ? "Actg" : btn;

                if (btn == "btn_sin1")
                {
                    op = "Asin";
                }
                else if (btn == "btn_cos1")
                {
                    op = "Acos";
                }
                else if (btn == "btn_tan1")
                {
                    op = "Atan";
                }
                else if (btn == "btn_crg1")
                {
                    op = "Actg";
                }
                if (zadnjigumb != "scientificBtn" && !isLastOperator(zadnjigumb) && zadnjigumb != "." && Racun1 != "")
                {
                    if (calc.ScientificMode == "rad")
                    {
                        tempnum = float.Parse(Racun1);
                        Racun3 = $"{op}(" + tempnum + "rad)";
                        switch (op)
                        {
                            case "Asin":
                                Racun1 = Math.Asin(tempnum).ToString();
                                break;
                            case "Acos":
                                Racun1 = Math.Acos(tempnum).ToString();
                                break;
                            case "Atan":
                                Racun1 = Math.Atan(tempnum).ToString();
                                break;
                            case "Actg":
                                Racun1 = Math.Atan2(1, tempnum).ToString();
                                break;
                        }
                        zadnjigumb = "scientificBtn";
                    }
                    else if (calc.ScientificMode == "deg")
                    {
                        tempnum = float.Parse(Racun1);
                        Racun3 = $"{op}(" + tempnum + "deg)";
                        switch (op)
                        {
                            case "Asin":
                                Racun1 = (Math.Asin(tempnum) * (180 / Math.PI)).ToString();
                                break;
                            case "Acos":
                                Racun1 = (Math.Acos(tempnum) * (180 / Math.PI)).ToString();
                                break;
                            case "Atan":
                                Racun1 = (Math.Atan(tempnum) * (180 / Math.PI)).ToString();
                                break;
                            case "Actg":
                                Racun1 = (Math.Atan2(1, tempnum) * (180 / Math.PI)).ToString();
                                break;
                        }
                        zadnjigumb = "scientificBtn";
                    }

                    if (!containsOperator(Racun2))
                    {
                        Racun2 = Racun2.Replace(tempnum.ToString(), Racun1);
                    }
                    else
                    {
                        Racun2 = Racun2.Substring(0, operatorIndex(Racun2) + 2);
                        Racun2 = Racun2 + Racun1;
                    }

                }
            }

            else if (btn == "btn_pi")
            {
                double tempnum = 0;

                if (zadnjigumb != "scientificBtn" && !isLastOperator(zadnjigumb) && zadnjigumb != "." && Racun1 != "")
                {
                    tempnum = float.Parse(Racun1);
                    Racun3 = tempnum + "π";
                    Racun1 = (tempnum * Math.PI).ToString();
                    if (!containsOperator(Racun2))
                    {
                        Racun2 = Racun2.Replace(tempnum.ToString(), Racun1);
                    }
                    else
                    {
                        Racun2 = Racun2.Substring(0, operatorIndex(Racun2) + 2);
                        Racun2 = Racun2 + Racun1;
                    }
                    zadnjigumb = "scientificBtn";
                }

                else if (Racun1 == "")
                {
                    Racun3 = "π";
                    Racun1 = Math.PI.ToString();
                    Racun2 += Racun1;
                    zadnjigumb = "scientificBtn";
                }
            }

            else if (btn == "btn_xpow2" || btn == "btn_xpow3" || btn == "btn_xinverse" || btn == "btn_logarithm" || btn == "btn_ln" || btn == "btn_exp" || btn == "btn_bin" || btn == "btn_hex" || btn == "btn_sqrt")
            {
                double tempnum = 0;
                string op = btn.Substring(4);

                if (zadnjigumb != "scientificBtn" && !isLastOperator(zadnjigumb) && zadnjigumb != "." && Racun1 != "")
                {
                    tempnum = float.Parse(Racun1);
                    switch (op)
                    {
                        case "xpow2":
                            Racun3 = tempnum + "^2";
                            Racun1 = Math.Pow(tempnum, 2).ToString();
                            break;
                        case "xpow3":
                            Racun3 = tempnum + "^3";
                            Racun1 = Math.Pow(tempnum, 3).ToString();
                            break;
                        case "xinverse":
                            Racun3 = "1/" + tempnum;
                            Racun1 = (1 / tempnum).ToString();
                            break;
                        case "logarithm":
                            Racun3 = "log(" + tempnum + ")";
                            Racun1 = Math.Log10(tempnum).ToString();
                            break;
                        case "ln":
                            Racun3 = "ln(" + tempnum + ")";
                            Racun1 = Math.Log(tempnum).ToString();
                            break;
                        case "exp":
                            Racun3 = "exp(" + tempnum + ")";
                            Racun1 = Math.Exp(tempnum).ToString();
                            break;
                        case "hex":
                            Racun3 = "hex(" + tempnum + ")";
                            Racun1 = Convert.ToString(Convert.ToInt32(tempnum), 16);
                            break;
                        case "bin":
                            Racun3 = "bin(" + tempnum + ")";
                            Racun1 = Convert.ToString(Convert.ToInt32(tempnum), 2);
                            break;
                        case "sqrt":
                            Racun3 = "sqrt(" + tempnum + ")";
                            Racun1 = Math.Sqrt(tempnum).ToString();
                            break;
                    }

                    if (!containsOperator(Racun2))
                    {
                        Racun2 = Racun2.Replace(tempnum.ToString(), Racun1);
                    }
                    else
                    {
                        Racun2 = Racun2.Substring(0, operatorIndex(Racun2) + 2);
                        Racun2 = Racun2 + Racun1;
                    }
                    zadnjigumb = "scientificBtn";
                }
            }

            else if (btn == "btn_percentage")
            {
                if (zadnjigumb != "scientificbtn" && !isLastOperator(zadnjigumb) && zadnjigumb != "." && Racun1 != "" && zadnjigumb != "(" && zadnjigumb != ")")
                {
                    int num1 = 0;
                    int num2 = 0;
                    string operation = Racun2;

                    foreach (char c in operation) //number of brackets
                    {
                        if (c == '(')
                        {
                            num1++;
                        }
                        else if (c == ')')
                        {
                            num2++;
                        }
                    }

                    if (num1 == num2 && containsOperator(Racun2))
                    {
                        Racun2 = calculatePercentage(Racun2, Racun2);
                        zadnjigumb = "scientificBtn";
                        Racun1 = "";
                    }

                    if (num1 > num2)
                    {
                        int index = Racun2.LastIndexOf("(");

                        string Expression = Racun2.Substring(index + 1);

                        if (Expression.Contains(" + ") || Expression.Contains(" - ") || Expression.Contains(" * ") || Expression.Contains(" / "))
                        {
                            Racun2 = calculatePercentage(Expression, Racun2);
                        }

                        zadnjigumb = "scientificBtn";
                        Racun1 = "";
                    }


                }
            }

            else if (btn == "btn_parenthesis")
            {
                string operation = Racun2;

                int num1 = 0;
                int num2 = 0;

                foreach (char c in operation)
                {
                    if (c == '(')
                    {
                        num1++;
                    }
                    else if (c == ')')
                    {
                        num2++;
                    }
                }

                if (zadnjigumb != "." && zadnjigumb != "=")
                {
                    if (Racun2 == "")
                    {
                        Racun2 += "( ";
                        zadnjigumb = "(";
                    }

                    else
                    {
                        if (num2 < num1 && (isLastNumber(Racun2) || isLastClosedParenthesis(Racun2)))
                        {
                            Racun2 += " )";
                            zadnjigumb = ")";
                        }
                        else if (num1 <= num2 && (isLastNumber(Racun2) || isLastClosedParenthesis(Racun2)))
                        {
                            Racun2 += " * ( ";
                            zadnjigumb = "(";
                        }

                        else if (!isLastNumber(Racun2))
                        {
                            Racun2 += "( ";
                            zadnjigumb = "(";
                        }
                    }

                    Racun1 = "";
                }
            }

            else if (btn == "btn_history")
            {
                if (isHistory == "no")
                {
                    isHistory = "yes";
                }
                else if (isHistory == "yes")
                {
                    isHistory = "no";
                }
            }

            //Setting values back
            calc.Racun3 = Racun3;
            calc.Racun1 = Racun2;
            calc.Racun2 = Racun1;
            calc.Zadnjigumb = zadnjigumb;
            calc.isHistory = isHistory;
            calc.Mem = mem;
            calc.ScientificMode = scientificMode;
            calc.IsScientific = isScientific;
        }

        private static bool isLastClosedParenthesis(string Racun2)
        {
            try
            {
                if (Racun2[Racun2.Length - 1] == ')')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool canSwitchOperator(string Racun2)
        {
            try
            {
                string text = Racun2.Substring(Racun2.Length - 3);

                if (text.Contains(" + ") || text.Contains(" - ") || text.Contains(" * ") || text.Contains(" / "))
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool containsOperator(string Racun2)
        {
            if (Racun2.Contains("+") || Racun2.Contains("-") && Racun2.LastIndexOf("-") != 0 || Racun2.Contains("*") || Racun2.Contains("%"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string calculatePercentage(string Expression, string Racun2)
        {
            try
            {
                int index = lastOperatorIndex(Expression, Racun2);
                Expression = Expression.Substring(0, index);


                double operand1;
                double operand2;
                operand1 = Calculator.CalculateExpression(Expression);
                operand2 = Convert.ToDouble(GetLastNumber(Racun2));


                string tempExpression = Racun2.Substring(0, Racun2.Length - operand2.ToString().Length);

                Racun2 = tempExpression + (operand2 / 100 * operand1).ToString();

                return Racun2;
            }

            catch
            {
                return "";
            }
        }

        private static int lastOperatorIndex(string expression, string Racun2)
        {
            char[] operators = { '+', '-', '*', '/' };
            int lastIndex = 0;

            foreach (char c in Racun2)
            {
                if (Array.IndexOf(operators, c) != -1)
                {
                    lastIndex = expression.IndexOf(c, lastIndex + 1);
                }
            }

            return lastIndex;
        }

        private static string GetLastNumber(string input)
        {
            int endIndex = input.Length - 1;
            string number = "";

            while (endIndex >= 0 && (char.IsDigit(input[endIndex]) || input[endIndex] == '.'))
            {
                number = input[endIndex] + number;
                endIndex--;
            }

            return number;
        }

        private static int operatorIndex(string Racun2)
        {
            if (Racun2.Contains("+"))
            {
                return Racun2.IndexOf("+");
            }
            else if (Racun2.Contains("-"))
            {
                return Racun2.IndexOf("-");
            }
            else if (Racun2.Contains("*"))
            {
                return Racun2.IndexOf("*");
            }
            else if (Racun2.Contains("%"))
            {
                return Racun2.IndexOf("%");
            }
            else
            {
                return 0;
            }
        }

        private static bool isLastParenthesis(string Racun2)
        {
            if (Racun2[Racun2.Length - 2] == '(' || Racun2[Racun2.Length - 1] == ')')
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        private static bool isLastOperator(string zadnjigumb)
        {
            if (zadnjigumb == "=" || zadnjigumb == "+" || zadnjigumb == "-" || zadnjigumb == "*" || zadnjigumb == "/")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string InsertCharacter(string originalString, char character, int index)
        {
            StringBuilder finalString = new StringBuilder(originalString);
            finalString.Insert(index, character);
            return finalString.ToString();
        }

        private static string RemoveCharacter(string originalString, int index)
        {
            StringBuilder finalString = new StringBuilder(originalString);
            finalString.Remove(index, 1);
            return finalString.ToString();
        }

        private static bool isLastNumber(string Racun2)
        {
            if (char.IsDigit(Racun2[Racun2.Length - 1]))
            {
                return true;

            }
            else
            {
                return false;
            }
        }

    }
}


