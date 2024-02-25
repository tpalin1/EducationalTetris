using System;
using System.Collections.Generic;
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
    // TODO:
    // if current level < 10 - only + and -
    // if current level < 20 - +, -, *, (, )
    // if current level < 30 - +, -, *, , (, ), /

    int targetNumber = _random.Next(MinNumber, MaxNumber);
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
        // randomly add the operator + or - to solutionSelectables
        BinaryOperatorType operatorType = _random.Next(0, 2) == 0 ? BinaryOperatorType.Addition :
          BinaryOperatorType.Subtraction;
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
        // at random, add either a + or - operator
        BinaryOperatorType operatorType = _random.Next(0, 2) == 0 ? BinaryOperatorType.Addition :
          BinaryOperatorType.Subtraction;
        providedSelectables.Add(new BinaryOperator(operatorType));
      }
    }

    // shuffles the provided selectables
    providedSelectables = providedSelectables.OrderBy(x => _random.Next()).ToList();

    return (targetNumber, providedSelectables);
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
  private int EvaluateSelectables(List<ProblemSelectable> selectables)
  {
    int result = 0;
    BinaryOperatorType lastOperation = BinaryOperatorType.Addition;

    // TODO, make sure it evaluates the selectables correctly
    // e.g. takes into account operator precedence and associativity

    foreach (var selectable in selectables)
    {
      if (selectable is Number num)
      {
        switch (lastOperation)
        {
          case BinaryOperatorType.Addition:
            result += num.Value;
            break;
          case BinaryOperatorType.Subtraction:
            result -= num.Value;
            break;
          case BinaryOperatorType.Multiplication:
            result *= num.Value;
            break;
          case BinaryOperatorType.Division:
            result = num.Value != 0
              ? result / num.Value
              : result; // just keep the same result if division by zero
            break;
        }
      }
      else if (selectable is BinaryOperator binaryOperator)
      {
        // Store the last operator to apply in the next loop iteration
        lastOperation = binaryOperator.BinOp;
      }
    }

    return result;
  }
}

}
