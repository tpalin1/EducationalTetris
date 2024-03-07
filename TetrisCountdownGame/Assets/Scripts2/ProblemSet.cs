using System;
using System.Collections.Generic;
using System.Linq;

namespace ProblemSetSpace
{
    /// <summary>
    /// Class to represent a randomised maths "countdown" problem for the user to solve.
    /// </summary>
    public class ProblemSet
    {
        private const int MinNumber = 0;
        private const int MaxNumber = 9;

        private readonly System.Random _random = new System.Random();

        private int _targetNumber = 0;

        private List<ProblemSelectable> _solutionSelectables;
        private List<ProblemSelectable> _providedSelectables;

        /// <summary>
        /// Generates a new problem set for the user to solve.
        /// </summary>
        /// <returns>A tuple containing the number and the selectables</returns>
        /// <author>Sebastian Kjallgren, Tom Palin</author>
        public (int, List<ProblemSelectable>) GetNewProblemSet(int currentLevel)
        {
            int numSelectables = _random.Next(3, 6); // Random number of selectables between 3 and 6
            List<ProblemSelectable> solutionSelectables = new List<ProblemSelectable>();
            List<ProblemSelectable> providedSelectables = new List<ProblemSelectable>();

            // generate the solutionSelectables
            for (int i = 0; i < numSelectables; i++)
            {
                if (i % 2 == 0)
                {
                    Number value = GetRandomNumber(currentLevel);
                    solutionSelectables.Add(value);
                    providedSelectables.Add(value);
                }
                else
                {
                    BinaryOperatorType operatorType = GetRandomOperator(currentLevel);
                    solutionSelectables.Add(new BinaryOperator(operatorType));
                }
            }

            _targetNumber = EvaluateSelectables(solutionSelectables);

            // add some more selectables to the providedSelectables -- this is to make the problem more interesting
            int j = _random.Next(1, 2);
            for (int i = 0; i < j; i++)
            {
                providedSelectables.Add(GetRandomNumber(currentLevel));
            }

            // add the available operators to the providedSelectables
            foreach (var op in GetOperatorsForLevel(currentLevel))
            {
                providedSelectables.Add(new BinaryOperator(op));
            }

            // shuffles the provided selectables
            providedSelectables = providedSelectables.OrderBy(x => _random.Next()).ToList();

            // if the target number is too low or too high then recurse to get a new problem set
            return _targetNumber is < MinNumber or > MaxNumber 
                ? GetNewProblemSet(currentLevel) 
                : (_targetNumber, providedSelectables);
        }

        /// <summary>
        /// Gets the operators that can be used in the given level.
        /// </summary>
        /// <param name="currentLevel"></param>
        /// <returns>List of BinaryOperatorType</returns>
        /// <author>Sebastian Kjallgren</author>
        private List<BinaryOperatorType> GetOperatorsForLevel(int currentLevel)
        {
          List<BinaryOperatorType> operators = new List<BinaryOperatorType> {BinaryOperatorType.Addition, 
              BinaryOperatorType.Subtraction};

          if (currentLevel >= 10)
          {
            operators.Add(BinaryOperatorType.Multiplication);
          }

          if (currentLevel >= 30)
          {
            operators.Add(BinaryOperatorType.Division);
          }

          return operators;
        }

        /// <summary>
        /// Gets a random operator from the list of available operators
        /// </summary>
        /// <param name="currentLevel"></param>
        /// <returns>BinaryOperatorType</returns>
        /// <author>Sebastian Kjallgren</author>
        private BinaryOperatorType GetRandomOperator(int currentLevel)
        {
          List<BinaryOperatorType> operators = GetOperatorsForLevel(currentLevel);
          return operators[_random.Next(0, operators.Count)];
        }

        /// <summary>
        /// When the user is at level 0-10 we give small numbers, in the range 0-20.
        ///
        /// As they progress to level 10-20 we give numbers in the range 0-50.
        ///
        /// Then they progress to level 20-30 we give numbers in the range 0-100.
        /// </summary>
        /// <param name="currentLevel"></param>
        /// <returns>The random number</returns>
        /// <author>Sebastian Kjallgren</author>
        private Number GetRandomNumber(int currentLevel)
        {
          switch (currentLevel)
          {
            case < 10:
              return new Number(_random.Next(0, 20));
            case < 20:
              return new Number(_random.Next(0, 50));
            default:
              return new Number(_random.Next(0, 100));
          }
        }

        /// <summary>
        /// Check if the user's solution is correct.
        /// </summary>
        /// <param name="userSelectables"></param>
        /// <returns>True if the solution is correct, otherwise false</returns>
        /// <author>Sebastian Kjallgren</author>
        public bool IsSolutionCorrect(List<ProblemSelectable> userSelectables)
        {
          return EvaluateSelectables(userSelectables) == _targetNumber;
        }

        /// <summary>
        /// Evaluates the selectables to get the integer.
        /// </summary>
        /// <param name="selectables"></param>
        /// <returns>The integer that the selections evaluate to</returns>
        /// <author>Sebastian Kjallgren, Tom Palin</author>
        public int EvaluateSelectables(List<ProblemSelectable> selectables)
        {
            Stack<int> numbers = new Stack<int>();
            Stack<BinaryOperatorType> operators = new Stack<BinaryOperatorType>();

            foreach (var selectable in selectables)
            {
                if (selectable is Number num)
                {
                    numbers.Push(num.Value);
                }
                else if (selectable is BinaryOperator binaryOperator)
                {
                    while (operators.Count > 0 && numbers.Count > 1 && Precedence(binaryOperator.BinOp) <= Precedence(operators.Peek()))
                    {
                        ApplyOperation(numbers, operators);
                    }
                    operators.Push(binaryOperator.BinOp);
                }
            }

            while (operators.Count > 0 && numbers.Count > 1)
            {
                ApplyOperation(numbers, operators);
            }

            return numbers.Count > 0 ? numbers.Pop() : 0;
        }

        /// <summary>
        /// Funtion that returns the precedence of the operator.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <author>Sebastian Kjallgren, Tom Palin</author>
        private int Precedence(BinaryOperatorType op)
        {
            return op switch
            {
                BinaryOperatorType.Addition or BinaryOperatorType.Subtraction => 1,
                BinaryOperatorType.Multiplication or BinaryOperatorType.Division => 2,
                _ => 0,
            };
        }

        /// <summary>
        /// Function that applies the operation to the numbers.
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="operators"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <author>Sebastian Kjallgren, Tom Palin</author>
        private  void ApplyOperation(Stack<int> numbers, Stack<BinaryOperatorType> operators)
        {
            int b = numbers.Pop();
            int a = numbers.Pop();
            BinaryOperatorType op = operators.Pop();

            int result = op switch
            {
                BinaryOperatorType.Addition => a + b,
                BinaryOperatorType.Subtraction => a - b,
                BinaryOperatorType.Multiplication => a * b,
                BinaryOperatorType.Division => a / b,
                _ => throw new ArgumentException("Unknown operator"),
            };

            numbers.Push(result);
        }
    }
}
