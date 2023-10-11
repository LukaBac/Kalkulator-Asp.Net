using Kalkulator_ASP.Net.Models;
using System;
using System.Text;

namespace Kalkulator_ASP.Net.CalculatorLib
{
    public static class CalculatorInput
    {
        public static void registerInput(string btn, HomeModel calc)
        {
            string Racun2 = calc.Racun1;
            string Racun1 = calc.Racun2;
            string Racun3 = calc.Racun3;
            double mem = calc.Mem;
            string isScientific = calc.IsScientific;

            //za izbjegavanje errora
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
                if(btn != "0")
                {
                    if (!isLastParenthesis(Racun2))
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
            else if(btn == "btn_ce")
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
            else if(btn == "btn_delete")
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
            else if(btn == "btn_dot")
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
            else if(btn == "btn_changeprefix")
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

            //memory NE RADI!!!!
            else if(btn == "btn_memclear")
            {
                mem = 0;
            }
            else if(btn == "btn_memrecall")
            {
                Racun1 = "";
                Racun1 += mem;
                Racun2 += mem;
            }
            else if(btn == "memplus_btn")
            {
                if (Racun1 != "")
                {
                    mem += Convert.ToDouble(Racun1);
                }
            }
            else if(btn == "memreduce_btn")
            {
                if (Racun1 != "")
                {
                    mem -= Convert.ToDouble(Racun1);
                }
            }


            else if(btn == "btn_modeChange")
            {
                Console.WriteLine("changed");
                if(isScientific == "no")
                {
                    isScientific = "yes";
                }
                else if(isScientific == "yes")
                {
                    isScientific = "no";
                }
            }

            //Console.WriteLine(calc.IsScientific);

            //Setting values back
            calc.Racun3 = Racun3;
            calc.Racun1 = Racun2;
            calc.Racun2 = Racun1;
            calc.Zadnjigumb = zadnjigumb;
            calc.Mem = mem;
            calc.IsScientific = isScientific;
            Console.WriteLine("u funkc: " + calc.IsScientific);
        }


        private static bool isLastParenthesis(string Racun2)
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
            //if (Int32.TryParse(zadnjigumb, out int broj))
            //{
            //    return true;
            //}
            //else {
            //    return false;
            //}

            if (Char.IsDigit(Racun2[Racun2.Length - 1]))
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
