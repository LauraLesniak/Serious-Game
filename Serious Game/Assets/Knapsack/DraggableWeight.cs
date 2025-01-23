using UnityEngine;

public class DraggableWeight : MonoBehaviour
{
    public float weight = 1.0f; // Default weight for the object

    [Range(1, 10)] 
    public int priority = 5; // Priority range (1 = High, 10 = Low)
    private Vector3 spawn;


    void Start()
    {
        spawn = gameObject.transform.position;
    }

public void Respawn()
{
    gameObject.transform.position = spawn; // Return to original position
}

}
