namespace Kalkulator_ASP.Net.Libraries.CalculatorLib
{
    public static class Calculator
    {
        public static double CalculateExpression(string expression)
        {
            string[] SplitExpression = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            List<string> ExpressionList = SplitExpression.ToList();
            List<double> numberList = new List<double>();
            List<char> operandList = new List<char>();

            Console.WriteLine("\n");
            Console.WriteLine("\n\n\n");
            try
            {
                if (ExpressionList.Count(x => x == "(") != ExpressionList.Count(x => x == ")")) //provjeravanje broja zagrada
                {
                    while (ExpressionList.Count(x => x == "(") > ExpressionList.Count(x => x == ")"))
                    {
                        ExpressionList.RemoveAt(ExpressionList.LastIndexOf("("));
                    }
                }

                if (runCheck(ExpressionList) == 1)
                {
                    while (ExpressionList.Contains("(") || ExpressionList.Contains(")")) // rijesavanje zagrada
                    {
                        numberList.Clear();
                        operandList.Clear();
                        List<double> tempNumberList = new List<double>();
                        List<char> tempOperandList = new List<char>();


                        int startIndex = ExpressionList.FindLastIndex(ch => ch == "(");
                        int index = startIndex;
                        if (startIndex != -1)
                        {
                            while (ExpressionList[index] != ")")
                            {
                                if (double.TryParse(ExpressionList[index], out double DoubleValue)) //ako je broj
                                {
                                    numberList.Add(DoubleValue);
                                }
                                else if (ExpressionList[index] == "+" || ExpressionList[index] == "-" || ExpressionList[index] == "*" || ExpressionList[index] == "/")
                                {
                                    operandList.Add(ExpressionList[index][0]);
                                }
                                index++;
                            }

                            ExpressionList.RemoveRange(startIndex, index - startIndex + 1);
                            ExpressionList.Insert(startIndex, solveSubExpression(numberList, operandList).ToString());
                        }
                    }

                    numberList.Clear();
                    operandList.Clear();
                    foreach (string element in ExpressionList) // rijesavanje ostatka jednadzbe
                    {
                        if (double.TryParse(element, out double DoubleValue)) //ako je broj
                        {
                            numberList.Add(DoubleValue);
                        }
                        else if (element == "+" || element == "-" || element == "*" || element == "/")
                        {
                            operandList.Add(element[0]);
                        }
                    }

                    return solveSubExpression(numberList, operandList);

                }

                else if (runCheck(ExpressionList) == 2) // ako je jednadzba jedan broj
                {
                    return Convert.ToDouble(expression);
                }

                else
                {
                    throw new ArgumentException("error");
                }
            }
            catch
            {
                throw new ArgumentException("error");
            }

        }

        private static int runCheck(List<string> expression)
        {
            int numOperand = 0;
            int numNumbers = 0;

            foreach (string element in expression)
            {
                if (double.TryParse(element, out double DoubleValue)) //ako je broj
                {
                    numNumbers++;
                }
                else if (element == "+" || element == "-" || element == "*" || element == "/")
                {
                    numOperand++;
                }
                else if (element != "(" && element != ")")
                {
                    return 0;
                }
            }

            if (numNumbers - 1 == numOperand)
            {
                return 1;
            }

            else if (numNumbers == 1 && numOperand == 0)
            {
                return 2;
            }

            else
            {
                return 0;
            }
        }

        private static double solveSubExpression(List<double> numberList, List<char> operandList)
        {
            //checks for division and multiplication
            while (operandList.Contains('*') || operandList.Contains('/'))
            {
                for (int i = 0; i < operandList.Count; i++)
                {
                    if (operandList[i] == '*' || operandList[i] == '/')
                    {
                        double calculation = makeCalculation(numberList[i], numberList[i + 1], operandList[i]);

                        numberList[i] = calculation;
                        numberList.RemoveAt(i + 1);
                        operandList.RemoveAt(i);
                    }
                }
            }


            //solves all other operands
            while (operandList.Count > 0)
            {
                for (int i = 0; i < operandList.Count; i++)
                {
                    if (operandList[i] == '+' || operandList[i] == '-')
                    {
                        double calculation = makeCalculation(numberList[i], numberList[i + 1], operandList[i]);

                        numberList[i] = calculation;
                        numberList.RemoveAt(i + 1);
                        operandList.RemoveAt(i);
                    }
                }
            }

            return numberList[0];
        }

        private static double makeCalculation(double operand1, double operand2, char op)
        {
            switch (op)
            {
                case '+':
                    return operand1 + operand2;

                case '-':
                    return operand1 - operand2;
                case '*':
                    return operand1 * operand2;
                case '/':
                    if (operand2 != 0)
                    {
                        return operand1 / operand2;
                    }
                    else
                    {
                        throw new ArgumentException("error");
                    }
                default:
                    throw new ArgumentException("error");
            }
        }
    }
}
