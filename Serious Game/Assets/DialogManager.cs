using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox; // Assign the dialog box (Panel) in the Inspector
    public Button closeButton;  // Assign the close button in the Inspector

    void Start()
    {
        // Ensure the dialog box is hidden at the start
        dialogBox.SetActive(false);

        // Add the close functionality to the close button
        closeButton.onClick.AddListener(HideDialog);
    }

    public void ShowDialog()
    {
        dialogBox.SetActive(true); // Show the dialog box when the image is clicked
    }

    void HideDialog()
    {
        dialogBox.SetActive(false); // Hide the dialog box when the close button is clicked
    }
}
