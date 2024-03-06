using System.Collections.Generic;
using NUnit.Framework;
using ProblemSetSpace;

namespace TestsProblemSet
{
  [TestFixture]
  [TestOf(typeof(ProblemSet))]
  public class ProblemSetTest
  {
    private ProblemSet _problemSet = null;

    public ProblemSetTest()
    {
      _problemSet = new ProblemSet();
    }

    /// <summary>
    /// Test to get a valid problem set for levels 0 - 9.
    ///
    /// We expect to get a problem set that has numbers between -64 and 128, and only the addition and subtraction
    /// operators.
    /// </summary>
    [Test]
    public void GetValidProblemSetLevel0To9()
    {
      int startLevel = 0;
      int endLevel = 10;
      int minNumber = -64;
      int maxNumber = 128;

      List<BinaryOperatorType> operators = new List<BinaryOperatorType> {BinaryOperatorType.Addition,
        BinaryOperatorType.Subtraction};

      for (int level = startLevel; level < endLevel; level++)
      {
        var problemSelectables = _problemSet.GetNewProblemSet(level);

        ValidProblemHelper(minNumber, maxNumber, operators, problemSelectables);
      }

    }

    /// <summary>
    /// Test to get a valid problem set for levels 10 - 30.
    ///
    /// We expect to get a problem set that has numbers between -64 and 128, and only the addition and subtraction
    /// operators.
    /// </summary>
    [Test]
    public void GetValidProblemSetLevel10To30()
    {
      int startLevel = 10;
      int endLevel = 30;
      int minNumber = -64;
      int maxNumber = 128;

      List<BinaryOperatorType> operators = new List<BinaryOperatorType> {BinaryOperatorType.Addition,
        BinaryOperatorType.Subtraction, BinaryOperatorType.Multiplication};

      for (int level = startLevel; level < endLevel; level++)
      {
        var problemSelectables = _problemSet.GetNewProblemSet(level);

        ValidProblemHelper(minNumber, maxNumber, operators, problemSelectables);
      }
    }

    /// <summary>
    /// Test to get a valid problem set for levels over 30.
    ///
    /// We expect to get a problem set that has numbers between -64 and 128, and all the binary operators.
    /// </summary>
    [Test]
    public void GetValidProblemSetLevel30Plus()
    {
      int startLevel = 30;
      int endLevel = 35;
      int minNumber = -64;
      int maxNumber = 128;

      List<BinaryOperatorType> operators = new List<BinaryOperatorType> {BinaryOperatorType.Addition,
        BinaryOperatorType.Subtraction, BinaryOperatorType.Multiplication, BinaryOperatorType.Division};

      for (int level = startLevel; level < endLevel; level++)
      {
        var problemSelectables = _problemSet.GetNewProblemSet(level);

        ValidProblemHelper(minNumber, maxNumber, operators, problemSelectables);
      }
    }


    /// <summary>
    /// A helper function for GetValidProblemSetLevel-functions to validate the problem set.
    ///
    /// Takes in the minimum and maximum number that the problem set should contain, as well a list of operators that it
    /// can contain.
    /// </summary>
    private void ValidProblemHelper(int minNumber, int maxNumber, List<BinaryOperatorType> operators,
      (int, List<ProblemSelectable>) problemSet)
    {
      var (targetNumber, problemSelectables) = problemSet;
      int numSelectables = problemSelectables.Count;

      Assert.GreaterOrEqual(targetNumber, minNumber);
      Assert.LessOrEqual(targetNumber, maxNumber);

      foreach (var selectable in problemSelectables)
      {
        if (selectable is Number number)
        {
          int value = number.Value;
          Assert.GreaterOrEqual(value, minNumber);
          Assert.LessOrEqual(value, maxNumber);
        }
        else if (selectable is BinaryOperator binaryOperator)
        {
          Assert.IsTrue(operators.Contains(binaryOperator.BinOp));
        }
      }
    }

    /// <summary>
    /// A test to see if evaluating an expression with addition is done correct.
    /// </summary>
    [Test]
    public void EvaluateAddition()
    {
      List<ProblemSelectable> selectables = new List<ProblemSelectable>
      {
        new Number(1),
        new BinaryOperator(BinaryOperatorType.Addition),
        new Number(5),
        new BinaryOperator(BinaryOperatorType.Addition),
        new Number(3)
      };

      int result = _problemSet.EvaluateSelectables(selectables);

      Assert.AreEqual(9, result);
    }

    /// <summary>
    /// A test to see if evaluating an expression with multiplication is done correct.
    /// </summary>
    [Test]
    public void EvaluateMultiplication()
    {
      List<ProblemSelectable> selectables = new List<ProblemSelectable>
      {
        new Number(1),
        new BinaryOperator(BinaryOperatorType.Multiplication),
        new Number(5),
        new BinaryOperator(BinaryOperatorType.Multiplication),
        new Number(3)
      };

      int result = _problemSet.EvaluateSelectables(selectables);

      Assert.AreEqual(15, result);
    }

    /// <summary>
    /// A test to see if evaluating an expression with division is done correct.
    /// </summary>
    [Test]
    public void EvaluateDivision()
    {
      List<ProblemSelectable> selectables = new List<ProblemSelectable>
      {
        new Number(25),
        new BinaryOperator(BinaryOperatorType.Division),
        new Number(5),
      };

      int result = _problemSet.EvaluateSelectables(selectables);

      Assert.AreEqual(5, result);
    }
  }
}
