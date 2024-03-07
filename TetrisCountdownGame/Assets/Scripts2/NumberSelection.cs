using System;
using UnityEngine;
using UnityEngine.UI; // Add this line
using TMPro;
using System.Collections.Generic;
using GameStateSpace;
using ProblemSetSpace;

public class ProblemSetController : MonoBehaviour, IGameStateObserver
{
    private ProblemSet problemSet;
    public Transform buttonContainer; 
    public Transform numberButtonContainer; // Container for number buttons
    public Transform operatorButtonContainer; // Container for operator buttons
    public TextMeshProUGUI equationContainer; // Container for the equation
    public Button buttonPrefab; 
    public TextMeshProUGUI targetNumberText; 

    public List<ProblemSelectable> userSelectables = new List<ProblemSelectable>(); 
    
    private int currentLevel = 1;
    private const string _resetTag = "ResetTag";
    private const string _undoTag = "UndoTag";

    public static ProblemSetController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //subscribe to game state
        GameState.Instance.Subscribe(this);
    
        // add listeners to the undo and reset buttons by tag "ResetTag" and "UndoTag"
        GameObject.FindWithTag(_resetTag).GetComponent<Button>().onClick.AddListener(OnResetClicked);
        GameObject.FindWithTag(_undoTag).GetComponent<Button>().onClick.AddListener(OnUndoClicked);
    
        problemSet = new ProblemSet();
        var newProblem = problemSet.GetNewProblemSet(currentLevel);
        Debug.Log("Target Number: " + newProblem.Item1);


        Debug.Log("Selectables: " + string.Join(", ", newProblem.Item2));

        // Set the target number text
        targetNumberText.text = " " + newProblem.Item1; // Add this line



       // Create a button for each number selectable
        foreach (var selectable in newProblem.Item2)
        {
            switch (selectable)
            {
                case Number number:
                {
                    Button button = Instantiate(buttonPrefab, buttonContainer.transform);
                    string buttonText = number.ToString().Split('=')[1].Trim(' ', '}'); // Add this line
                    button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText; // Change this line

                    // Add a click listener to the button
                    button.onClick.AddListener(() => OnButtonClicked(selectable, button));
                    break;
                }
                //Add the buttons to the correct container
                case BinaryOperator binaryOperator:
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
                    button.onClick.AddListener(() => OnButtonClicked(selectable, button));
                    break;
                }
            }
        }

    }
   
    /// <summary>
    /// A method that is called when the game state changes
    /// </summary>
    /// <param name="gameState"></param>
    /// <author>Sebastian Kjallgren, Tom Palin</author>
    public void OnGameStateChanged(GameStateEnum gameState)
    {
        switch (gameState)
        {

            case GameStateEnum.TetrisGameUnsolved:
            //Clear and make a new problem 
            if (buttonContainer != null)
            {
                foreach (Transform child in buttonContainer)
                {
                    Destroy(child.gameObject);
                }
            }
            if (operatorButtonContainer != null)
            {
                foreach (Transform child in operatorButtonContainer)
                {
                    Destroy(child.gameObject);
                }
            }
            Debug.Log("The game state is now TetrisGameUnsolved");
            break;
            
            case GameStateEnum.CountdownBeingSolved:
                userSelectables.Clear();
                equationContainer.text = "";
                
                var newProblem = problemSet.GetNewProblemSet(currentLevel);
                targetNumberText.text = " " + newProblem.Item1;
                
                // Create a button for each number selectable
                foreach (var selectable in newProblem.Item2)
                {
                    AddSelectableToCanvas(selectable);
                }
                break;

         
            default:
                break;
        }

        
    }

    /// <summary>
    /// Adds a selectable to the canvas appropriately
    /// </summary>
    /// <param name="selectable"></param>
    /// <author>Sebastian Kjallgren, Tom Palin</author>
    private void AddSelectableToCanvas(ProblemSelectable selectable)
    {
        switch (selectable)
        {
            case Number number:
            {

                //Do null checks 
                if (buttonContainer == null) return;
                if (buttonPrefab == null) return;
                if (buttonContainer.transform == null) return;

                Button button = Instantiate(buttonPrefab, buttonContainer.transform);
                string buttonText = number.ToString().Split('=')[1].Trim(' ', '}');
                button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
                button.onClick.AddListener(() => OnButtonClicked(selectable, button));
                break;
            }
            case BinaryOperator binaryOperator:
            {

                //Do null checks
                if (operatorButtonContainer == null) return;
                if (buttonPrefab == null) return;
                if (operatorButtonContainer.transform == null) return;
                
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
                }
                button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
                button.onClick.AddListener(() => OnButtonClicked(selectable, button));
                break;
            }
        }
    }


    void OnButtonClicked(ProblemSelectable selectable, Button button)
    {
        Debug.Log("Button clicked: " + selectable);
        userSelectables.Add(selectable);
    
        // if the solution is now correct we change the game state 
        CheckSolution();

        switch (selectable)
        {
            // Add the selected button to the equation
            case Number number:
                Debug.Log("Button clicked: " + selectable);
                // Here you can add the code to handle the button click
                equationContainer.text += number.Value.ToString() + " "; // Append number to the equation

                //Delete the button that was clicked from the screen
                Destroy(button.gameObject);
                break;
            case BinaryOperator binaryOperator:
                switch (binaryOperator.BinOp)
                {
                    case BinaryOperatorType.Addition:
                        equationContainer.text += "+ "; // Append operator to the equation
                        break;
                    case BinaryOperatorType.Subtraction:
                        equationContainer.text += "- "; // Append operator to the equation
                        break;
                    case BinaryOperatorType.Multiplication:
                        equationContainer.text += "* "; // Append operator to the equation
                        break;
                    case BinaryOperatorType.Division:
                        equationContainer.text += "/ "; // Append operator to the equation
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// Method that is called when the undo button is clicked.
    ///
    /// Makes the last selectable available again
    /// </summary>
    /// <author>Sebastian Kjallgren</author>
    public void OnUndoClicked()
    {
        if (userSelectables.Count <= 0) return;
        
        ProblemSelectable lastSelectable = userSelectables[^1];
        userSelectables.RemoveAt(userSelectables.Count - 1);
        
        WriteSelectablesToEquationText();

        // adds it back to the selectable list (if it is a number because operators do not get removed from the screen)
        if (lastSelectable is Number)
        {
            AddSelectableToCanvas(lastSelectable);
        }
    }

    /// <summary>
    /// Overwrites the equation text to whatever the user selectables list is
    /// </summary>
    /// <author>Sebastian Kjallgren</author>
    private void WriteSelectablesToEquationText()
    {
        equationContainer.text = "";
        
        foreach (ProblemSelectable selectable in userSelectables)
        {
            if (selectable is Number number)
            {
                equationContainer.text += number.Value.ToString() + " ";
            }
            else if (selectable is BinaryOperator binaryOperator)
            {
                switch (binaryOperator.BinOp)
                {
                    case BinaryOperatorType.Addition:
                        equationContainer.text += "+ ";
                        break;
                    case BinaryOperatorType.Subtraction:
                        equationContainer.text += "- ";
                        break;
                    case BinaryOperatorType.Multiplication:
                        equationContainer.text += "* ";
                        break;
                    case BinaryOperatorType.Division:
                        equationContainer.text += "/ ";
                        break;
                }
            }
            
        }
    }

    /// <summary>
    /// Method that is called when the reset button is clicked.
    ///
    /// Makes all the selectables available again and clears the equation text.
    /// </summary>
    /// <author>Sebastian Kjallgren</author>
    public void OnResetClicked()
    {
        Debug.Log("Reset clicked");
        
        equationContainer.text = "";
        
        // add all the user selectables back to the screen and clear the user selectables list
        foreach (ProblemSelectable selectable in userSelectables)
        {
            // because the operators do not get removed, we only need to add them if its a number
            if (selectable is Number)
            {
                AddSelectableToCanvas(selectable);
            }
        }
        
        userSelectables.Clear();
    }

    /// <summary>
    /// Checks if the user's solution is correct
    /// </summary>
    /// <author>Sebastian Kjallgren, Tom Palin</author>
    public void CheckSolution()
    {
        if (userSelectables == null) return;

        if (!problemSet.IsSolutionCorrect(userSelectables)) return;
        
        // update the game state to notify the tetris board that the user is allowed to interact with the board
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
    }
}
