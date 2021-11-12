using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeTest.DataAccess.MSSQL.Models;

namespace ExpressionTreeTest.DataAccess.MSSQL
{
    /// <summary>
    /// Обратная польская запись.
    /// </summary>
    public class ReversePolishNotation
    {
        /// <summary>
        /// Операции с приоритетом ( наивысший = 0 ).
        /// </summary>
        private readonly Dictionary<char, int> _operations = new Dictionary<char, int>()
        {
            { '(', 0 },
            { ')', 1 },
            { '|', 2 },
            { '&', 2 }
        };

        private readonly string _delimeters = " ";

        /// <summary>
        /// Является ли символ разделителем.
        /// </summary>
        /// <param name="c">Символ для проверки.</param>
        /// <returns>true, если проверяемый символ - разделитель.</returns>
        private bool IsDelimeter(char c)
        {
            if ((_delimeters.IndexOf(c) != -1))
                return true;
            return false;
        }

        /// <summary>
        /// Является ли символ оператором.
        /// </summary>
        /// <param name="с">Символ для проверки.</param>
        /// <returns>true, если проверяемый символ - оператор.</returns>
        private bool IsOperator(char op)
        {
            if (_operations.ContainsKey(op))
                return true;
            return false;
        }

        /// <summary>
        /// Получить приоритет операций.
        /// </summary>
        /// <param name="op">Символ операции.</param>
        /// <returns>Приоритет.</returns>
        private int GetPriority(char op)
        {
            if (_operations.ContainsKey(op) == false)
                return _operations.Values.Max() + 1;

            return _operations[op];
        }

        /// <summary>
        /// Проверка строки на открытые и закрытые скобки.
        /// </summary>
        /// <param name="brackets_string"></param>
        /// <returns></returns>
        public bool CheckBrackets(string brackets_string)
        {
            Dictionary<char, char> pairs = new Dictionary<char, char>()
            {
                { '(', ')' },
            };

            var stack = new Stack<char>();
            for (int i = 0; i < brackets_string.Length; i++) {
                if (pairs.ContainsKey(brackets_string[i]) | pairs.ContainsValue(brackets_string[i]))
                    if (pairs.ContainsKey(brackets_string[i]))
                        stack.Push(brackets_string[i]);
                    else {
                        char stackTopSymbol;
                        var popResult = stack.TryPop(out stackTopSymbol);
                        if (popResult == false)
                            return false;

                        var stackTopSymbolPair = pairs[stackTopSymbol];
                        if (stackTopSymbolPair != brackets_string[i])
                            return false;
                    }
            }

            return stack.Count() == 0;
        }

        /// <summary>
        /// Проверка строки на содержание операций или разделителя.
        /// </summary>
        /// <returns>Результат проверки.</returns>
        public bool CheckString(string input)
        {
            for (int i = 0; i < input.Length; i++) 
            {
                if (
                    (
                        IsDelimeter(input[i]) |
                        IsOperator(input[i]) |
                        char.IsDigit(input[i])
                    ) == false)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Получить обратную польскую запись операций. 
        /// </summary>
        /// <param name="input">Строка операций.</param>
        /// <returns></returns>
        public string GetRpnStringRule(string input)
        {
            if (CheckBrackets(input) == false)
                throw new Exception($"Some trouble with brackets in string: \"{input}\".");

            if (CheckString(input) == false)
                throw new Exception($"String must contain operator or delimeter symbols. \"{input}\"");

            string output = string.Empty; //Строка для хранения выражения
            Stack<char> operStack = new Stack<char>(); //Стек для хранения операторов

            for (int i = 0; i < input.Length; i++) //Для каждого символа в входной строке
            {
                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //Переходим к следующему символу

                //Если символ - цифра, то считываем все число
                if (char.IsDigit(input[i])) //Если цифра
                {
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i])) {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }

                //Если символ - оператор
                if (IsOperator(input[i])) 
                {
                    if (input[i] == '(') //Если символ - открывающая скобка
                        operStack.Push(input[i]); //Записываем её в стек
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(') {
                            output += s.ToString() + ' ';
                            s = operStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {
                        if (operStack.Count > 0) //Если в стеке есть элементы
                            if (GetPriority(input[i]) <= GetPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                        operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека
                    }
                }
            }

            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output; //Возвращаем выражение в постфиксной записи
        }

        /// <summary>
        /// Сформировать предикат из строки представляющей обратную польскую запись.
        /// </summary>
        /// <typeparam name="T">Параметр типа.</typeparam>
        /// <param name="rpnStringRule">Строка в формате обратной польской записи.</param>
        /// <param name="filterParams">Список параметров фильтрации.</param>
        /// <param name="expParam">Параметр для дерева выражений.</param>
        /// <returns></returns>
        public Expression FormWherePredicate<T>(string rpnStringRule, List<FilterParam> filterParams, ParameterExpression expParam)
        {
            Expression result = null; //Результат
            Stack<Expression> temp = new Stack<Expression>(); //Временный стек для решения
            ExpressionBuilder _expressionBuilder = new ExpressionBuilder();

            for (int i = 0; i < rpnStringRule.Length; i++) //Для каждого символа в строке
            {
                //Если символ - цифра, то читаем все число и записываем на вершину стека
                if (char.IsDigit(rpnStringRule[i])) {
                    string stringIndex = string.Empty;

                    while (!IsDelimeter(rpnStringRule[i]) && !IsOperator(rpnStringRule[i])) //Пока не разделитель
                    {
                        stringIndex += rpnStringRule[i]; //Добавляем
                        i++;
                        if (i == rpnStringRule.Length) break;
                    }
                    
                    Expression exp = _expressionBuilder.GetExpression<T>(expParam, filterParams[int.Parse(stringIndex)]);

                    temp.Push(exp); //Записываем в стек
                    i--;
                }
                else if (IsOperator(rpnStringRule[i])) //Если символ - оператор
                {
                    //Берем два последних значения из стека
                    Expression left = temp.Pop();
                    Expression right = temp.Pop();

                    switch (rpnStringRule[i]) //И производим над ними действие, согласно оператору
                    {
                        case '|':
                            result = Expression.Or(left, right);
                            break;
                        case '&':
                            result = Expression.And(left, right);
                            break;
                    }
                    temp.Push(result); //Результат вычисления записываем обратно в стек
                }
            }
            return temp.Peek(); //Забираем результат всех вычислений из стека и возвращаем его
        }
    }
}
