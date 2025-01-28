using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class MainScreen : MonoBehaviour
{
    public static MainScreen Instance { get; private set; }
    public int currentScreen = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this method with, e.g., SetState(3)
    public void SetState(int index)
    {
        currentScreen = index;
        // Safety check
        if (index < 0 || index >= transform.childCount)
        {
            Debug.LogError($"SetState failed: index {index} is out of range.");
            return;
        }

        // Deactivate/Activate children
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void OnPress()
    {
        //based on the current screen, do something
        switch (currentScreen)
        {
            case 0:
                Debug.Log("Screen 0 pressed");
                // Add your logic for screen 0
                break;
            case 1:
                Debug.Log("Screen 1 pressed");
                // Add your logic for screen 1
                break;
            case 2:
                Debug.Log("Screen 2 pressed");
                // Add your logic for screen 2
                break;
            case 3: // Airlock screen
                SceneManager.LoadScene("AirlockScene");
                break;
            // Add more cases as needed
            default:
                Debug.LogWarning("Unknown screen pressed");
                break;
        }
    }

}
