using UnityEngine;

public class EscapeToHide : MonoBehaviour
{
    public GameObject targetObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
        }
    }
}