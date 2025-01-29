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
            case 3: // Airlock screen
                SceneManager.LoadScene("AirlockScene");
                break;
            
            case 5: // circuit game
                SceneManager.LoadScene("CircuitGame");
                break;          

            case 8: // flow game
                SceneManager.LoadScene("MaxFlow");
                break;        

            case 10: // knapsack game
                SceneManager.LoadScene("Knapsack");
                break;
            // Add more cases as needed
            default:
                break;
        }
    }

}
