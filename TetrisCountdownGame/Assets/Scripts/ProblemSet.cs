using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine; // Add this line

namespace ProblemSetSpace
{

  /**
   * Class to represent a problem, in which the user is given a number (and some selectables), and has to prove that a
   * certain order and subset of these selectables produces the number.
   *
   * @author Sebastian Kjallgren
   */
  public class ProblemSet
  {
    
    private const int MinNumber = 0;
    private const int MaxNumber = 30;

    private readonly System.Random _random = new System.Random();

    private readonly int _targetNumber = 0;

    public int targetNumber;
    private List<ProblemSelectable> _solutionSelectables;
    private List<ProblemSelectable> _providedSelectables;


    

    /**
     * Function to retrieve a new problem set
     *
     * @param currentLevel - the current level of the game
     *
     * @return a tuple containing the number and the selectables
     *
     * @author Sebastian Kjallgren
     */
    public (int, List<ProblemSelectable>) GetNewProblemSet(int currentLevel)
    {
      int numSelectables = _random.Next(5, 10); // Random number of selectables between 5 and 9
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
          providedSelectables.Add(new BinaryOperator(operatorType));
        }
      }

     targetNumber = EvaluateSelectables(solutionSelectables);

      // add some more selectables to the providedSelectables
      int j = _random.Next(1, 3);
      for (int i = 0; i < j; i++)
      {
        if (i % 2 == 0)
        {
          providedSelectables.Add(new Number(_random.Next(MinNumber, MaxNumber)));
        }
        else
        {
          BinaryOperatorType operatorType = GetRandomOperator(currentLevel);
          providedSelectables.Add(new BinaryOperator(operatorType));
        }
      }

      // shuffles the provided selectables
      providedSelectables = providedSelectables.OrderBy(x => _random.Next()).ToList();

      // if the target number is too low or too high then recurse to get a new problem set
      if (targetNumber is < MinNumber or > MaxNumber)
      {
        return GetNewProblemSet(currentLevel);
      }

      return (targetNumber, providedSelectables);
    }

    /// <summary>
    /// Gets the operators that can be used in the given level.
    /// </summary>
    /// <param name="currentLevel"></param>
    /// <returns>List of BinaryOperatorType</returns>
    private List<BinaryOperatorType> GetOperatorsForLevel(int currentLevel)
    {
      List<BinaryOperatorType> operators = new List<BinaryOperatorType> {BinaryOperatorType.Addition, BinaryOperatorType.Subtraction};

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
    /// <returns></returns>
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

    /**
     * Function to check if the user's solution is correct
     *
     * @param userSelectables - the selectables the user has selected
     *
     * @return true if the user's solution is correct, false otherwise
     *
     * @author Sebastian Kjallgren
     */
    // public bool IsSolutionCorrect(List<ProblemSelectable> userSelectables)
    // {
    //   return EvaluateSelectables(userSelectables) == _targetNumber;
    // }


     //Get the users current input and see if it matches the target number
    public bool IsSolutionCorrect(List<ProblemSelectable> userSelectables)
    {

        int userSolution = EvaluateSelectables(userSelectables);
        UnityEngine.Debug.Log("Target number: " + targetNumber);

        Debug.Log("User solution: " + userSolution);

        return userSolution == targetNumber;
    }

    /**
     * Function to evaluate the selectables list to a number
     *
     * @param selectables - the selectables to evaluate
     *
     * @return the number that the selectables evaluate to
     *
     * @author Sebastian Kjallgren
     */



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
private  int Precedence(BinaryOperatorType op)
{
    return op switch
    {
        BinaryOperatorType.Addition or BinaryOperatorType.Subtraction => 1,
        BinaryOperatorType.Multiplication or BinaryOperatorType.Division => 2,
        _ => 0,
    };
}

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
