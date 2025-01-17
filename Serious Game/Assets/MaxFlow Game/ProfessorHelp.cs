using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfessorHelp : MonoBehaviour
{
    public TextMeshPro helpText; // Reference to the TextMeshPro component
    [TextArea] // Allows for multiline editing in Unity's Inspector
    public string professorHelpMessage; // The message to display when clicked
    
    // Shows and hides the text on click
    void OnMouseDown()
    {
        if (helpText != null)
        {
            // Check if the text is already visible
            if (helpText.text == professorHelpMessage)
            {
                helpText.text = ""; // Hide the text
            }
            else
            {
                helpText.text = professorHelpMessage; // Show the professor's help message
            }
        }
    }

}
