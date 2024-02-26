using UnityEngine;
using UnityEngine.UI; // Add this line
using TMPro;
using System.Collections.Generic;
using ProblemSetSpace;


public class ProblemSetController : MonoBehaviour
{
    private ProblemSet problemSet;


  public Transform buttonContainer; // Add this line
    public Button buttonPrefab; // Add this line




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
        }

    }

     // This method is called when a button is clicked
    void OnButtonClicked(ProblemSelectable selectable)
    {
        Debug.Log("Button clicked: " + selectable);
        // Here you can add the code to handle the button click
    }


    // Call this method when the user submits their solution
    public void CheckSolution(List<ProblemSelectable> userSelectables)
    {
        if (problemSet.IsSolutionCorrect(userSelectables))
        {
            Debug.Log("Correct solution!");
            currentLevel++;
        }
        else
        {
            Debug.Log("Incorrect solution.");
        }


    }


}
