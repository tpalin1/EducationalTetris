// This following line prevents the error "The predefined type 'System.Runtime.CompilerServices.IsExternalInit" from
// being thrown.
namespace System.Runtime.CompilerServices
{
  public static class IsExternalInit {}
}

namespace ProblemSetSpace
{

  /**
   * A ProblemSelectable is one that a user chooses - it is either a number or an operator.
   *
   * This is an abstract class, so it cannot be instantiated. It is used to group together child records.
   *
   * @author Sebastian Kjallgren
   */
  public abstract record ProblemSelectable;

  /**
   * A BinaryOperator is a ProblemSelectable that represents an operator that can be used in a problem.
   *
   * @author Sebastian Kjallgren
   */
  public record BinaryOperator(BinaryOperatorType BinOp) : ProblemSelectable;

  /**
   * The BinaryOperatorType enum represents the different types of binary operators that can be used in a problem.
   *
   * @author Sebastian Kjallgren
   */
  public enum BinaryOperatorType
  {
    Addition,
    Subtraction,
    Multiplication,
    Division,
  }

  /**
   * A Number is a ProblemSelectable that represents a number that can be used in a problem.
   *
   * @author Sebastian Kjallgren
   */
  public record Number(int Value) : ProblemSelectable;

}
