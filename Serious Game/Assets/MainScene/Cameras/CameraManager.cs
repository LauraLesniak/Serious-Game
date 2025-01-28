using UnityEngine.UI;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public int currentCamera = 0;

    public Sprite[] sprites;

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
        currentCamera = index;
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

        RefreshCanvas();
        
        // Save to PlayerPrefs
        // PlayerPrefs.SetInt("ChildStateIndex", index);
        // // Write to disk (optional optimization)
        // PlayerPrefs.Save();
    }

    public void RefreshCanvas()
    {
        // Find the CameraCanvas object
        GameObject canvas = GameObject.Find("CameraCanvas");
        if (canvas == null)
        {
            Debug.LogError("CameraCanvas not found!");
            return;
        }

        //set the first childs Image Component Source Image to appropriate camera
        Image cameraImage = canvas.transform.GetChild(0).GetComponent<Image>();

        //double check indexed sprite not out of bounds
        if (currentCamera < 0 || currentCamera >= sprites.Length)
        {
            Debug.LogError($"RefreshCanvas failed: index {currentCamera} is out of range.");
            return;
        }
        cameraImage.sprite = sprites[currentCamera];

    }

    public void ShowCanvas()
    {
        GameObject canvas = GameObject.Find("CameraCanvas");
        if (canvas == null)
        {
            Debug.LogError("CameraCanvas not found!");
            return;
        }
        canvas.transform.GetChild(0).gameObject.SetActive(true);
        RefreshCanvas();
    }
}
