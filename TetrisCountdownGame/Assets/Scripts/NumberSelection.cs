using UnityEngine;
using UnityEngine.UI; // Add this line
using TMPro;
using System.Collections.Generic;
using GameStateSpace;
using ProblemSetSpace;

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

        
        //subscribe to game state
        // GameState.Instance.Subscribe(this);
// Subscribe to the game state
    GameState.Instance.Subscribe(this);
    

        //Subscribe to the game state
        // GameState.Instance.Subscribe(this);
        
        problemSet = new ProblemSet();
        var newProblem = problemSet.GetNewProblemSet(currentLevel);
        Debug.Log("Target Number: " + newProblem.Item1);


        Debug.Log("Selectables: " + string.Join(", ", newProblem.Item2));

        // Set the target number text
        targetNumberText.text = " " + newProblem.Item1; // Add this line



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
   
    /// <summary>
    /// </summary>
    /// <param name="gameState"></param>
    public void OnGameStateChanged(GameStateEnum gameState)
    {
        //iF THE game stae is countdownbeingsolved we refresh with a new problem set like we did in start

        if(gameState == GameStateEnum.TetrisGameUnsolved){
            //Clear and make a new problem 
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in operatorButtonContainer)
            {
                Destroy(child.gameObject);
            }
        }

        if(gameState == GameStateEnum.CountdownBeingSolved)
        {
            // Clear the user selectables
            userSelectables.Clear();
            // Clear the equation
            equationContainer.text = "";
            // Get a new problem set
            var newProblem = problemSet.GetNewProblemSet(currentLevel);
            // Set the target number text
            targetNumberText.text = " " + newProblem.Item1;
            // Create a button for each number selectable
            foreach (var selectable in newProblem.Item2)
            {
                if (selectable is Number number)
                {
                    Button button = Instantiate(buttonPrefab, buttonContainer.transform);
                    string buttonText = number.ToString().Split('=')[1].Trim(' ', '}');
                    button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
                    button.onClick.AddListener(() => OnButtonClicked(selectable));
                }
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

        
    }

    

    //

    

    void OnButtonClicked(ProblemSelectable selectable)
{
    Debug.Log("Button clicked: " + selectable);
    userSelectables.Add(selectable);
    CheckSolution();


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
    
    // check if the solution is correct
    CheckSolution();
}



    // Call this method when the user submits their solution
    public void CheckSolution()
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


            //Clear the buttons and the text from the screen
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in operatorButtonContainer)
            {
                Destroy(child.gameObject);
            }

            // // get mew level
            var newProblem = problemSet.GetNewProblemSet(currentLevel);

        }
        else
        {
            Debug.Log("Incorrect solution.");
        }


    }


}
