using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
    private const int MinNumber = -64;
    private const int MaxNumber = 128;
    private readonly Random _random = new Random();
    private readonly int _targetNumber = 0;
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
          int value = _random.Next(MinNumber, MaxNumber);
          solutionSelectables.Add(new Number(value));
          providedSelectables.Add(new Number(value));
        }
        else
        {
          BinaryOperatorType operatorType = GetRandomOperator(currentLevel);
          solutionSelectables.Add(new BinaryOperator(operatorType));
          providedSelectables.Add(new BinaryOperator(operatorType));
        }
      }

      int targetNumber = EvaluateSelectables(solutionSelectables);

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

    /**
     * Function to check if the user's solution is correct
     *
     * @param userSelectables - the selectables the user has selected
     *
     * @return true if the user's solution is correct, false otherwise
     *
     * @author Sebastian Kjallgren
     */
    public bool IsSolutionCorrect(List<ProblemSelectable> userSelectables)
    {
      return EvaluateSelectables(userSelectables) == _targetNumber;
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
      string expression = "";

      foreach (var selectable in selectables)
      {
        if (selectable is Number num)
        {
          string number = num.ToString();
          expression += number + " ";
        }
        else if (selectable is BinaryOperator binaryOperator)
        {
          switch (binaryOperator.BinOp)
          {
            case BinaryOperatorType.Addition:
              expression += "+ ";
              break;
            case BinaryOperatorType.Subtraction:
              expression += "- ";
              break;
            case BinaryOperatorType.Multiplication:
              expression += "* ";
              break;
            case BinaryOperatorType.Division:
              expression += "/ ";
              break;
          }
        }
      }

      return Convert.ToInt32(new DataTable().Compute(expression, null));
    }
  }
}
