using UnityEngine;
using UnityEngine.UI; // Add this line
using TMPro;
using System.Collections.Generic;
using ProblemSetSpace;
using GameStateSpace;

public class ProblemSetController : MonoBehaviour, IGameStateObserver
{
    private ProblemSet problemSet;

 public Transform buttonContainer; // Add this line
   public Transform numberButtonContainer; // Container for number buttons
    public Transform operatorButtonContainer; // Container for operator buttons

    public TextMeshProUGUI equationContainer; // Container for the equation
    public Button buttonPrefab; // Add this line

     public TextMeshProUGUI targetNumberText; // Add this line



    public List<ProblemSelectable> userSelectables = new List<ProblemSelectable>(); //


    private int currentLevel = 1;



   void Start()
    {
        problemSet = new ProblemSet();
        var newProblem = problemSet.GetNewProblemSet(currentLevel);
        Debug.Log("Target Number: " + newProblem.Item1);


        Debug.Log("Selectables: " + string.Join(", ", newProblem.Item2));

        // Set the target number text
        targetNumberText.text = "Target Number: " + newProblem.Item1; // Add this line



       // Create a button for each number selectable
        foreach (var selectable in newProblem.Item2)
        {
           if (selectable is Number number)
            {
                Button button = Instantiate(buttonPrefab, buttonContainer.transform);
                string buttonText = number.ToString().Split('=')[1].Trim(' ', '}'); // Add this line
                button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText; // Change this line

                // Add a click listener to the button
                button.onClick.AddListener(() => OnButtonClicked(selectable));
            }

            //Add the buttons to the correct container
            else if (selectable is BinaryOperator binaryOperator)
            {
                Button button = Instantiate(buttonPrefab, operatorButtonContainer.transform);
                string buttonText = "";
                switch (binaryOperator.BinOp)
                {
                    case BinaryOperatorType.Addition:
                        buttonText = "+";
                        break;
                    case BinaryOperatorType.Subtraction:
                        buttonText = "-";
                        break;
                    // Add cases for other binary operators here
                }
                button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
                button.onClick.AddListener(() => OnButtonClicked(selectable));
            }

        }

    }

    

    //

    public void OnGameStateChanged(GameStateEnum gameStateEnum)
    {
        return;
    }

    void OnButtonClicked(ProblemSelectable selectable)
{
    Debug.Log("Button clicked: " + selectable);
    userSelectables.Add(selectable);
    CheckSolution(userSelectables);


    // Add the selected button to the equation
    if (selectable is Number number)
    {
        Debug.Log("Button clicked: " + selectable);
        // Here you can add the code to handle the button click
        equationContainer.text += number.Value.ToString() + " "; // Append number to the equation
    }
    else if (selectable is BinaryOperator binaryOperator)
    {
        switch (binaryOperator.BinOp)
        {
            case BinaryOperatorType.Addition:
                equationContainer.text += "+ "; // Append operator to the equation
                break;
            case BinaryOperatorType.Subtraction:
                equationContainer.text += "- "; // Append operator to the equation
                break;
            // Add cases for other binary operators here
        }
    }
}



    // Call this method when the user submits their solution
    public void CheckSolution(List<ProblemSelectable> userSelectables)
    {
        if(userSelectables == null)
        {
            Debug.Log("User selectables is null");
        }
        Debug.Log("Here is current solution" + string.Join(", ", userSelectables));
        if (problemSet.IsSolutionCorrect(userSelectables))
        {
            Debug.Log("Correct solution!");
            GameState.Instance.SetGameState(GameStateEnum.TetrisPlayable);
            currentLevel++;


            // // get mew level
            // var newProblem = problemSet.GetNewProblemSet(currentLevel);
        }
        else
        {
            Debug.Log("Incorrect solution.");
        }


    }


}
