using UnityEngine;

public class DraggableWeight : MonoBehaviour
{
    public float weight = 1.0f; // Default weight for the object

    [Range(1, 10)] 
    public int priority = 5; // Priority range (1 = High, 10 = Low)
    private Transform spawn;
    private Rigidbody rb;

    void Start()
    {
        spawn = new GameObject("SpawnPoint").transform;
        spawn.position = gameObject.transform.position;
        spawn.rotation = gameObject.transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        gameObject.transform.position = spawn.position; // Return to original position
        gameObject.transform.rotation = spawn.rotation; // Return to original rotation
        if (rb != null)
            {
                rb.velocity = Vector3.zero; // Set velocity to zero
                rb.angularVelocity = Vector3.zero; // Set angular velocity to zero
            }
    }

}
